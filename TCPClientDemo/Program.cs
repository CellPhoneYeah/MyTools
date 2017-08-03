using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintMsg();
            HandleConsole();
        }

        private static void PrintMsg()
        {
            Console.WriteLine("输入【1】开始通讯");
            Console.WriteLine("输入【exit】退出通讯");
        }

        private static void HandleConsole()
        {
            string ConsoleMsg = Console.ReadLine();
            do
            {
                switch (ConsoleMsg)
                {
                    case "1":
                        ClientEntity client = new ClientEntity();
                        client.StartClient();
                        break;
                    default:
                        break;
                }
            } while (ConsoleMsg.ToLower() != "exit");

        }
    }
}
