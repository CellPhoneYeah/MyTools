using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace UdpClientDemo
{
    public class ClientConfig
    {
        /// <summary>
        /// 本地IP
        /// </summary>
        public static IPAddress LocalAdress;
        /// <summary>
        /// 8802
        /// </summary>
        public static int LocalPort;

        /// <summary>
        /// 本地IP
        /// </summary>
        public static IPAddress ServerAdress;
        /// <summary>
        /// 8801
        /// </summary>
        public static int ServerPort;

        static ClientConfig()
        {
            LocalAdress = IPAddress.Loopback;
            LocalPort = 8802;

            ServerAdress = IPAddress.Loopback;
            ServerPort = 8801;
        }
    }
}
