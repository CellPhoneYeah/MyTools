using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpClientDemo
{
    public class ClientEntity
    {
        private TcpClient _Client;
        public TcpClient Client { get { return _Client; } set { _Client = value; } }

        private NetworkStream curStream;

        private BinaryWriter Writer { get; set; }

        private BinaryReader Reader { get; set; }

        public ClientEntity()
        {
            ClientConfig.ServerAdress = IPAddress.Loopback.ToString();
            ClientConfig.ServerPort = 8899;
        }

        public void StartClient()
        {
            ConnectToServer();
            Console.WriteLine("连接成功，可以对话,输入【exit】退出");
            string msgToSend = Console.ReadLine();
            while (msgToSend.ToLower() != "exit")
            {
                SendMessageToServer(msgToSend);
                msgToSend = Console.ReadLine();
            }
        }


        private void ConnectToServer()
        {
            try
            {
                Thread threadConnect = new Thread(Connect);
                threadConnect.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Connect()
        {
            try
            {
                _Client = new TcpClient();
                if (!_Client.Connected)
                    _Client.Connect(IPAddress.Parse(ClientConfig.ServerAdress), ClientConfig.ServerPort);
                Thread.Sleep(1000);
                if (_Client != null)
                {
                    curStream = _Client.GetStream();
                    Reader = new BinaryReader(curStream);
                    Writer = new BinaryWriter(curStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接失败" + ex.Message);
            }
        }

        private void Send(object message)
        {
            Thread sendThread = new Thread(SendMessageToServer);
            sendThread.Start(message);
        }

        private void SendMessageToServer(object message)
        {
            try
            {
                byte[] msgBytes = Encoding.Default.GetBytes(message.ToString());
                Writer.Write(msgBytes);
                Writer.Flush();
                Console.WriteLine("发送成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送失败："+ex.Message);
            }
        }

    }
}
