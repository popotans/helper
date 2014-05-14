using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.localhost;
namespace WebApplication1
{
    public partial class UploadServiceTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Click += new EventHandler(Button1_Click);
        }

        void Button1_Click(object sender, EventArgs e)
        {
            Upload up = new Upload();

            up.header = new Helper.Web.UploadSoapHeader();
            up.header.AppID = "1";
            up.header.Algorithm = "md5";
            up.header.Signature = up.CreateSignature(up.header.AppID + up.header.Algorithm + "key1", "md5", "key1");

            Helper.Web.UploadResult rs = up.Upload(new Helper.Web.UploadRequest()
            {
                SaveVirtualPath = "Styles",
                FileBytes = FileUpload1.FileBytes,
                FileName = FileUpload1.FileName

            });

            Response.Write(rs.Code + "," + rs.Msg + "," + rs.ReturnFilePath);
        }
    }
}