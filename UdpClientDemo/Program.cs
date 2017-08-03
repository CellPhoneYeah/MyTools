using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UdpClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Print();
            string command = Console.ReadLine();
            ConsoleHandle(command);
        }

        static void ConsoleHandle(string command)
        {
            ClientEntity client = new ClientEntity();
            client.StartListen();
            while (command != "exit")
            {
                switch (command)
                {
                    case "1":
                        client.SendMessage();
                        break;
                    case "2":
                        break;
                    case "exit":
                        return;
                    default:
                        break;
                }
                command = Console.ReadLine();
            }
            
        }

        static void Print()
        {
            Console.WriteLine("UDP客户端测试");
            Console.WriteLine("输入【exit】退出");
            Console.WriteLine("输入【1】发送数据");
        }
    }
}
