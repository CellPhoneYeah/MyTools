using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaYeFeng
{
    public static class CYFStringHelper
    {
        /// <summary>
        /// 将字符串集合用指定字符串分隔，默认为逗号
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string SeparateString(this ICollection<string> source,string separator = ",")
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(separator))
                separator = ",";
            result = string.Join(separator, source.ToArray());
            return result;
        }
    }
}
