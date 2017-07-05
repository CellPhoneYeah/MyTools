using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaYeFeng
{
    /// <summary>
    /// 远程调用封装对象
    /// </summary>
    [Serializable]
    public class RemoteCallWrapper
    {
        /// <summary>
        /// 契约/接口全名
        /// </summary>
        public string Contract { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<object> Parameters { get; set; }

        public RemoteCallWrapper()
        {
            this.Parameters = new List<object>();
        }
    }
}
