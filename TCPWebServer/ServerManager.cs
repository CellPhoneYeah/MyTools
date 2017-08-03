using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPWebServer
{
    public class ServerManager
    {
        ServerListener listener;
        public ServerManager()
        {
            listener = new ServerListener();
        }

        public void Start()
        {
            Console.WriteLine("启动监听：");
            listener.AcceptClientConnect();
            while (true)
            {
                listener.ReceiveMessage();
            }
        }
    }
}
