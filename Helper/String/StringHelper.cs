using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Helper.Str
{
    public class StringHelper
    {
        public static string CreateUniqueIDWithBase64()
        {
            Guid id = Guid.NewGuid();
            DateTime begin = new DateTime(2013, 9, 1);
            TimeSpan ts = DateTime.Now - begin;
            string s = (int)ts.TotalMinutes + Convert.ToBase64String(id.ToByteArray()).Replace("+", "!").Replace("/", "$");
            return s;
        }

        public static string CreateUniqueIDWithComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(2013, 9, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build    
            //the byte string    
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array        
            // Note that SQL Server is accurate to 1/300th of a    
            // millisecond so we divide by 3.333333    
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering    
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid    
            Array.Copy(daysArray, daysArray.Length - 2, guidArray,
              guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray,
              guidArray.Length - 4, 2);
            Guid guid = new Guid(guidArray);
            //string s = guid.ToString("N");
            //string last12 = DateTime.Now.ToString("yyMMddHHmmssff");// s.Substring(s.Length - 12);
            //string nefors = s.Substring(0, s.Length - 14);
            //string rs = string.Format("{0}{1}", last12, nefors);
            return guid.ToString("N");

        }

        public static int ConvertToInt(string s, int drefaultv)
        {
            if (string.IsNullOrEmpty(s)) return drefaultv;
            int v = 0;
            if (int.TryParse(s, out v)) return v;
            return drefaultv;
        }

        public static string GetRnd(int length)
        {
            string chars = "abcdefghjkmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            StringBuilder buider = new StringBuilder(length);

            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                buider.Append(chars[(int)rnd.Next(0, chars.Length)]);
            }
            return buider.ToString();
        }
        public static string GetRndNoNum(int length)
        {
            string chars = "abcdefghjkmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
            StringBuilder buider = new StringBuilder(length);
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                buider.Append(chars[(int)rnd.Next(0, chars.Length)]);
            }
            return buider.ToString();
        }
        static Random rnd = new Random();
        public static string GetRndOnlyNum(int length)
        {
            string chars = "1234567890";
            StringBuilder buider = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                buider.Append(chars[(int)rnd.Next(0, chars.Length)]);
            }
            return buider.ToString();
        }

        public static string StripHtml(string html)
        {
            if (!string.IsNullOrEmpty(html))
            {
                string StrNohtml = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
                StrNohtml = System.Text.RegularExpressions.Regex.Replace(StrNohtml, "&[^;]+;", "");
                return StrNohtml;
            }
            return html;
        }

        public static int GetStrLength(string str)
        {
            return Regex.Replace(str, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length;
        }
        //我是大中国人民啊abc
        public static string CutString(string source, int bytelength)
        {
            if (string.IsNullOrEmpty(source)) return source;
            if (bytelength % 2 != 0) bytelength -= 1;
            StringBuilder sb = new StringBuilder(bytelength);
            char[] arr = source.ToCharArray();
            foreach (char c in arr)
            {
                if (GetStrLength(sb.ToString()) < bytelength) { sb.Append(c); }
            }

            return sb.ToString();
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length">字节长度</param>
        /// <param name="ends">附加的结尾字符</param>
        /// <returns></returns>
        public static string CutString(string source, int length, string ends)
        {
            int strw = 0, j = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];
                if ((int)c >= 0x4E00 && (int)c <= 0x9FA5)
                {
                    strw = 2;
                }
                else strw = 1;
                j += strw;
                if (string.IsNullOrEmpty(ends))
                {
                    if (j > length) break;
                }
                else
                {
                    if (j - ends.Length > length) { sb.Append(ends); break; }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        #region  filter and replace
        /// <summary>
        /// 过滤字符串中的日文字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FilterJapanstr(string s)
        {
            string rs = "";
            s = s.Replace("ゴ", "?");
            s = s.Replace("ガ", "?");
            s = s.Replace("ギ", "?");
            s = s.Replace("グ", "?");
            s = s.Replace("ゲ", "?");
            s = s.Replace("ザ", "?");
            s = s.Replace("ジ", "?");
            s = s.Replace("ズ", "?");
            s = s.Replace("ヅ", "?");
            s = s.Replace("デ", "?");
            s = s.Replace("ド", "?");
            s = s.Replace("ポ", "?");
            s = s.Replace("ベ", "?");
            s = s.Replace("プ", "?");
            s = s.Replace("ビ", "?");
            s = s.Replace("パ", "?");
            s = s.Replace("ヴ", "?");
            s = s.Replace("ボ", "?");
            s = s.Replace("ペ", "?");
            s = s.Replace("ブ", "?");
            s = s.Replace("ピ", "?");
            s = s.Replace("バ", "?");
            s = s.Replace("ヂ", "?");
            s = s.Replace("ダ", "?");
            s = s.Replace("ゾ", "?");
            s = s.Replace("ゼ", "?");
            s = s.Replace("~", "-");
            s = s.Replace("、", ",");
            //の灬amp;
            s = s.Replace("の", "");
            s = s.Replace("灬", "");
            s = s.Replace("amp;", "");
            s = s.Replace("メ", "");
            s = s.Replace("だ", "");
            foreach (char c in s)
            {
                if (c >= 0x3040 && c <= 0x309F) { }
                else if (c >= 0x30A0 && c <= 0x30FF) { }
                else
                {
                    rs += c;
                }
            }

            return rs;
        }

        #endregion


        #region 正则 提取

        /// <summary>
        /// 获取页面的链接正则
        /// </summary>
        /// <param name="HtmlCode"></param>
        /// <returns></returns>
        public static List<string> GetHref(string HtmlCode)
        {
            string Reg = @"(h|H)(r|R)(e|E)(f|F) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)('|""| *|>)?";
            List<string> list = new List<string>();
            foreach (Match m in Regex.Matches(HtmlCode, Reg))
            {
                list.Add((m.Value).ToLower().Replace("href=", "").Replace("\"", "").Replace("'", "").Trim());
            }
            return list;
        }

        /// <summary>
        /// Validates an email address.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmailValid(string email)
        {
            Regex emailRegex = new Regex(
               @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(email))
            {
                return emailRegex.IsMatch(email.Trim());
            }

            return false;
        }

        /// <summary>
        /// Validates an IPv4 address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsIpV4AddressValid(string address)
        {

            if (!string.IsNullOrEmpty(address))
            {
                Regex validIpV4AddressRegex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$", RegexOptions.IgnoreCase);
                return validIpV4AddressRegex.IsMatch(address.Trim());
            }

            return false;
        }
        #endregion

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            if (str == null)
            {
                str = string.Empty;
            }
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        /// <summary>
        /// 金额转换为中文大写
        /// </summary>
        /// <param name="strAmount"></param>
        /// <returns></returns>
        public static string MoneyToChinese(string strAmount)
        {
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数
            if (strAmount.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                strAmount = strAmount.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            strAmount = Math.Round(double.Parse(strAmount), 2).ToString();
            if (strAmount.IndexOf(".") > 0)
            {
                if (strAmount.IndexOf(".") == strAmount.Length - 2)
                {
                    strAmount = strAmount + "0";
                }
            }
            else
            {
                strAmount = strAmount + ".00";
            }
            strLower = strAmount;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            functionReturnValue = strUpper;

            if (IsNegative == true)
            {
                return "负" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }

        /// <summary>
        /// 将时间换成中文
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public static string DateToChineseString(DateTime datetime)
        {
            TimeSpan ts = DateTime.Now - datetime;
            //    System.Web.HttpContext.Current.Response.Write(ts.TotalDays);
            if ((int)ts.TotalDays >= 365)
            {
                return (int)ts.TotalDays / 365 + "年前";
            }
            if ((int)ts.TotalDays >= 30 && ts.TotalDays <= 365)
            {
                return (int)ts.TotalDays / 30 + "月前";
            }
            if ((int)ts.TotalDays == 1)
            {
                return "昨天";
            }
            if ((int)ts.TotalDays == 2)
            {
                return "前天";
            }
            if ((int)ts.TotalDays >= 3 && ts.TotalDays <= 30)
            {
                return (int)ts.TotalDays + "天前";
            }
            if ((int)ts.TotalDays == 0)
            {
                if ((int)ts.TotalHours != 0)
                {
                    return (int)ts.TotalHours + "小时前";
                }
                else
                {
                    if ((int)ts.TotalMinutes == 0)
                    {
                        return "1分钟前";
                    }
                    else
                    {
                        return (int)ts.TotalMinutes + "分钟前";
                    }
                }
            }
            return datetime.ToString("yyyy年MM月dd日 HH:mm");
        }
        /// <summary>
        /// 获得文件的名称
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            return GetFileName(false, filePath);
        }
        /// <summary>
        /// 获得文件的名称
        /// </summary>
        /// <param name="isUrl">是否是网址</param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(bool isUrl, string filePath)
        {
            if (isUrl)
                return filePath.Substring(filePath.LastIndexOf("/") + 1, filePath.Length - filePath.LastIndexOf("/") - 1);
            else
                return filePath.Substring(filePath.LastIndexOf("\\") + 1, filePath.Length - filePath.LastIndexOf("\\") - 1);
        }
        /// <summary>
        /// 获得文件的后缀
        /// 不带点，小写
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileExt(string filePath)
        {
            return filePath.Substring(filePath.LastIndexOf(".") + 1, filePath.Length - filePath.LastIndexOf(".") - 1).ToLower();
        }

        #region other
        public static string ShortUrl(string url)
        {
            //可以自定义生成MD5加密字符传前的混合KEY 
            string key = "Rash";
            //要使用生成URL的字符 
            string[] chars = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            //对传入网址进行MD5加密
            string hex = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key + url, "md5");
            string[] resUrl = new string[4];
            for (int i = 0; i < 4; i++)
            {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
                int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16); string outChars = string.Empty; for (int j = 0; j < 6; j++)
                {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                    int index = 0x0000003D & hexint;
                    //把取得的字符相加 
                    outChars += chars[index];
                    //每次循环按位右移5位 
                    hexint = hexint >> 5;
                }
                //把字符串存入对应索引的输出数组 
                resUrl[i] = outChars;
            }
            string rs = "";
            foreach (string s in resUrl)
            {
                rs += s;
            }
            return rs;
        }

        /// <summary>
        /// 截取beginstr和endstr之间的字符串
        /// </summary>
        /// <param name="source">原字符串</param>
        /// <param name="beginStr">开始字符串</param>
        /// <param name="endStr">结束字符串</param>
        /// <returns></returns>
        public static string SubStr(string source, string beginStr, string endStr)
        {
            int start = source.IndexOf(beginStr);
            int end = source.IndexOf(endStr);
            string rs = "";

            if (end - start <= 0) return "";

            rs = source.Substring(start + beginStr.Length, end - start - beginStr.Length);
            return rs;
        }

        /// <summary>
        /// Gets the sub domain.
        /// </summary>
        /// <param name="url">
        /// The URL to get the sub domain from.
        /// </param>
        /// <returns>
        /// The sub domain.
        /// </returns>
        public static string GetSubDomain(Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                var host = url.Host;
                if (host.Split('.').Length > 2)
                {
                    var lastIndex = host.LastIndexOf(".");
                    var index = host.LastIndexOf(".", lastIndex - 1);
                    return host.Substring(0, index);
                }
            }
            return null;
        }

        /// <summary>
        /// 生成不重复的随机数
        /// </summary>
        /// <param name="Count">生成的数量</param>
        /// <returns></returns>
        static public int[] CreateRandomNumbers(int Count)
        {
            int[] arr = new int[Count];
            int max2 = Count + 1;
            Dictionary<int, bool> dy = new Dictionary<int, bool>(Count);

            Random rd = new Random();
            int num;
            for (int i = 0; i < arr.Length; )
            {
                num = rd.Next(0, Count + 1);
                if (!dy.ContainsKey(num))
                {
                    dy.Add(num, true);
                    arr[i] = num;
                    i++;
                }
            }
            dy.Clear();
            return arr;
        }

        /// <summary>
        /// 在1--maxVal 范围内生成count个不重复的随机数
        /// </summary>
        /// <param name="maxVal"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] CreateRandomNumbers(int maxVal, int count)
        {
            int[] arr = new int[count];
            Random rnd = new Random();
            List<int> list = new List<int>();
            for (int i = 0; i < maxVal + 1; )
            {
                int iq = rnd.Next(1, maxVal);
                if (!list.Contains(iq))
                {
                    list.Add(iq);
                    arr[i] = iq;
                    i++;
                    if (i == count) break;
                }
            }
            list.Clear();
            return arr;
        }

        /// <summary>
        /// Generates random password for password reset
        /// </summary>
        /// <returns>
        /// Random password
        /// </returns>
        public static string RandomPassword()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var password = string.Empty;
            var random = new Random();

            for (var i = 0; i < 8; i++)
            {
                var x = random.Next(1, chars.Length);
                if (!password.Contains(chars.GetValue(x).ToString()))
                {
                    password += chars.GetValue(x);
                }
                else
                {
                    i--;
                }
            }

            return password;
        }

        static RegexOptions RegexOptions = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);
        /// <summary>
        /// 去除source中的超级链接
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RemoveALink(string source)
        {
            string pattern = "(?s)(?i)<a.*?>(?<title>.*?)</a>";
            MatchCollection mc = Regex.Matches(source, pattern, RegexOptions);
            Regex reg = new Regex(pattern);
            foreach (Match m in mc)
            {
                string title = m.Groups["title"].Value;
                //source = Regex.Replace(source, pattern, title);
                source = reg.Replace(source, title, 1);
            }
            return source;
        }

        /// <summary>
        /// 从字典kwdDic 中为内容source加上超链接
        /// </summary>
        /// <param name="kwdDic"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string AddKeywordsALink(Dictionary<string, string> kwdDic, string source)
        {
            List<string> keys = new List<string>();
            foreach (KeyValuePair<string, string> item in kwdDic)
            {
                if (!keys.Contains(item.Key))
                {
                    keys.Add(item.Key);
                }
            }
            keys = keys.OrderBy(x => x.Length).ToList();

            Regex reg = null;
            foreach (string s in keys)
            {
                string to = "<a href='" + kwdDic[s] + "'>" + s + "</a>";
                reg = new Regex(s + "(?!</a>)", RegexOptions);
                source = reg.Replace(source, to, 1);
            }

            return source;
        }
        #endregion

        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        public static string StrToXml(string str)
        {
            string strTemp = str;
            strTemp = strTemp.Replace("&amp;", "&");
            strTemp = strTemp.Replace("&lt;", "<");
            strTemp = strTemp.Replace("&gt;", ">");
            strTemp = strTemp.Replace("&apos;", "\'");
            strTemp = strTemp.Replace("&quot;", "\"");
            return strTemp;
        }

        public static string XmlToStr(string xmlstr)
        {
            string strTemp = xmlstr;
            strTemp = strTemp.Replace("&", "&amp;");
            strTemp = strTemp.Replace("<", "&lt;");
            strTemp = strTemp.Replace(">", "&gt;");
            strTemp = strTemp.Replace("\'", "&apos;");
            strTemp = strTemp.Replace("\"", "&quot;");
            return strTemp;
        }
    }
}
