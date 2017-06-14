using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace ChaYeFeng
{
    public class CacheManager
    {
        #region 私有字段
        /// <summary>
        /// 缓存器
        /// </summary>
        static Dictionary<int, object> _Dic;
        #endregion
        #region 私有方法
        static CacheManager()
        {
            _Dic = new Dictionary<int, object>();
        }
        /// <summary>
        /// 获取下一个缓存键名
        /// </summary>
        /// <returns></returns>
        private static int _GetNextKey()
        { 
            Random random = new Random();
            int key = random.Next(1, 100000);
            while (_Dic.ContainsKey(key))
            {
                key = random.Next(1, 100000);
            }
            return key;
        }
        /// <summary>
        /// 注册字典
        /// </summary>
        /// <typeparam name="Tkey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <param name="dic">缓存</param>
        /// <returns>缓存类别键名</returns>
        private static int _Register<Tkey, TData>(CacheDictionary<Tkey, TData> dic)
        {
            int key = _GetNextKey();
            _Dic.Add(key, dic);
            return key;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 注册缓存，并返回缓存键值
        /// </summary>
        /// <typeparam name="TKey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <returns>缓存类别键名</returns>
        public static int Register<TKey, TData>()
        {
            return _Register<TKey, TData>(new CacheDictionary<TKey, TData>());
        }
        /// <summary>
        /// 注册缓存，并返回缓存键值
        /// </summary>
        /// <typeparam name="TKey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <param name="sourceDataGetter">单一源数据获取器</param>
        /// <returns>缓存类别键名</returns>
        public static int Register<TKey, TData>(Func<TKey, TData> sourceDataGetter)
        {
            return _Register<TKey, TData>(new CacheDictionary<TKey, TData>(sourceDataGetter));
        }
        /// <summary>
        /// 注册缓存，并返回缓存键值
        /// </summary>
        /// <typeparam name="TKey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <param name="sourceAllDataGetter">所有源数据获取器</param>
        /// <returns>缓存类别键名</returns>
        public static int Register<TKey,TData>(Func<List<TData>> sourceAllDataGetter)
        {
            return _Register(new CacheDictionary<TKey, TData>(sourceAllDataGetter));
        }
        /// <summary>
        /// 获取缓存数据
        /// 如果缓存中不存在则通过SourceGetter获取
        /// 如果通过SourceGetter获取到null对象，则不添加到缓存
        /// </summary>
        /// <typeparam name="TKey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <param name="cacheTypeKey">缓存类别键名</param>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <returns></returns>
        public static bool Get<TKey, TData>(int cacheTypeKey, TKey key, out TData value)
        {
            object obj;
            if (_Dic.TryGetValue(cacheTypeKey,out obj))
            {
                CacheDictionary<TKey,TData> dic = obj as CacheDictionary<TKey,TData>;
                return dic.Get(key,out value);
            }
            value = default(TData);
            return false;
        }
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <typeparam name="TKey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <param name="cacheTypeKey">缓存类别键名</param>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <returns></returns>
        public static bool Add<TKey, TData>(int cacheTypeKey, TKey key, TData value)
        {
            object obj;
            if (_Dic.TryGetValue(cacheTypeKey,out obj))
            {
                CacheDictionary<TKey,TData> dic = obj as CacheDictionary<TKey,TData>;
                return dic.Add(key,value);
            }
            return false;
        }
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <typeparam name="TKey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <param name="cacheTypeKey">缓存类别键名</param>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <returns></returns>
        public static bool Set<TKey, TData>(int cacheTypeKey, TKey key, TData value)
        {
            object obj;
            if (_Dic.TryGetValue(cacheTypeKey,out obj))
            {
                CacheDictionary<TKey,TData> dic = obj as CacheDictionary<TKey,TData>;
                return dic.Set(key,value);
            }
            return false;
        }
        /// <summary>
        /// 通过SourceDataGetter重新加载指定key的值
        /// </summary>
        /// <typeparam name="TKey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <param name="cacheTypeKey">缓存类别键名</param>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static bool Reload<TKey, TData>(int cacheTypeKey, TKey key)
        {
            object obj;
            if (_Dic.TryGetValue(cacheTypeKey,out obj))
            {
                CacheDictionary<TKey,TData> dic = obj as CacheDictionary<TKey,TData>;
                return dic.Reload(key);
            }
            return false;
        }
        /// <summary>
        /// 通过SourceAllDataGetter重新加载指定类型缓存的所有缓存对象
        /// </summary>
        /// <typeparam name="TKey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <param name="cacheTypeKey">缓存类别键名</param>
        /// <returns></returns>
        public static bool Reload<TKey, TData>(int cacheTypeKey)
        {
            object obj;
            if (_Dic.TryGetValue(cacheTypeKey,out obj))
            {
                CacheDictionary<TKey,TData> dic = obj as CacheDictionary<TKey,TData>;
                return dic.ReloadAll();
            }
            return false;
        }
        /// <summary>
        /// 移除键/值
        /// </summary>
        /// <typeparam name="TKey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <param name="cacheTypeKey">缓存类别键名</param>
        /// <param name="key">键名</param>
        /// <returns>返回是否移除成功，如果不存在，则返回false</returns>
        public static bool Remove<TKey, TData>(int cacheTypeKey, TKey key)
        {
            object obj;
            if (_Dic.TryGetValue(cacheTypeKey,out obj))
            {
                CacheDictionary<TKey,TData> dic =obj as CacheDictionary<TKey,TData>;
                return dic.Remove(key);
            }
            return false;
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <typeparam name="TKey">缓存键名类型</typeparam>
        /// <typeparam name="TData">缓存键值类型</typeparam>
        /// <param name="cacheTypeKey">缓存类别键名</param>
        public static void Clear<TKey, TData>(int cacheTypeKey)
        {
            object obj;
            if (_Dic.TryGetValue(cacheTypeKey,out obj))
            {
                CacheDictionary<TKey, TData> dic = obj as CacheDictionary<TKey, TData>;
                dic.Clear();
            }
        }
        public static void ClearAll()
        {
            _Dic.Clear();
        }
        #endregion
    }
}
