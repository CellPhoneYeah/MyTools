using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ChaYeFeng
{
    public static class CYFStringHelper
    {
        /// <summary>
        /// 将字符串集合用指定字符串分隔，默认为逗号
        /// </summary>
        /// <param name="source">源数据集合</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string SeparateString(this ICollection<string> source,string separator = ",")
        {
            string result = string.Empty;
            if (source == null)
                throw new Exception("不能对空对象执行SeparateString()");
            if (string.IsNullOrEmpty(separator))
                separator = ",";
            result = string.Join(separator, source.ToArray());
            return result;
        }

        /// <summary>
        /// 抽取集合的某一个指定属性值，获取它并用指定符号隔开（默认逗号）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源数据集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string SeparateString<T>(this ICollection<T> source, string propertyName, string separator = ",")
        {
            try
            {
                string result = string.Empty;
                if (source == null)
                    throw new Exception("不能对空对象执行SeparateString<T>()");
                if (string.IsNullOrEmpty(separator))
                    separator = ",";
                Type type = typeof(T);
                PropertyInfo property = type.GetProperty(propertyName);
                if (property == null)
                    throw new Exception("SeparateString<T>()中找不到对应的属性");
                List<string> tempList = source.Select(x => property.GetValue(x, null).ToString()).ToList();
                result = tempList.SeparateString(separator);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
