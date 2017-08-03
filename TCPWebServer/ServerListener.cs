using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.IO;
namespace TCPWebServer
{
    public class ServerListener
    {
        private TcpListener serverListener;

        private TcpClient curClient;

        BinaryReader reader;
        BinaryWriter writer;
        public ServerListener()
        {
            serverListener = new TcpListener(ServerConfig.ServerAddress, ServerConfig.ServerPort);
            serverListener.Start();
        }

        public void AcceptClientConnect()
        {
            try
            {
                curClient = serverListener.AcceptTcpClient();
                if (curClient != null)
                {
                    NetworkStream stream = curClient.GetStream();
                    reader = new BinaryReader(stream);
                    writer = new BinaryWriter(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接失败" + ex.Message);
            }
        }

        public void StopListen()
        {
            serverListener.Stop();
        }

        public void ReceiveMessage()
        {
            try
            {
                byte[] receiveBytes = new byte[2048];
                int byteCount = reader.Read(receiveBytes,0,receiveBytes.Length);
                string receiveMsg = Encoding.Default.GetString(receiveBytes, 0, byteCount);
                Console.WriteLine("收到消息：" + receiveMsg);
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine("断开连接" + ex.Message);
            }
        }

        private void SendMessage()
        {
            try
            {
                writer.Write("收到消息");
                writer.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine("对方断开连接" + ex.Message);
            }
        }
    }
}
