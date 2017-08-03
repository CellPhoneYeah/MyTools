using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UdpServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHandle();
        }

        static void ConsoleHandle()
        {
            PrintReadMe();
            ServerEntity server = new ServerEntity();
            server.StartReceive();
            string CommandStr = Console.ReadLine();
            while (CommandStr.ToLower() != "exit")
            {
                //启动监听
                switch (CommandStr)
                {
                    case "1":
                        server.ToSend();
                        break;
                    default:
                        Console.WriteLine("非法命令");
                        break;
                }
                PrintReadMe();
                CommandStr = Console.ReadLine();
            }
        }

        static void PrintReadMe()
        {
            Console.WriteLine("UDP服务Demo");
            Console.WriteLine("输入【exit】退出");
            Console.WriteLine("输入【1】发送信息");
        }
    }
}
