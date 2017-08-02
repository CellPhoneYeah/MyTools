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
            TCPClientManager client = new TCPClientManager();
            client.SendMessage("测试一下行不行");
        }
    }
}
