using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketWebClient
{
    class Program
    {
        static Socket socketClient;
        static void Main(string[] args)
        {
            try
            {
                ListenServer();
                socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                while (true)
                {
                    socketClient.Connect(IPAddress.Loopback, 8899);
                    Console.WriteLine("输入要发送的信息");
                    string msgToSend = Console.ReadLine();
                    Thread sendThread = new Thread(SendMessage);
                    sendThread.IsBackground = true;
                    sendThread.Start(msgToSend);
                    Console.ReadLine(); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误");
            }
        }

        private static void ListenServer()
        {
            Socket socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketServer.Bind(new IPEndPoint(IPAddress.Loopback, 8800));
            socketServer.Listen(10);
            Thread listenThread = new Thread(ListentMethod);
            listenThread.IsBackground = true;
            listenThread.Start(socketServer);
        }

        private static void ListentMethod(object socketServer)
        {
            Socket socket = socketServer as Socket;
            while (true)
            {
                Socket serverSocket = socket.Accept();
                byte[] receiveBytes = new byte[0];
                int byteLength = 0;
                byteLength = serverSocket.Receive(receiveBytes);
                string receiveStr = Encoding.Default.GetString(receiveBytes);
                Console.WriteLine("收到消息"+receiveStr);
            }
        }

        private static void SendMessage(object message)
        {
            try
            {
                byte[] msgBytes = Encoding.Default.GetBytes(message.ToString());
                socketClient.Send(msgBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送失败" + ex.Message);
            }
        }
    }
}
