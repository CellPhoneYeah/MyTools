using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpClientDemo
{
    /// <summary>
    /// 客户端中用于接收服务端消息
    /// </summary>
    public class ClientReceiver
    {
        public TcpListener listener;
        private TcpClient serverEntity;
        public ClientReceiver()
        {
            listener = new TcpListener(IPAddress.Loopback, 8801);
        }

        public void Start()
        {
            Thread listenThread = new Thread(new ThreadStart(StartListen));
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void StartListen()
        {
            if (listener != null)
            {
                listener.Start();
                while (true)
                {
                    serverEntity = listener.AcceptTcpClient();
                    HandleReceiveMessage(serverEntity.GetStream());
                }
            }
        }

        private void HandleReceiveMessage(NetworkStream stream)
        {
            try
            {
                byte[] msgBytes = new byte[2048];
                int curLength = 0;
                curLength =stream.Read(msgBytes, 0, msgBytes.Length);
                string msgStr = "";
                while (curLength>0)
                {
                    msgStr += Encoding.Default.GetString(msgBytes);
                    curLength = stream.Read(msgBytes, 0, msgBytes.Length);
                }
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                Console.WriteLine(msgStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
