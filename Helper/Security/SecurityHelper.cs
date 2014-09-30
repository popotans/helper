using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Helper.Security
{
    public class SecurityHelper
    {
        public static string MD5(string str)
        {
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(str);
            bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string str2 = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str2 = str2 + bytes[i].ToString("x").PadLeft(2, '0');
            }
            return str2;
        }

        public static string MD516(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
            return t2 = t2.Replace("-", "");
        }

        public static string ToBase64(string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            string result = "";
            result = Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes(source));
            return result;
        }
        public static string FromBase64(string base64Source)
        {
            if (string.IsNullOrEmpty(base64Source)) return string.Empty;

            try
            {
                return Encoding.GetEncoding("utf-8").GetString(Convert.FromBase64String(base64Source));
            }
            catch (Exception)
            {
                return "Convert Error!";
            }
        }

        /// <summary>
        /// SHA1加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string SHA1(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            return enText.ToString();
        }

        /// <summary>
        /// Encrypts a string using the SHA256 algorithm.
        /// </summary>
        /// <param name="plainMessage">
        /// The plain Message.
        /// </param>
        /// <returns>
        /// The hash password.
        /// </returns>
        public static string HashPassword(string plainMessage)
        {
            var data = Encoding.UTF8.GetBytes(plainMessage);
            using (HashAlgorithm sha = new SHA256Managed())
            {
                sha.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(sha.Hash);
            }
        }

        #region 进制转换

        /// <summary>
        /// 把十进制转换为toBase指定的进制数
        /// </summary>
        /// <param name="tenvalue"></param>
        /// <param name="toBase"></param>
        /// <returns></returns>
        public static string ConvertFromTen(Int64 tenvalue, int toBase)
        {
            string rs = string.Empty;
            Int64 result = tenvalue;
            while (result > 0)
            {
                Int64 val = result % toBase;
                rs = Convert1((int)val).ToString() + rs;
                result = (Int64)(result / toBase);
            }
            return rs;
        }

        /// <summary>
        /// 把toBase表示的进制数转换为十进制
        /// </summary>
        /// <param name="str"></param>
        /// <param name="toBase"></param>
        /// <returns></returns>
        public static Int64 ConvertToTen(string str, int toBase)
        {
            int length = str.Length;
            Int64 result = 0;
            for (int i = 0; i < length; i++)
            {
                Int64 val = (Int64)Math.Pow(toBase, (length - i - 1));
                char c = str[i];
                Int64 tmp = Convert1(c);
                result += tmp * val;
            }
            return result;
        }

        /// <summary>
        /// 10进制转换为62进制
        /// </summary>
        /// <param name="tenvalue"></param>
        /// <returns></returns>
        public static string ConvertTenToSixtyTwo(Int64 tenvalue)
        {
            string rs = string.Empty;
            Int64 result = tenvalue;
            while (result > 0)
            {
                Int64 val = result % 62;
                rs = Convert1((int)val).ToString() + rs;
                result = (Int64)(result / 62);
            }
            return rs;
        }

        /// <summary>
        /// 62进制转换为10进制
        /// </summary>
        /// <param name="sixtytwo"></param>
        /// <returns></returns>
        public static Int64 ConvertSixtytwoToTen(string sixtytwo)
        {
            int length = sixtytwo.Length;
            Int64 result = 0;
            for (int i = 0; i < length; i++)
            {
                Int64 val = (Int64)Math.Pow(62, (length - i - 1));
                char c = sixtytwo[i];
                Int64 tmp = Convert1(c);
                result += tmp * val;
            }
            return result;
        }

        private static int Convert1(char c)
        {
            switch (c)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;

                case 'a':
                    return 10;
                case 'b':
                    return 11;
                case 'c':
                    return 12;
                case 'd':
                    return 13;
                case 'e':
                    return 14;
                case 'f':
                    return 15;
                case 'g':
                    return 16;
                case 'h':
                    return 17;
                case 'i':
                    return 18;
                case 'j':
                    return 19;
                case 'k':
                    return 20;
                case 'l':
                    return 21;
                case 'm':
                    return 22;
                case 'n':
                    return 23;
                case 'o':
                    return 24;
                case 'p':
                    return 25;
                case 'q':
                    return 26;
                case 'r':
                    return 27;
                case 's':
                    return 28;
                case 't':
                    return 29;
                case 'u':
                    return 30;
                case 'v':
                    return 31;
                case 'w':
                    return 32;
                case 'x':
                    return 33;
                case 'y':
                    return 34;
                case 'z':
                    return 35;

                case 'A':
                    return 36;
                case 'B':
                    return 37;
                case 'C':
                    return 38;
                case 'D':
                    return 39;
                case 'E':
                    return 40;
                case 'F':
                    return 41;
                case 'G':
                    return 42;
                case 'H':
                    return 43;
                case 'I':
                    return 44;
                case 'J':
                    return 45;
                case 'K':
                    return 46;
                case 'L':
                    return 47;
                case 'M':
                    return 48;
                case 'N':
                    return 49;
                case 'O':
                    return 50;
                case 'P':
                    return 51;
                case 'Q':
                    return 52;
                case 'R':
                    return 53;
                case 'S':
                    return 54;
                case 'T':
                    return 55;
                case 'U':
                    return 56;
                case 'V':
                    return 57;
                case 'W':
                    return 58;
                case 'X':
                    return 59;
                case 'Y':
                    return 60;
                case 'Z':
                    return 61;
                default:
                    return 0;
            }


        }

        private static char Convert1(int val)
        {
            switch (val)
            {
                case 0:
                    return '0';
                case 1:
                    return '1';
                case 2:
                    return '2';
                case 3:
                    return '3';
                case 4:
                    return '4';
                case 5:
                    return '5';
                case 6:
                    return '6';
                case 7:
                    return '7';
                case 8:
                    return '8';
                case 9:
                    return '9';

                case 10:
                    return 'a';
                case 11:
                    return 'b';
                case 12:
                    return 'c';
                case 13:
                    return 'd';
                case 14:
                    return 'e';
                case 15:
                    return 'f';
                case 16:
                    return 'g';
                case 17:
                    return 'h';
                case 18:
                    return 'i';
                case 19:
                    return 'j';
                case 20:
                    return 'k';
                case 21:
                    return 'l';
                case 22:
                    return 'm';
                case 23:
                    return 'n';
                case 24:
                    return 'o';
                case 25:
                    return 'p';
                case 26:
                    return 'q';
                case 27:
                    return 'r';
                case 28:
                    return 's';
                case 29:
                    return 't';
                case 30:
                    return 'u';
                case 31:
                    return 'v';
                case 32:
                    return 'w';
                case 33:
                    return 'x';
                case 34:
                    return 'y';
                case 35:
                    return 'z';

                case 36:
                    return 'A';
                case 37:
                    return 'B';
                case 38:
                    return 'C';
                case 39:
                    return 'D';
                case 40:
                    return 'E';
                case 41:
                    return 'F';
                case 42:
                    return 'G';
                case 43:
                    return 'H';
                case 44:
                    return 'I';
                case 45:
                    return 'J';
                case 46:
                    return 'K';
                case 47:
                    return 'L';
                case 48:
                    return 'M';
                case 49:
                    return 'N';
                case 50:
                    return 'O';
                case 51:
                    return 'P';
                case 52:
                    return 'Q';
                case 53:
                    return 'R';
                case 54:
                    return 'S';
                case 55:
                    return 'T';
                case 56:
                    return 'U';
                case 57:
                    return 'V';
                case 58:
                    return 'W';
                case 59:
                    return 'X';
                case 60:
                    return 'Y';
                case 61:
                    return 'Z';
            }
            return '0';
        }
        #endregion

        private static string checkKeyLength(string key)
        {
            string origal = "!zIpuTdR@sEf=as6FygUoW8kQmGhlXcV";
            int length = key.Length;
            if (length < 32)
            {
                key += origal.Substring(0, 32 - length);
            }
            else if (length > 32)
            {
                key = MD5(key);
            }
            return key;
        }

        public static string Base62Encrypt(long n)
        {
            string number = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = string.Empty;
            int length = number.Length;
            while (n / length >= 1)
            {
                result = number[(int)(n % length)] + result;
                n /= length;
            }
            result = number[(int)n] + result;
            return result;
        }

        public long Base62Decrypt(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;
            long result = 0;
            string number = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            s = s.Trim();
            int length = s.Length;
            int m = number.Length;
            for (int x = 0; x < length; x++)
            {
                result += number.IndexOf(s[length - 1 - x]) * (long)Math.Pow(m, x);
            }

            return result;
        }


        /// <summary>
        /// aes  2.0 返回62 进制
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AESEncrypt62(string toEncrypt, string key)
        {
            key = checkKeyLength(key);
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in resultArray)
            {
                //sb.AppendFormat("{0:X2}", b);
                sb.Append(ConvertTenToSixtyTwo(b).PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        /// <summary>
        /// aes解密 2.0 由62进制解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AESDecrypt62(string toDecrypt, string key)
        {
            key = checkKeyLength(key);
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            //  byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            int halfInputLength = toDecrypt.Length / 2;
            byte[] toDecryptArray = new byte[halfInputLength];
            for (int x = 0; x < halfInputLength; x++)
            {
                Int64 i = ConvertSixtytwoToTen(toDecrypt.Substring(x * 2, 2));
                toDecryptArray[x] = (byte)i;
            }
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string AESDecrypt62(string toDecrypt)
        {
            return AESDecrypt62(toDecrypt, "as%^ksdjfhsdf=@aslkfdjupaocv");
        }
        public static string AESEncrypt62(string toEncrypt)
        {
            return AESEncrypt62(toEncrypt, "as%^ksdjfhsdf=@aslkfdjupaocv");
        }


        public static string Encrypt62(string source)
        {
            byte[] resultArray = Encoding.UTF8.GetBytes(source);
            StringBuilder sb = new StringBuilder(resultArray.Length + resultArray.Length / 2);
            foreach (byte b in resultArray)
            {
                sb.Append(ConvertTenToSixtyTwo(b).PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        public static string Decrypt62(string encryptStr)
        {
            int length = encryptStr.Length / 2;
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                Int64 ii = ConvertSixtytwoToTen(encryptStr.Substring(i * 2, 2));
                bytes[i] = (byte)ii;
            }
            string s = Encoding.UTF8.GetString(bytes);
            return s;
        }

        public static string Encrypt16(string source)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("utf-8").GetBytes(source);
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        public static string Decrypt16(string hexString)
        {
            string rs = "";
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            rs = System.Text.Encoding.GetEncoding("utf-8").GetString(returnBytes);
            return rs;
        }
    }
}
