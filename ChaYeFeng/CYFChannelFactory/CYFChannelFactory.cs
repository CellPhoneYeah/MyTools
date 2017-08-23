using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;

namespace ChaYeFeng
{
    public class CYFChannelFactory
    {
        public static object ExecuteRemoteMethod<T>(string remoteAddress, string methodName, params object[] args)
        {
            WSHttpBinding wsBinding = new WSHttpBinding(SecurityMode.None);
            EndpointAddress address = new EndpointAddress(remoteAddress);
            ChannelFactory<T> factory = new ChannelFactory<T>(wsBinding, address);
            T instance = factory.CreateChannel();
            try
            {
                Type type = typeof(T);
                MethodInfo method = type.GetMethod(methodName);
                return method.Invoke(instance, args);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
