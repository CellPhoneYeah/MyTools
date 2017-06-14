using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel.Composition.Hosting;

namespace ChaYeFeng
{
    /// <summary>
    /// 默认使用配置中的Plugin
    /// </summary>
    public class PluginFactory
    {
        #region 属性和全局变量

        /// <summary>
        /// 插件指定的协定名称
        /// </summary>
        public string PluginName { get; set; }

        /// <summary>
        /// 导出插件的组合容器
        /// </summary>
        private CompositionContainer container;

        /// <summary>
        /// 默认访问部件的路径，当没有显式声明路径时使用"."或".\bin"，分别针对winform程序和web程序
        /// </summary>
        public static string DefaultPath
        {
            get
            {
                if (System.Web.HttpContext.Current == null)
                {
                    return ".";
                }
                else
                {
                    return @".\bin";
                }
            }
            set
            {
                DefaultPath = value;
            }
        }

        /// <summary>
        /// 默认过滤部件格式，当没有显式声明格式时使用"*.dll"
        /// </summary>
        public static string DefaultPattern
        {
            get
            {
                return "*.dll";
            }
            set { DefaultPattern = value; }
        }

        /// <summary>
        /// 默认插件协议名称，当没有显式声明插件协议名称时使用""
        /// </summary>
        public static string DefaultNodeName
        {
            get
            {
                return "";
            }
            set
            {
                DefaultNodeName = value;
            }
        }

        /// <summary>
        /// 导出插件的组合容器
        /// </summary>
        public CompositionContainer Container
        {
            get
            {
                return container;
            }
        }

        #endregion

        /// <summary>
        /// 调用默认Plugin插件
        /// </summary>
        public PluginFactory()
            : this(DefaultNodeName)
        {

        }

        /// <summary>
        /// 初始化一个自定义名称的插件，指定路径，如".\bin";指定格式，如"*.dll"
        /// </summary>
        /// <param name="path">部件路径</param>
        /// <param name="pattern">部件格式</param>
        /// <param name="pluginName"></param>
        public PluginFactory(string pluginValue, string path, string pattern)
        {
            PluginName = pluginValue;
            GenContainer(path, pattern);
        }

        /// <summary>
        /// 初始化一个自定义名称的插件,默认指定路径为当前路径下，对象为所有DLL
        /// </summary>
        /// <param name="pluginName"></param>
        public PluginFactory(string pluginName)
            : this(pluginName, DefaultPath, DefaultPattern)
        {

        }

        /// <summary>
        /// 获取指定路径和格式的插件部件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private CompositionContainer GenContainer(string path, string pattern)
        {
            AggregateCatalog catalog = new AggregateCatalog(new DirectoryCatalog(path, pattern));
            container = new CompositionContainer(catalog);
            return container;
        }

        /// <summary>
        /// 获取导出部件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPlugin<T>()
        {
            List<T> results = new List<T>();
            try
            {
                results = GetPlugins<T>();
                if (results != null && results.Count > 0)
                    return results[0];
                else
                    throw new Exception("GetPlugin<T>()获取导出部件失败");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有导出部件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetPlugins<T>()
        {
            IEnumerable<T> temp = null;
            try
            {
                if (container != null)
                {
                    if (string.IsNullOrEmpty(PluginName))
                    {
                        temp = container.GetExportedValues<T>();
                        if (temp == null || temp.Count() == 0)
                            throw new Exception("类型为：" + typeof(T).Name + "没有找到导出对象");
                    }
                    else
                    {
                        temp = container.GetExportedValues<T>(PluginName);
                        if (temp == null || temp.Count() == 0)
                            throw new Exception("类型为：" + typeof(T).Name + "没有找到指定协定名称为" + PluginName + "的导出对象");
                    }
                }
                if (temp != null && temp.Count() > 0)
                    return temp.ToList();
                else
                    throw new Exception("获取插件：" + typeof(T).Name + "失败");
            }
            catch (Exception ex)
            {
                CYFLog.WriteLog(ex.Message);
                CYFLog.WriteLog("尝试获取默认部件");
                try
                {
                    temp = container.GetExportedValues<T>();
                    if (temp == null || temp.Count() == 0)
                        throw new Exception("尝试获取默认部件失败");
                    else
                        return temp.ToList();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
        }

    }
}
