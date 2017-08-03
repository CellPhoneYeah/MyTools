using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
namespace UdpClientDemo
{
    public class ClientEntity
    {
        private UdpClient udpClient;
        private UdpClient udpListen;
        public ClientEntity()
        { 
        
        }

        public void SendMessage()
        {
            Console.WriteLine("输入需要发送的数据，按【enter】键发送");
            string message = Console.ReadLine();
            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine("发送信息不能为空");
                return;
            }
            IPEndPoint localEndPoint = new IPEndPoint(ClientConfig.ServerAdress, 8803);
            udpClient = new UdpClient(localEndPoint);
            Thread sendThread = new Thread(ToSend);
            sendThread.IsBackground = true;
            sendThread.Start(message);
        }

        private void ToSend(object message)
        {
            try
            {
                IPEndPoint targetEndPoint = new IPEndPoint(ClientConfig.ServerAdress, ClientConfig.ServerPort);
                byte[] sendBytes = Encoding.Default.GetBytes(message.ToString());
                udpClient.Send(sendBytes, sendBytes.Length, targetEndPoint);
                udpClient.Close();
                Console.WriteLine("发送成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送错误" + ex.Message);
            }
        }

        public void StartListen()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(ClientConfig.LocalAdress, ClientConfig.LocalPort);
            udpListen = new UdpClient(remoteEndPoint);
            Thread listenThread = new Thread(ListenHandle);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ListenHandle()
        {
            IPEndPoint curEndPoint = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                while (true)
                {
                    byte[] receiveBytes = udpListen.Receive(ref curEndPoint);
                    string receiveStr = Encoding.Default.GetString(receiveBytes);
                    Console.WriteLine(string.Format("收到{0}的消息：{1}", curEndPoint.ToString(), receiveStr)); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("侦听错误" + ex.Message);
            }
        }
    }
}
