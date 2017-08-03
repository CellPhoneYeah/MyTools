using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServerManager manager = new ServerManager();
                manager.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
