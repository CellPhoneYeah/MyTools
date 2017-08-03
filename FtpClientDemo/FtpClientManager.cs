using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPClientDemo
{
    public class FtpClientManager
    {
        private NetworkCredential credential = new NetworkCredential("叶晓峰","0419");
        private IPAddress ServerAdress = IPAddress.Parse("192.168.1.175");
        public FtpClientManager()
        { 
            
        }

        public void SendMessage(string message)
        {
            try
            {
                string uri = "ftp://" + ServerAdress.ToString();
                FtpWebRequest curRequest = GetFtpWebRequest(uri, WebRequestMethods.Ftp.ListDirectoryDetails);
                FtpWebResponse curResponse = (FtpWebResponse)curRequest.GetResponse();
                AnalyResponse(curResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 解析服务器返回的信息
        /// </summary>
        /// <param name="response"></param>
        private void AnalyResponse(FtpWebResponse response)
        {
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string responseStr = reader.ReadToEnd();
            Console.WriteLine("获得服务器返回的消息"+responseStr);
        }

        public FtpWebRequest GetFtpWebRequest(string Uri,string Method)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Uri);
            ftpRequest.Method = Method;
            ftpRequest.KeepAlive = true;
            ftpRequest.UseBinary = true;
            ftpRequest.Credentials = credential;
            return ftpRequest;
        }
    }
}
