using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// getImage 的摘要说明
    /// </summary>
    public class getImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string url = "http://localhost:14977/attach/month_1308/130804113241118500.jpg";
            url = "http://static.googleadsserving.cn/pagead/imgad?id=CICAgIDQp-7xjAEQrAIY-gEyCMBPcjdgV-QB";
            Helper.ImageHelper.OutputImg(url, 111, 222);
            context.Response.End();

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}