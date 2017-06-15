using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaYeFeng
{
    public class CYFStringValidates
    {
        /// <summary>
        /// 验证字符串是否可以转换成数值
        /// </summary>
        /// <param name="source">原始字符串</param>
        /// <param name="result">转换后的字符串，如果转换失败则为"0"</param>
        /// <returns></returns>
        public static bool GetValidateNum(string source, out string result)
        {
            result = "0";
            string temp = source.Trim();
            if (temp.Count(x => x == '.') > 1)//小数点过多
                return false;
            int pointIndex = temp.IndexOf('.');
            if (pointIndex == 0 && pointIndex == temp.Length - 1)
                temp = "0" + temp + "0";
            else
                if (pointIndex == 0)
                    temp = "0" + temp;
                else
                    if (pointIndex == temp.Length - 1)
                        temp = temp + "0";


            char[] chars = temp.ToCharArray();
            if (chars.Count() <= 0)
            {
                return false;
            }
            foreach (char item in chars)
            {
                if (!char.IsDigit(item) && item != '.')
                {
                    return false;
                }
            }
            result = temp;
            return true;
        }
    }
}
