using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaYeFeng
{
    public class WSWrapProxyFactory
    {
        /// <summary>
        /// wsdl地址，即服务端地址
        /// </summary>
        public string WsdlAdress{get;private set;}

        public WSWrapProxyFactory(string wsdlAdress)
        { 
            this.WsdlAdress = wsdlAdress;
        }

        //public T CreateProxy<T>()
        //{
        //    object newObj = null;//需要返回的服务实例

        //    Type curType = typeof(T);//实例类型

        //    Type newType = 
        //}


    }
}
