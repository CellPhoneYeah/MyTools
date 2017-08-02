using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ChaYeFeng
{
    public class WebServiceCaller
    {
        /// <summary>
        /// 缓存xmlNameSpace,避免重复调用GetNameSpace()
        /// </summary>
        private static Hashtable _xmlNameSpaces = new Hashtable();
        public static XmlDocument QueryPostWebService(string URL, string methodName, Hashtable pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + methodName);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            byte[] data = EncodePars(pars);
            WriteRequestData(request, data);
            return ReadXmlResponse(request.GetResponse());
        }

        public static XmlDocument QueryGetWebService(string URL, string methodName, Hashtable pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + methodName);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            return ReadXmlResponse(request.GetResponse());
        }

        public static XmlDocument QuerySoapWebService(string URL, string methodName, Hashtable pars)
        {
            if (_xmlNameSpaces.ContainsKey(URL))
                return QuerySoapWebService(URL, methodName, pars, _xmlNameSpaces[URL].ToString());
            else
                return QuerySoapWebService(URL, methodName, pars, GetNameSpace(URL));
        }


        private static XmlDocument QuerySoapWebService(string URL, string methodName, Hashtable pars, string xmlNameSpace)
        {
            try
            {
                _xmlNameSpaces[URL] = xmlNameSpace;//加入缓存，提高效率
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/soap+xml; charset=utf-8";
                request.Headers.Add("SOAPAction", "\"" + xmlNameSpace + (xmlNameSpace.EndsWith("/") ? "" : "/") + methodName + "\"");
                SetWebRequest(request);
                byte[] data = EncodeParsToSoap(pars, xmlNameSpace, methodName);
                WriteRequestData(request, data);
                XmlDocument doc = new XmlDataDocument(), doc2 = new XmlDataDocument();
                //string paraSetStr = HttpUtility.UrlEncode("insertName") + "=" + HttpUtility.UrlEncode("123");
                //byte[] paraBytes = Encoding.UTF8.GetBytes(paraSetStr);
                //string temp =
                //    string.Format("POST /GetName.asmx HTTP/1.1\t\rHost: localhost\t\rContent-Type: text/xml; charset=utf-8\t\rContent-Length: {0}\t\rSOAPAction: \"http://tempuri.org/GetInsert\"\t\r<?xml version=\"1.0\" encoding=\"utf-8\"?>\t\r<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">\t\r<soap:Body>\t\r<GetInsert xmlns=\"http://tempuri.org/\">\t\r<insertName>123</insertName>\t\r</GetInsert>\t\r</soap:Body>\t\r</soap:Envelope>",paraBytes.Length);
                //byte[] tempBytes = Encoding.UTF8.GetBytes(temp);
                //request.GetRequestStream().Write(tempBytes,0,tempBytes.Length);

                Stream stream = request.GetRequestStream();
                byte[] contentByte = new byte[stream.Length];
                stream.Read(contentByte, 0, contentByte.Length);
                doc = ReadXmlResponse(request.GetResponse());

                XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
                mgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope");
                string retXml = doc.SelectSingleNode("//soap:body/*/*", mgr).InnerXml;
                doc2.LoadXml("<root>" + retXml + "</root>");
                AddDelaration(doc2);
                return doc2;
                //return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 设置头部信息
        /// </summary>
        /// <param name="doc2"></param>
        private static void AddDelaration(XmlDocument doc2)
        {
            XmlDeclaration decl = doc2.CreateXmlDeclaration("1.0", "urf-8", null);
            doc2.InsertBefore(decl, doc2.DocumentElement);
        }

        /// <summary>
        /// 创建服务的参数信息流
        /// </summary>
        /// <param name="pars">参数表</param>
        /// <param name="xmlNameSpace">命名空间</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        private static byte[] EncodeParsToSoap(Hashtable pars, string xmlNameSpace, string methodName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"></soap:Envelope>");
            AddDelaration(doc);
            XmlElement soapBody = doc.CreateElement("soap:Body");
            XmlElement soapMethod = doc.CreateElement(methodName);
            soapMethod.SetAttribute("xmlns", xmlNameSpace);
            foreach (string k in pars.Keys)
            {
                XmlElement soapPar = doc.CreateElement(k);
                soapPar.InnerXml = ObjectToSoapXml(pars[k]);
                soapMethod.AppendChild(soapPar);
            }
            soapBody.AppendChild(soapMethod);
            doc.DocumentElement.AppendChild(soapBody);
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }

        /// <summary>
        /// 把参数转换SoapXml类型
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string ObjectToSoapXml(object o)
        {
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, o);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));
            if (doc.DocumentElement != null)
                return doc.DocumentElement.InnerXml;
            else
                return o.ToString();
        }

        private static string GetNameSpace(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url+"?WSDL");
            SetWebRequest(request);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            XmlDocument doc = new XmlDataDocument();
            doc.LoadXml(sr.ReadToEnd());
            sr.Close();

            return doc.DocumentElement.Attributes["targetNamespace"].Value;
            //return doc.SelectSingleNode("//targetNamespace").Value;
        }

        /// <summary>
        /// 读取服务响应请求发送的数据流
        /// </summary>
        /// <param name="webResponse"></param>
        /// <returns></returns>
        private static XmlDocument ReadXmlResponse(WebResponse webResponse)
        {
            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            string retXml = sr.ReadToEnd();
            sr.Close();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }

        /// <summary>
        /// 将所调用的方法信息写入请求流数据中
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        private static void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        private static byte[] EncodePars(Hashtable pars)
        {
            return Encoding.UTF8.GetBytes(ParsToString(pars));
        }

        private static string ParsToString(Hashtable pars)
        {
            string result= string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (string k in pars.Keys)
            {
                if (sb.Length > 0)
                    sb.Append("&");
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(pars[k].ToString()));
            }
            result = sb.ToString();
            return result;
        }

        private static void SetWebRequest(HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }
    }
}
