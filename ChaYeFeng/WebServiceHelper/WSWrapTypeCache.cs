using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaYeFeng
{
    public class WSWrapTypeCache
    {
        #region 静态成员
        static WSWrapTypeCache _instance;
        static object Locker = new object();
        #endregion

        #region 实例成员
        private Dictionary<string, Type> cache;
        #endregion

        #region 静态构造函数
        public static WSWrapTypeCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null)
                            _instance = new WSWrapTypeCache();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region 实例构造函数
        private WSWrapTypeCache()
        {
            cache = new Dictionary<string, Type>();
        }
        #endregion

        /// <summary>
        /// 根据识别id获取缓存中已经记录的服务类型
        /// </summary>
        /// <param name="wsdlAdressTypeName">服务地址+"@"+接口全名（包括命名空间）</param>
        /// <returns></returns>
        public Type Get(string wsdlAdressTypeName)
        {
            if (cache == null)
                return null;
            if (string.IsNullOrEmpty(wsdlAdressTypeName))
                throw new ArgumentNullException("wsdlAdressTypeName");
            if (cache.ContainsKey(wsdlAdressTypeName))
            {
                return cache[wsdlAdressTypeName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 向缓存添加新的接口类型
        /// </summary>
        /// <param name="wsdlAdressTypeName">服务地址+"@"+接口全名（包括命名空间）</param>
        public void Put(string wsdlAdressTypeName,Type type)
        {
            if (!cache.ContainsKey(wsdlAdressTypeName))
            {
                lock (Locker)
                {
                    if (!cache.ContainsKey(wsdlAdressTypeName))
                        cache.Add(wsdlAdressTypeName, type);
                }
            }
        }
    }
}
