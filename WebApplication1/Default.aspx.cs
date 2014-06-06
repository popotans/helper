using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Click += new EventHandler(Button1_Click);
            //if (!IsPostBack)
            //{
            //    HttpContext.Current.Items["a"] = "a";
            //    object obj = HttpContext.Current.Items["a"];
            //}

            //Response.Write(HttpRuntime.AppDomainAppPath);
            //Response.Write("<br>" + AppDomain.CurrentDomain.BaseDirectory);

            //string[] urls = ShortUrl("http://www.young-sun.com");
            //Response.Write("<br>" + urls.Count());

            //foreach (string s in urls)
            //{
            //    Response.Write(s + "<br>");
            //}

            //Response.Write("<br/>" + Short62(1) + "??>>>>" + "803401262");
            //Response.Write("<br>long=" + UnShort62("SmZmS"));

            string s = "聂军华";

            //string str1 = Helper.Security.SecurityHelper.Encrypt16(s);
            //Response.Write(str1+"<br/>");
            //Response.Write(Helper.Security.SecurityHelper.Decrypt16(str1));
            s = Helper.Str.StringHelper.UnicodeEncode(s);

            s = Helper.Str.StringHelper.UnicodeDecode(@"\u5e7f\u5dde\u5927\u5b66\u7eba\u7ec7\u670d\u88c5\u5b66\u9662\u6559\u80b2\u7f51");
            Response.Write(s);

        }

        void Button1_Click(object sender, EventArgs e)
        {
            object obj1 = HttpContext.Current.Items["a"];
            if (FileUpload1.HasFile)
            {
                byte[] arr = FileUpload1.FileBytes;
                Response.Write(FileUpload1.FileName.Trim());
                localhost.Upload upload = new localhost.Upload();
                localhost.UploadRequest request = new localhost.UploadRequest()
                {
                    FileBytes = arr,
                    SaveVirtualPath = "/Account/" + Guid.NewGuid() + FileUpload1.FileName
                };
                //


                localhost.UploadSoapHeader header = new localhost.UploadSoapHeader
                {
                    //AuthKey = "niejunhua"
                };


                upload.UploadSoapHeaderValue = header;
                localhost.UploadResult rs = upload.CallUpload(request);

                Response.Write("code=" + rs.Code + "<br>");
                Response.Write("Msg=" + rs.Msg + "<br>");
            }
        }

        public string[] ShortUrl(string url)
        {
            string[] chars = new string[] {"a" , "b" , "c" , "d" , "e" , "f" , "g" , "h" ,
              "i" , "j" , "k" , "l" , "m" , "n" , "o" , "p" , "q" , "r" , "s" , "t" ,
              "u" , "v" , "w" , "x" , "y" , "z" , "0" , "1" , "2" , "3" , "4" , "5" ,
              "6" , "7" , "8" , "9" , "A" , "B" , "C" , "D" , "E" , "F" , "G" , "H" ,
              "I" , "J" , "K" , "L" , "M" , "N" , "O" , "P" , "Q" , "R" , "S" , "T" ,
              "U" , "V" , "W" , "X" , "Y" , "Z"};
            string value = "";// UserMd5(url);
            value = Helper.Security.SecurityHelper.MD5(url);

            Response.Write("<br>value=" + value);
            Response.Write("<br>hashcode:" + value.GetHashCode());
            value = value.GetHashCode().ToString();

            string[] results = new string[value.Length / 8];
            for (int i = 0; i < value.Length / 8; i++)
            {
                string tempResult = value.Substring(i * 8, 8);
                long lHexLong = 0x3FFFFFFF & long.Parse(tempResult);
                string outchar = "";
                for (int j = 0; j < 6; j++)
                {
                    long index = 0x0000003D & lHexLong;
                    outchar += chars[(int)index];
                    lHexLong = lHexLong >> 5;
                }
                results[i] = outchar;
            }
            return results;
        }

        public string Short62(long n)
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

        public long UnShort62(string s)
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
    }
}
