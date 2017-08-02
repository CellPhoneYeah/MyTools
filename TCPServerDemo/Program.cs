using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCPServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpServerManager server = new FtpServerManager();
            server.Start();
        }
    }
}
