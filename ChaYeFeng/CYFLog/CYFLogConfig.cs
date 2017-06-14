using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ChaYeFeng
{
    /// <summary>
    /// log的相关配置类
    /// </summary>
    public class CYFLogConfig
    {
        private string _FilePath;

        public string FilePath
        {
            get { return _FilePath; }
        }

        private static CYFLogConfig _Instance = new CYFLogConfig();

        public static CYFLogConfig Instance
        {
            get { return _Instance; }
        }

        private CYFLogConfig()
        {
            string tempStr = null;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("LogPathString"))
            {
                tempStr = ConfigurationManager.AppSettings["LogPathString"];
            }

            if (!string.IsNullOrEmpty(tempStr))
                _FilePath = tempStr;
            else
            {
                if (System.Web.HttpContext.Current == null)
                    //winform程序
                    _FilePath = AppDomain.CurrentDomain.BaseDirectory;
                else
                    //web程序
                    _FilePath = AppDomain.CurrentDomain.BaseDirectory + @"bin\";
            }
        }
    }
}
