using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ChaYeFeng
{
    public class DALConfig
    {
        private static DALConfig _instance = new DALConfig();

        public string ConnectionStr
        {
            get
            {
                return _connectionStr;
            }
        }

        private string _connectionStr;

        private DALConfig()
        {
            _connectionStr = ConfigurationManager.AppSettings["ConnectionString"];
        }

        public static DALConfig Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
