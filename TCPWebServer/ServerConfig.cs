using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TCPWebServer
{
    public class ServerConfig
    {
        public static IPAddress ServerAddress { get; set; }

        public static int ServerPort { get; set; }

        static ServerConfig()
        {
            ServerAddress = IPAddress.Loopback;
            ServerPort = 8899;
        }
    }
}
