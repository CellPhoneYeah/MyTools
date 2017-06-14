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
        public static bool Get<TKey, TData>(int cacheTypeKey, TKey key, out TData value)
        { 
            
        }
        #endregion
    }
}
