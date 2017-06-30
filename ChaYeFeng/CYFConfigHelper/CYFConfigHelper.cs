using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;

namespace ChaYeFeng
{
    public class CYFConfigHelper
    {
        public static NameValueCollection AllSettings;
        public static ConnectionStringSettingsCollection AllConnections;

        static CYFConfigHelper()
        {
            AllSettings = ConfigurationManager.AppSettings;
            AllConnections = ConfigurationManager.ConnectionStrings;
        }

        /// <summary>
        /// 获取配置名称的配置值
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static string GetSetting(string settingName)
        {
            if (!AllSettings.AllKeys.Contains(settingName))
                return "";
            string result = string.Empty;
            result = AllSettings[settingName];
            return result;
        }

        /// <summary>
        /// 根据连接名称获取数据连接的字符串
        /// </summary>
        /// <param name="connectName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string connectName)
        {
            ConnectionStringSettings tempSetting = null;
            IEnumerator enumerator = AllConnections.GetEnumerator();
            while(enumerator.MoveNext())
            {
                tempSetting = enumerator.Current as ConnectionStringSettings;
                if (tempSetting == null)
                    continue;
                if (tempSetting.Name == connectName)
                    return tempSetting.ConnectionString;
            }
            return string.Empty;
        }
    }
}
