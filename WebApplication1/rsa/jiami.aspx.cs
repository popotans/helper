using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper.Security;
namespace WebApplication1.rsa
{
    public partial class jiami : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RsaEncrypt re = new RsaEncrypt();

            string privateKey = @"<RSAKeyValue><Modulus>4KMaZkINCQhDgmoAUWt02Wy2z8pug3vB5f6hLKi2Xl2KHpawbCRoSHavOq1vYxOdOM1PLi+pJXqsQxqOvGZKrAdoAwserZG/EXaKsgDlag3O7ppBMo5vIbPLbFk3U/S69YiFjAf9nfBst7hZv6ny8csnEQ5FsCV3E45bje/n31M=</Modulus><Exponent>AQAB</Exponent><P>/r+oENJL0kBtF4tkh1cc9nvl+soPW9a7xorwB7m83LUkVmYDlss63rSKqVaZZcMOmCf0Z5slbzl1kdDTqifHzw==</P><Q>4b2U6r1gxFGTCts812xVLH4Mycl2y0A+IaV2QYnKIrsm8EHtH0RpHywY86GBTi53PBH4Q/V1aOoXbez8PmxNPQ==</Q><DP>2irNBkrzgEaIpwdLutSSC02kcZkmeiJ2gdxvIE6jPeksC4omPLgj3KKFtexiRtI753cgSrKF5rEwzQYon6qd5Q==</DP><DQ>tYRWLHNfTj0x+bbUs3x++Kna6p8/EKx1yWRdNNHxYgAPjX3ViCDgTpqM7creSNmAMyNX+H11jYk+kbvQEPnRGQ==</DQ><InverseQ>B6uXEtIT8CKWWcdrJxUqMNY7VakmYBBeH0LvDj8ExVpxDn5oI6t3/TXswNyeFT7w1pIqrQgIWAkIyrbpZJ8lWA==</InverseQ><D>TMaSHEcQ63heknEmQK2oVzN329cFJH0kzTXU4k2bXu87aK76B0v+NF4UD8r9GfC3OJW7LT7u4bdmrtyh0jRn6nW4og7pQl2WjqiiVukDERe6yYPDgvwjPh6xkkXbfr6LUVuHoTFEU7wZMjoVimrmG39el6QA3hH6rS+LLEqW0bk=</D></RSAKeyValue>";

            string publisKey = @"<RSAKeyValue><Modulus>4KMaZkINCQhDgmoAUWt02Wy2z8pug3vB5f6hLKi2Xl2KHpawbCRoSHavOq1vYxOdOM1PLi+pJXqsQxqOvGZKrAdoAwserZG/EXaKsgDlag3O7ppBMo5vIbPLbFk3U/S69YiFjAf9nfBst7hZv6ny8csnEQ5FsCV3E45bje/n31M=</Modulus><Exponent>AQAB</Exponent><P>/r+oENJL0kBtF4tkh1cc9nvl+soPW9a7xorwB7m83LUkVmYDlss63rSKqVaZZcMOmCf0Z5slbzl1kdDTqifHzw==</P><Q>4b2U6r1gxFGTCts812xVLH4Mycl2y0A+IaV2QYnKIrsm8EHtH0RpHywY86GBTi53PBH4Q/V1aOoXbez8PmxNPQ==</Q><DP>2irNBkrzgEaIpwdLutSSC02kcZkmeiJ2gdxvIE6jPeksC4omPLgj3KKFtexiRtI753cgSrKF5rEwzQYon6qd5Q==</DP><DQ>tYRWLHNfTj0x+bbUs3x++Kna6p8/EKx1yWRdNNHxYgAPjX3ViCDgTpqM7creSNmAMyNX+H11jYk+kbvQEPnRGQ==</DQ><InverseQ>B6uXEtIT8CKWWcdrJxUqMNY7VakmYBBeH0LvDj8ExVpxDn5oI6t3/TXswNyeFT7w1pIqrQgIWAkIyrbpZJ8lWA==</InverseQ><D>TMaSHEcQ63heknEmQK2oVzN329cFJH0kzTXU4k2bXu87aK76B0v+NF4UD8r9GfC3OJW7LT7u4bdmrtyh0jRn6nW4og7pQl2WjqiiVukDERe6yYPDgvwjPh6xkkXbfr6LUVuHoTFEU7wZMjoVimrmG39el6QA3hH6rS+LLEqW0bk=</D></RSAKeyValue>";
            string hardId = Helper.Win32Helper.GetHardID();
            string text = "niejunhua";
            string jiami2 = re.Encrypt(publisKey, text);

            Response.Write("明文：" + text + "<br/><br/>");
            Response.Write("加密结果：" + jiami2 + "<br/><br/>");


            //string s = re.Decrypt(publisKey, jiami2);
            //Response.Write("jie密结果：" + s + "<br/><br/><br/>");

        }
    }
}