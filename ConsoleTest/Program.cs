using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using ChaYeFeng;
using TestMEF;
using System.Net;
using TestMEFInterface;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //PluginFactory pf = new PluginFactory();
            //ILog log = pf.GetPlugin<ILog>();
            //Console.WriteLine(log.Prefix());
            try
            {
                string codeString = @" 
                using System;
                using System.Collections;
                using System.Xml;
                using System.IO;
                using System.Windows.Forms;

                namespace CSharpScripter
                {
                    public class MyTest
                    {
                        public static string GetTestString(string param)
                        {
                            string MyStr = ""This is a Dynamic Compiler Demo!"" + param + DateTime.Now;
                            MessageBox.Show(MyStr);
                            return MyStr;
                        }
                    }
                }";
                CompilerParameters compilerParams = new CompilerParameters();
                //编译器选项
                compilerParams.CompilerOptions = "/target:library /optimize";
                //编译时内存输出
                compilerParams.GenerateInMemory = true;
                //生成调试信息
                compilerParams.IncludeDebugInformation = false;
                //添加相关引用
                compilerParams.ReferencedAssemblies.Add("System.dll");
                compilerParams.ReferencedAssemblies.Add("System.Data.dll");
                compilerParams.ReferencedAssemblies.Add("System.Xml.dll");
                compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
                compilerParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                //编译器实例
                ICodeCompiler compiler = new CSharpCodeProvider().CreateCompiler();
                //编译
                CompilerResults compilerResult = compiler.CompileAssemblyFromSource(compilerParams, codeString);
                //创建程序集
                Assembly assembly = compilerResult.CompiledAssembly;
                //获取编译后的类型
                object objTestClass = assembly.CreateInstance("CSharpScripter.MyTest");
                Type myTestClassType = objTestClass.GetType();
                Console.WriteLine(myTestClassType.GetMethod("GetTestString").Invoke(objTestClass, new object[] { "哈哈哈" }));
                Console.WriteLine("执行成功");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }
    }
}
