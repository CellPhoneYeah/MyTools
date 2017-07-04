using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;

namespace ChaYeFeng
{
    public class WebServiceHelper
    {
        public static object InvokeWebService(string url, string methodName, object[] args)
        {
            return WebServiceHelper.InvokeWebService(url, null, methodName, args);
        }

        public static object InvokeWebService(string url, string className, string methodName, object[] args)
        {
            string nameSpace = "ChaYeFeng.WebService.DynamicWebCalling";
            if (className == null||className=="")
            {
                className = WebServiceHelper.GetWsClassName(url);
            }

            try
            {
                //获取wsdl
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(nameSpace);

                //生成客户端代理代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider cscp = new CSharpCodeProvider();

                //设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll"); 

                //编译代理类
                CompilerResults cr = cscp.CompileAssemblyFromDom(cplist, ccu);
                if (cr.Errors.HasErrors == true)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (CompilerError error in cr.Errors)
                    {
                        sb.Append(error.ToString());
                        sb.Append(Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例
                Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(nameSpace + "." + className, true, false);
                object obj = Activator.CreateInstance(t);
                MethodInfo mi = t.GetMethod(methodName);

                //调用方法并返回结果
                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message,new Exception(ex.InnerException.StackTrace));
            }
        }

        private static string GetWsClassName(string url)
        {
            string[] parts = url.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
    }
}
