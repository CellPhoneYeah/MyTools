using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace UdpServerDemo
{
    public class ServerEntity
    {
        UdpClient sendClient;
        UdpClient receiveClient;
        public ServerEntity()
        { 
            
        }

        /// <summary>
        /// 开始侦听是否有消息
        /// </summary>
        public void StartReceive()
        {
            //创建接收套接字，即要侦听本地的一个端口
            IPEndPoint endPoint = new IPEndPoint(ServerConfig.ListenAddress, ServerConfig.ListenPort);
            receiveClient = new UdpClient(endPoint);

            Thread receiveThread = new Thread(ReceiveHandle);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        private void ReceiveHandle()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any,0);
            try
            {
                while (true)
                {
                    Console.WriteLine("侦听中....");
                    byte[] receiveBytes =
                       receiveClient.Receive(ref remoteEndPoint);
                    string receiveStr = Encoding.Default.GetString(receiveBytes);
                    Console.WriteLine(string.Format("收到来自{0}的：{1}",remoteEndPoint.ToString(), receiveStr)); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("侦听发生错误" + ex.Message);
            }
        }

        public void ToSend()
        {
            Console.WriteLine("输入要发送的内容：");
            string message = Console.ReadLine();
            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine("发送内容不能为空");
                return;
            }
            //匿名发送
            sendClient = new UdpClient(0);
            Thread sendThread = new Thread(SendHandle);
            sendThread.IsBackground = true;
            sendThread.Start(message);
        }

        private void SendHandle(object message)
        {
            IPEndPoint clientEndPoint = new IPEndPoint(ServerConfig.RemoteAddress, ServerConfig.RemotePort);
            try
            {
                byte[] msgBytes = Encoding.Default.GetBytes(message.ToString());
                sendClient.Send(msgBytes, msgBytes.Length,clientEndPoint);
                sendClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送失败：" + ex.Message);
            }
        }

        public void Close()
        {
            receiveClient.Close();
        }
    }
}
