using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.IO;
namespace Helper.Web
{
    public class UploadService : System.Web.Services.WebService
    {
        public UploadSoapHeader header = new UploadSoapHeader();
        [WebMethod]
        public string Index()
        {
            return "Congratulations!IT Works.";
        }

        [WebMethod(EnableSession = true, BufferResponse = false)]
        [System.Web.Services.Protocols.SoapHeader("header")]
        public UploadResult Upload(UploadRequest req)
        {
            UploadResult rs = new UploadResult();
            rs.Msg = "success";
            rs.ReturnFilePath = req.SaveVirtualPath;
            // check
            string checkRs = CheckAviable(req);
            if (checkRs.Length > 0)
            {
                rs.Code = 2;
                rs.Msg = checkRs;
                return rs;
            }

            if (!CheckAuthKey() && !CheckUser()
                )
            {
                rs.Code = 2;
                rs.Msg = "没有被授权";
                return rs;
            }


            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(req.SaveVirtualPath), FileMode.Create, FileAccess.Write);
            try
            {
                fs.Write(req.FileBytes, 0, req.SaveVirtualPath.Length);
            }
            catch (Exception e)
            {
                rs.Code = 1;
                rs.Msg = e.Message;
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
            return rs;
        }

        private string CheckAviable(UploadRequest req)
        {
            string fiel = req.SaveVirtualPath;
            if (ContainsIgnoreCase(fiel, ".asp")
                || ContainsIgnoreCase(fiel, ".aspx")
                || ContainsIgnoreCase(fiel, ".cs")
                || ContainsIgnoreCase(fiel, ".js")
                || ContainsIgnoreCase(fiel, ".php")
                || ContainsIgnoreCase(fiel, ".jsp")
                || ContainsIgnoreCase(fiel, ".java")
                || ContainsIgnoreCase(fiel, ".cgi")
                || ContainsIgnoreCase(fiel, ".ashx")
                || ContainsIgnoreCase(fiel, ".asmx")
                )
            {
                return "文件格式不被允许或不符合标准格式";
            }
            return string.Empty;
        }

        private bool ContainsIgnoreCase(string source, string tocompare)
        {
            return source.IndexOf(tocompare, StringComparison.CurrentCultureIgnoreCase) > -1;
        }

        private bool CheckAuthKey()
        {
            return !string.IsNullOrEmpty(header.AuthKey);
        }
        private bool CheckUser()
        {
            if (string.IsNullOrEmpty(header.UserName) || string.IsNullOrEmpty(header.Pwd)) return false;
            bool check = false;
            if (header.UserName != header.Pwd) check = true;
            return check;
        }
    }

    public class UploadResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public string ReturnFilePath { get; set; }
        public string ErrorDetail { get; set; }
    }

    public class UploadRequest
    {
        public string SaveVirtualPath { get; set; }
        public byte[] FileBytes { get; set; }
    }

    public class UploadSoapHeader : System.Web.Services.Protocols.SoapHeader
    {
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string AuthKey { get; set; }
    }
}
