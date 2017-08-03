using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPServerDemo
{
    public class TcpServerManager
    {
        private IPAddress serverAdress = IPAddress.Parse("192.168.1.175");
        private int serverPort = 9099;
        Thread listenThread;
        TcpListener listener;
        public TcpServerManager()
        {
            listener = new TcpListener(serverAdress, serverPort);
            listenThread = new Thread(new ParameterizedThreadStart(ListenMethod));
            listenThread.IsBackground = true;
        }

        public void Start()
        {
            try
            {
                if (listener != null)
                    listenThread.Start(listener);
                else
                    throw new Exception("服务未初始化");
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ListenMethod(object listener)
        {
            try
            {
                TcpListener curListener = listener as TcpListener;
                curListener.Start();
                if (curListener != null)
                {
                    while (true)
                    {
                        try
                        {
                            TcpClient curClient = curListener.AcceptTcpClient();
                            NetworkStream clientStream = curClient.GetStream();
                            byte[] tempList = new byte[clientStream.Length];
                            clientStream.Read(tempList, 0, tempList.Length);
                            string messageFromClient = Encoding.Default.GetString(tempList);
                            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"\r\n" + messageFromClient);
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
                else
                    throw new Exception("没有初始化服务对象");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
