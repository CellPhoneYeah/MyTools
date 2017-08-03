using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = Console.ReadLine();
            while (message.ToUpper()!="EXIT")
            {
                FtpClientManager client = new FtpClientManager();
                client.SendMessage(message);
                message = Console.ReadLine();
            }
        }
    }
}
