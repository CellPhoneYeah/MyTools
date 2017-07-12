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
using System.Web.Services.Discovery;
using System.Xml.Schema;

namespace ChaYeFeng
{
    /// <summary>
    /// 服务源代码生成工厂
    /// </summary>
    public class CodeFactory
    {
        #region 获取服务端原始wsdl代码
        /// <summary>
        /// 获取服务端返回的wsdl源代码
        /// </summary>
        /// <param name="swdlAdress"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public string GetOriginTypeCode(string swdlAdress, out string originTypeName)
        {
            try
            {
                WebClient client = new WebClient();
                string url = swdlAdress;
                //读取服务端地址的代码内容
                Stream stream = client.OpenRead(url);
                ServiceDescription description = ServiceDescription.Read(stream);
                stream.Close();

                string svcName = description.Services[0].Name;
                string svcNameSpace = "ChaYeFeng";

                if (description.Services.Count == 0)
                    throw new Exception(string.Format("\"{0}\"没有定义服务", swdlAdress));

                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();

                //指定访问协议
                importer.ProtocolName = "Soap";
                //指定生成代码的样式（客户端/服务器）
                importer.Style = ServiceDescriptionImportStyle.Client;
                //设置代码生成的各种选项
                importer.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.None;
                //添加要导入的wsdl文档
                importer.AddServiceDescription(description, null, null);
                //命名空间
                CodeNamespace nameSpace = new CodeNamespace();
                nameSpace.Name = svcNameSpace;
                //代码的容器
                CodeCompileUnit unit = new CodeCompileUnit();
                unit.Namespaces.Add(nameSpace);
                //以编程的方式调用xml web services提供支持的类
                DiscoveryClientProtocol dcp = new DiscoveryClientProtocol();
                //确定url是否为包含有服务说明文档
                dcp.DiscoverAny(url);
                //解析所有发现的文档，xml架构定义和服务说明
                dcp.ResolveAll();
                foreach (object value in dcp.Documents.Values)
                {
                    if (value is ServiceDescription)
                        importer.AddServiceDescription(value as ServiceDescription, null, null);
                    if (value is XmlSchema)
                        importer.Schemas.Add(value as XmlSchema);
                }

                //导入相应的参数来生成代码
                ServiceDescriptionImportWarnings warning = importer.Import(nameSpace, unit);

                //代码生成器
                CSharpCodeProvider provider = new CSharpCodeProvider();

                //调用编译器的参数
                CompilerParameters parameter = new CompilerParameters
                {
                    //是否生成可执行文件
                    GenerateExecutable = false,
                    //是否在内存中生成输出
                    GenerateInMemory = true
                };
                //添加当前项目引用的程序集
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");

                TextWriter stringWriter = new StringWriter();
                //未指定的代码文档对象编译单元生成代码，并使用指定的选项将代码发送到文本编写器
                provider.GenerateCodeFromCompileUnit(unit, stringWriter, null);

                originTypeName = svcName;
                return stringWriter.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        } 
        #endregion

        /// <summary>
        /// 生成类
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetCodeSb(string className,string originTypeName,Type type)
        {
            StringBuilder code = new StringBuilder();

            //添加引用部分
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Text;");
            //添加命名空间
            code.AppendLine("namespace ChaYeFeng");
            code.AppendLine("{ ");
            //添加类的内部内容
            code.AppendLine(string.Format("public class {0}:{1},{2}"
                ,className
                ,originTypeName
                ,type.FullName));
            code.AppendLine("}");
            //获取基类型的所有方法
            List<MethodInfo> methods = new List<MethodInfo>();
            methods.AddRange(type.GetMethods());
            //只获取一层继承的接口
            foreach (Type curType in type.GetInterfaces())
            {
                methods.AddRange(curType.GetMethods());
            }

            //遍历各个方法，获取其签名，作为新类型的方法
            foreach (var curMethod in methods)
            {
                //跳过释放方法
                if (curMethod.Name=="Dispose")
                    continue;

                //获取方法返回值类型的全名
                var returnTypeFullName = GetTypeFullName(curMethod.ReturnType);
                //得到方法的各个参数类型
                var paramInfo = curMethod.GetParameters();
                //添加方法头代码
                if (curMethod.DeclaringType != null)
                    code.AppendLine(string.Format("{0} {1}.{2}("
                        , returnTypeFullName
                        , curMethod.DeclaringType.FullName
                        , curMethod.Name));
                //添加方法的参数代码
                for (var i = 0; i < paramInfo.Length; i++)
                {
                    code.AppendFormat("{0} {1}", GetTypeFullName(paramInfo[i].ParameterType), paramInfo[i].Name);
                    if (i < paramInfo.Length - 1)
                        code.Append(",");
                }
                code.Append(")");
                //方法体代码
                code.AppendLine("{");
                //创建协议信息实体
                code.AppendLine(typeof(RemoteCallWrapper).FullName 
                    + "wrapper = new " 
                    + typeof(RemoteCallWrapper).FullName + "();");
                code.AppendLine("wrapper.Method = \"" + curMethod.Name + "\";");
                code.AppendLine("wrapper.Contract = \"" + type.FullName + "\";");
                code.AppendLine("}");

                for (int i = 0; i < paramInfo.Length; i++)
                {
                    code.AppendLine(string.Format("wrapper.Parameters.Add({0})", paramInfo[i].Name));
                }

                //code.AppendLine("byte[] buff = "+typeof(ObjectBinaryConverter)
            }
            return code;
        }

        /// <summary>
        /// 获取类型的全名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetTypeFullName(Type type)
        {
            string result = string.Empty;
            //如果是泛型，则返回
            if (type.IsGenericType)
            {
                var genType = type.GetGenericArguments();
                result = string.Format("List<{0}>", genType[0].FullName);
            }
            else if (type.FullName == "System.Void")
            {
                result = "void";
            }
            else
                result = type.FullName;
            return result;
        }
    }
}
