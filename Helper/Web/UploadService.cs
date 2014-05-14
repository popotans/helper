using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Security.Cryptography;
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
            rs.Code = 0;
            // check
            string checkRs = CheckAviable(req);
            if (checkRs.Length > 0)
            {
                rs.Code = 1;
                rs.Msg = checkRs;
                return rs;
            }

            if (string.IsNullOrEmpty(req.FileName))
            {
                rs.Code = 2;
                rs.Msg = "文件名称为空！";
                return rs;
            }

            if (!CheckSignature())
            {
                rs.Code = 3;
                rs.Msg = "授权失败！";
                return rs;
            }

            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + req.SaveVirtualPath + "\\" + req.FileName, FileMode.CreateNew, FileAccess.ReadWrite);
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
            rs.ReturnFilePath = req.SaveVirtualPath + "\\" + req.FileName;
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

        private string GetAppKey(string appId)
        {
            if (appId == "1")
                return "key1";
            else return "none";
        }
        private bool CheckSignature()
        {
            string keyFromAppid = GetAppKey(header.AppID);
            string rightSign = CreateSignature(header.AppID + header.Algorithm + keyFromAppid, header.Algorithm, keyFromAppid); ;
            if (rightSign == header.Signature) return true;
            return false;
        }
        [WebMethod]
        public string CreateSignature(string data, string algorithm, string authKey)
        {
            if (string.IsNullOrEmpty(algorithm))
            {
                throw new ArgumentNullException("algorithm", "算法为空");
            }
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data", "数据为空");
            }
            if (authKey == null)
            {
                authKey = string.Empty;
            }
            byte[] hash = null;
            data = string.Format("{0}{1}", data, authKey);
            data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
            switch (algorithm.ToLower())
            {

                case "md5":
                    hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
                    break;
                case "sha256":
                    hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
                    break;
                case "sha512":
                    hash = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
                    break;
                default:
                    throw new Exception("当前不支持以" + algorithm + "为算法的签名");
            }
            return Convert.ToBase64String(hash);
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
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
    }

    public class UploadSoapHeader : System.Web.Services.Protocols.SoapHeader
    {
        /// <summary>
        /// 
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// 签名格式：username+pwd+AuthKey
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 算法 md5，sha256, sha512
        /// </summary>
        public string Algorithm { get; set; }
    }
}
