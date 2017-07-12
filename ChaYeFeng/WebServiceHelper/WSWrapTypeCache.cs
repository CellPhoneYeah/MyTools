using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaYeFeng
{
    public class WsWrapTypeCache
    {
        #region 静态成员
        static WsWrapTypeCache _instance;
        static readonly object _locker = new object();
        #endregion

        #region 实例成员
        private readonly Dictionary<string, Type> _cache;
        #endregion

        #region 静态构造函数
        public static WsWrapTypeCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                            _instance = new WsWrapTypeCache();
                    }
                    return _instance;
                }
                return _instance;
            }
        }
        #endregion

        #region 实例构造函数
        private WsWrapTypeCache()
        {
            _cache = new Dictionary<string, Type>();
        }
        #endregion

        /// <summary>
        /// 根据识别id获取缓存中已经记录的服务类型
        /// </summary>
        /// <param name="wsdlAdressTypeName">服务地址+"@"+接口全名（包括命名空间）</param>
        /// <returns></returns>
        public Type Get(string wsdlAdressTypeName)
        {
            if (_cache == null)
                return null;
            if (string.IsNullOrEmpty(wsdlAdressTypeName))
                throw new ArgumentNullException("wsdlAdressTypeName");
            return _cache.ContainsKey(wsdlAdressTypeName) ? _cache[wsdlAdressTypeName] : null;
        }

        /// <summary>
        /// 向缓存添加新的接口类型
        /// </summary>
        /// <param name="wsdlAdressTypeName">服务地址+"@"+接口全名（包括命名空间）</param>
        public void Put(string wsdlAdressTypeName,Type type)
        {
            if (!_cache.ContainsKey(wsdlAdressTypeName))
            {
                lock (_locker)
                {
                    if (!_cache.ContainsKey(wsdlAdressTypeName))
                        _cache.Add(wsdlAdressTypeName, type);
                }
            }
        }
    }
}
