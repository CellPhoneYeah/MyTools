using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace UdpServerDemo
{
    public class ServerConfig
    {
        /// <summary>
        /// 侦听的IP地址,使用本地IP
        /// </summary>
        public static IPAddress ListenAddress;
        /// <summary>
        /// 侦听的端口，使用8801
        /// </summary>
        public static int ListenPort;

        /// <summary>
        /// 远程IP地址，使用本地IP
        /// </summary>
        public static IPAddress RemoteAddress;
        /// <summary>
        /// 远程端口，使用8802
        /// </summary>
        public static int RemotePort;
        static ServerConfig()
        {
            ListenAddress = IPAddress.Loopback;
            ListenPort = 8801;
            RemoteAddress = IPAddress.Loopback;
            RemotePort = 8802;
        }
    }
}
