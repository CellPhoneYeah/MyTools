using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaYeFeng
{
    public class CacheDictionary<TKey, TData>
    {
        #region 私有字段
        /// <summary>
        /// 单一元数据获取器
        /// </summary>
        private Func<TKey, TData> _SourceDataGetter;
        /// <summary>
        /// 所有源数据获取器
        /// </summary>
        private Func<List<TData>> _SourceAllDataGetter;
        /// <summary>
        /// 缓存存放的字典对象
        /// </summary>
        private Dictionary<TKey, TData> _Dict;
        /// <summary>
        /// 缓存数据列表对象
        /// </summary>
        private List<TData> _List;
        /// <summary>
        /// 缓存锁，防止读脏数据
        /// </summary>
        private object _Lock;
        #endregion
        #region 公共属性
        /// <summary>
        /// 缓存对象个数
        /// </summary>
        public int Count
        {
            get { return this._Dict.Count; }
        }
        /// <summary>
        /// 缓存数据列表对象
        /// </summary>
        public List<TData> List
        {
            get
            {
                if (this._List.Count < this._Dict.Count)
                {
                    this._List.Clear();
                    foreach (KeyValuePair<TKey, TData> kbp in this._Dict)
                    {
                        this._List.Add(kbp.Value);
                    }
                }
                return this._List;
            }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CacheDictionary()
        {
            this._Dict = new Dictionary<TKey, TData>();
            this._List = new List<TData>();
            this._Lock = new object();
        }
        /// <summary>
        /// 设置数据获取器
        /// </summary>
        /// <param name="sourceDataGetter">单一元数据</param>
        public CacheDictionary(Func<TKey, TData> sourceDataGetter)
            : this()
        {
            if (sourceDataGetter == null)
                throw new ArgumentNullException("sourceDataGetter");
            this._SourceDataGetter = sourceDataGetter;
        }
        public CacheDictionary(Func<List<TData>> sourceAllDataGetter)
            : this()
        {
            if (sourceAllDataGetter == null)
                throw new ArgumentNullException("sourceAllDataGetter");
            this._SourceAllDataGetter = sourceAllDataGetter;
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 获取缓存数据
        /// 如果缓存中不存在，则通过SourceGetter获取
        /// 如果通过SourceGetter获取到null对象，则不添加到缓存
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <returns></returns>
        public bool Get(TKey key, out TData value)
        {
            if (_Dict.TryGetValue(key, out value))
                return true;
            else
            {
                lock (_Lock)
                {
                    if (_Dict.TryGetValue(key, out value))
                        return true;
                    if (_SourceDataGetter == null)
                        return false;
                    TData tempData = _SourceDataGetter(key);
                    if (tempData != null)
                    {
                        _Dict.Add(key, tempData);
                        _List.Add(tempData);
                        value = tempData;
                        return true;
                    }
                    return false;
                }
            }
        }
        /// <summary>
        /// 设置缓存数据
        /// 如果键值已经存在，则返回false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(TKey key, TData value)
        {
            if (_Dict.ContainsKey(key))
                return false;
            else
            {
                lock (_Lock)
                {
                    if (_Dict.ContainsKey(key))
                        return false;
                    else
                    {
                        _Dict.Add(key, value);
                        if (!this._List.Contains(value))
                            this._List.Add(value);
                        return true;
                    }
                }
            }
        }
        /// <summary>
        /// 设置缓存数据
        /// 返回是否设置成功，如果键值存在，则覆盖
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <returns></returns>
        public bool Set(TKey key, TData value)
        {
            lock (_Lock)//无论有没有都要操作，那就直接锁住资源再判断
            {
                if (_Dict.ContainsKey(key))
                {
                    TData oldData = _Dict[key];
                    _List.Remove(oldData);
                    _Dict[key] = value;
                    _List.Add(value);
                    return true;
                }
                else
                {
                    _Dict.Add(key, value);
                    if (!this._List.Contains(value))
                        _List.Add(value);
                    return true;
                }
            }
        }
        /// <summary>
        /// 通过SourceDataGetter重新加载指定key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Reload(TKey key)
        {
            if (_SourceDataGetter == null)
                return false;
            TData tempData = _SourceDataGetter(key);
            return this.Set(key, tempData);
        }
        /// <summary>
        /// 通过_SourceAllDataGetter重新加载所有值
        /// </summary>
        /// <returns></returns>
        public bool ReloadAll()
        {
            if (_SourceAllDataGetter == null)
                return false;
            lock (this._Lock)
            {
                this._List = _SourceAllDataGetter();
            }
            return true;
        }
        /// <summary>
        /// 移除指定键名缓存
        /// 成功返回true，失败false
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            TData tempData;
            if (this._Dict.ContainsKey(key))
            {
                lock (this._Lock)
                {
                    if (this._Dict.ContainsKey(key))
                    {
                        tempData = this._Dict[key];
                        this._Dict.Remove(key);
                        if (this._List.Contains(tempData))
                            this._List.Remove(tempData);
                        return true;
                    }
                    else
                        return false;
                }
            }
            else
                return false;
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            lock (this._Lock)
            {
                this._List.Clear();
                this._Dict.Clear();
            }
        }
        #endregion
    }
}
