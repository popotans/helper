using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Helper.Str
{
    public class RegexHelper
    {
        public static bool IsNum(string s)
        {
            int i = int.MinValue;
            return int.TryParse(s, out i);
        }

        /// <summary>
        /// 匹配页面的图片地址
        /// </summary>
        /// <param name="HtmlCode"></param>
        /// <param name="imgHttp">要补充的http://路径信息</param>
        /// <returns></returns>
        public static List<string> GetImgSrc(string HtmlCode)
        {
            List<string> list = new List<string>();
            string Reg = @"";
            Reg = "(?is)<img.*?src=(['\"]?)(?<url>[^'\" ]+)(?=\\1)[^>]*>";
            foreach (Match m in Regex.Matches(HtmlCode, Reg, RegexOptions.IgnoreCase))
            {
                list.Add(m.Groups["url"].Value);
            }
            return list;
        }
    }
}
