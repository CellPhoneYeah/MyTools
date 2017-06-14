using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace ChaYeFeng
{
    public class CYFDALConfig
    {
        private static CYFDALConfig _instance = new CYFDALConfig();

        private string _connectionStr;

        /// <summary>
        /// 在Instance中才能访问的连接字符串
        /// </summary>
        public string ConnectionStr
        {
            get
            {
                return _connectionStr;
            }
        }

        public static CYFDALConfig Instance
        {
            get
            {
                return _instance;
            }
        }

        private CYFDALConfig()
        {
            string tempStr = string.Empty;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("ConnectionString"))
            {
                tempStr = ConfigurationManager.AppSettings["ConnectionString"];
            }
            if (string.IsNullOrEmpty(tempStr))
                CYFLog.WriteLog("没有在配置中初始化ConnectionString");
            else
                _connectionStr = tempStr;
        }
    }
}
