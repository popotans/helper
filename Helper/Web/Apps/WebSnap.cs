using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing;

namespace Helper.Web.Apps
{
    /// <summary>
    /// 生成网页截图
    /// </summary>
    public class WebSnap : System.Web.UI.Page
    {
        protected string Url { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentNullException("Url is NUll !");
            }

            CreateWebImg cwi = new CreateWebImg(Url, 0);
            MyResetEvent set = new MyResetEvent();
            cwi.Start(set.setEvent);
            set.setEvent.WaitOne();

            MemoryStream stream = cwi.ImageStream;
            if (stream != null)
            {
                using (stream)
                {
                    byte[] buff = stream.ToArray();
                    Response.ContentType = "image/Jpeg";
                    Response.BinaryWrite(buff);
                }
            }
            Response.End();
        }

        protected virtual void SetUrl(string url)
        {
            this.Url = url;
        }

    }


    public class MyResetEvent
    {
        public AutoResetEvent setEvent = new AutoResetEvent(false);

    }

    public class CreateWebImg
    {
        private AutoResetEvent Reset;

        public CreateWebImg(string url, int WidthPadding)
        {
            this.Url = url;
            this.RightWidthPadding = WidthPadding;
        }

        #region property

        private int _widthPadding;
        /// <summary>
        /// 右侧宽度偏移量
        /// </summary>
        public int RightWidthPadding
        {
            get { return _widthPadding; }
            set { _widthPadding = value; }
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        #endregion

        public void Start(AutoResetEvent res)
        {
            Reset = res;
            ThreadStart ts = new ThreadStart(delegate()
            {
                WebBrowser webBrowser = new WebBrowser();  // 创建一个WebBrowser

                webBrowser.ScrollBarsEnabled = false;  // 隐藏滚动条
                webBrowser.ScriptErrorsSuppressed = false;
                webBrowser.Navigate(Url);  // 打开网页
                webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);  // 增加网页加载完成事件处理函数
                webBrowser.Navigated += new WebBrowserNavigatedEventHandler(webBrowser_Navigated);

                while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
                {
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(10);
                }
            });
            Thread thread = new Thread(ts);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            WebBrowser webBrowser = (WebBrowser)sender;
            webBrowser.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
        }

        public MemoryStream ImageStream = null;

        /// <summary>
        /// 网页加载完成事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser webBrowser = (WebBrowser)sender;
            webBrowser.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
            // 网页加载完毕才保存
            if (webBrowser.ReadyState == WebBrowserReadyState.Complete)
            {
                // 获取网页高度和宽度,也可以自己设置
                int height = webBrowser.Document.Body.ScrollRectangle.Height;
                int width = webBrowser.Document.Body.ScrollRectangle.Width + this.RightWidthPadding;

                // 调节webBrowser的高度和宽度
                webBrowser.Height = height;
                webBrowser.Width = width;

                using (Bitmap bitmap = new Bitmap(width, height))// 创建高度和宽度与网页相同的图片
                {
                    Rectangle rectangle = new Rectangle(0, 0, width, height); // 绘图区域
                    webBrowser.DrawToBitmap(bitmap, rectangle);  // 截图
                    if (ImageStream == null) ImageStream = new MemoryStream();
                    bitmap.Save(ImageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                Reset.Set();
            }
        }

        void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            //script werror
            e.Handled = true;
        }
    }
}
