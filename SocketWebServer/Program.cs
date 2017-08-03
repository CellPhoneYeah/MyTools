using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Loopback;
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8899);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(10);
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                byte[] receiveByte = new byte[2048];
                clientSocket.Receive(receiveByte, 2048, SocketFlags.None);
                string receiveStr = Encoding.Default.GetString(receiveByte);
                Console.WriteLine("收到客户端信息:" + receiveStr);
                string responseStr = "响应客户端返回的信息";
                byte[] responseBytes = Encoding.Default.GetBytes(responseStr);
                clientSocket.Send(responseBytes);
                clientSocket.Close();
                Console.Read();
            }
        }
    }
}
