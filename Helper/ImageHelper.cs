using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Helper
{
    public class ImageHelper
    {
        /// <summary>
        /// 浏览器输出图片
        /// </summary>
        /// <param name="pathImageFrom">互联网图片地址或本地图片地址</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void OutputImg(string pathImageFrom, int width, int height)
        {
            string pathImageTo = "";

            string originalImagePath = pathImageFrom;
            string thumbnailPath = pathImageTo;

            System.Drawing.Image originalImage = null;
            if (!pathImageFrom.ToLower().StartsWith("http"))
                originalImage = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "" + originalImagePath);
            else
            {
                originalImage = null;
                WebRequest wreq = WebRequest.Create(pathImageFrom);
                wreq.Timeout = 10000;//超时时间
                HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                using (Stream s = wresp.GetResponseStream())
                {
                    originalImage = System.Drawing.Image.FromStream(s);
                }
            }

            int w1 = originalImage.Width, h1 = originalImage.Height;
            if (width != 0 && height != 0)
                if (w1 > width) height = width * h1 / w1;
                else if (h1 > height) width = w1 * height / h1;
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            string mode = "Cut";
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）　　　　　　　　
                    break;
                case "W"://指定宽，高按比例　　　　　　　　　　
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）　　　　　　　　
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
              new Rectangle(x, y, ow, oh),
              GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                //bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                //HttpContext.Current.Response.ClearContent();
                //  HttpContext.Current.Response.Clear();
                //   HttpContext.Current.Response.ContentType = "image/jpeg";
                //bitmap.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                //HttpContext.Current.Response.End();

                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    HttpContext.Current.Response.ContentType = "image/jpeg";
                    HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        public static  System.Drawing.Image Image2Thumbs2(string pathImageFrom, int width, int height)
        {
            string pathImageTo = "";

            string originalImagePath = pathImageFrom;
            string thumbnailPath = pathImageTo;

            System.Drawing.Image originalImage = null;
            if (!pathImageFrom.ToLower().StartsWith("http"))
                originalImage = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "" + originalImagePath);
            else
            {
                originalImage = null;
                WebRequest wreq = WebRequest.Create(pathImageFrom);
                wreq.Timeout = 10000;//超时时间
                HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                using (Stream s = wresp.GetResponseStream())
                {
                    originalImage = System.Drawing.Image.FromStream(s);
                }
            }

            int w1 = originalImage.Width, h1 = originalImage.Height;
            if (width != 0 && height != 0)
                if (w1 > width) height = width * h1 / w1;
                else if (h1 > height) width = w1 * height / h1;
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            string mode = "Cut";
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）　　　　　　　　
                    break;
                case "W"://指定宽，高按比例　　　　　　　　　　
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）　　　　　　　　
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
              new Rectangle(x, y, ow, oh),
              GraphicsUnit.Pixel);
        //    return bitmap;
            try
            {
                System.Drawing.Image img = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    img = System.Drawing.Image.FromStream(ms);
                    return img;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="oldFilePath">原始图片路径</param>
        /// <param name="newFilePath">将要添加水印图片路径</param>
        /// <param name="waterPosition">水印位置</param>
        /// <param name="waterImagePath">水印图片路径</param>
        /// <param name="transparency">透明度</param>
        /// <param name="quality">质量</param>
        public static void CreateWaterImage(string oldFilePath, string newFilePath, int waterPosition, string waterImagePath, int watermarkTransparency, int quality)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(oldFilePath);

            Bitmap bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            g.DrawImage(image, 0, 0, image.Width, image.Height);

            //设置透明度
            System.Drawing.Image watermark = new Bitmap(waterImagePath);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
            {
                transparency = (watermarkTransparency / 10.0F);
            }

            float[][] colorMatrixElements = {
													new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
													new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
													new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
													new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f}, //注意：倒数第二处为0.0f为完全透明，1.0f为完全不透明
													new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
												};
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int _width = image.Width;
            int _height = image.Height;
            int xpos = 0;
            int ypos = 0;
            int WatermarkWidth = 0;
            int WatermarkHeight = 0;
            double bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((_width > watermark.Width * 2) && (_height > watermark.Height * 2))
            {
                bl = 1;
            }
            else if ((_width > watermark.Width * 2) && (_height < watermark.Height * 2))
            {
                bl = Convert.ToDouble(_height / 2) / Convert.ToDouble(watermark.Height);

            }
            else if ((_width < watermark.Width * 2) && (_height > watermark.Height * 2))
            {
                bl = Convert.ToDouble(_width / 2) / Convert.ToDouble(watermark.Width);
            }
            else
            {
                if ((_width * watermark.Height) > (_height * watermark.Width))
                {
                    bl = Convert.ToDouble(_height / 2) / Convert.ToDouble(watermark.Height);
                }
                else
                {
                    bl = Convert.ToDouble(_width / 2) / Convert.ToDouble(watermark.Width);
                }
            }
            WatermarkWidth = Convert.ToInt32(watermark.Width * bl);
            WatermarkHeight = Convert.ToInt32(watermark.Height * bl);
            switch (waterPosition)
            {
                case 3:
                    xpos = _width - WatermarkWidth - 10;
                    ypos = 10;
                    break;
                case 2:
                    xpos = 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
                case 5:
                    xpos = _width / 2 - WatermarkWidth / 2;
                    ypos = _height / 2 - WatermarkHeight / 2;
                    break;
                case 1:
                    xpos = 10;
                    ypos = 10;
                    break;
                case 4:
                default:
                    xpos = _width - WatermarkWidth - 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
            }
            g.DrawImage(watermark, new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);
            try
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType.IndexOf("jpeg") > -1)
                    {
                        ici = codec;
                    }
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];

                if (quality < 0 || quality > 100)
                {
                    quality = 80;
                }

                qualityParam[0] = quality;

                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;

                if (ici != null)
                {
                    bmp.Save(newFilePath, ici, encoderParams);
                }
                else
                {
                    bmp.Save(newFilePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                watermark.Dispose();
                imageAttributes.Dispose();
                image.Dispose();
                bmp.Dispose();
            }
        }

        /// <summary>
        /// 添加文字水印
        /// </summary>
        /// <param name="oldFilePath">原始图片路径</param>
        /// <param name="newFilePath">将要添加水印图片路径</param>
        /// <param name="waterPosition">水印位置</param>
        /// <param name="waterText">水印内容</param>
        public static void CreateWaterText(string oldFilePath, string newFilePath, int waterPosition, string waterText, int quality, string fontname, int fontsize)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(oldFilePath);
            Bitmap bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            g.DrawImage(image, 0, 0, image.Width, image.Height);

            int _width = bmp.Width;
            int _height = bmp.Height;

            Font crFont = new Font(fontname, fontsize, FontStyle.Bold, GraphicsUnit.Pixel);
            SizeF crSize = g.MeasureString(waterText, crFont);

            float xpos = 0;
            float ypos = 0;
            switch (waterPosition)
            {
                case 3:
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case 2:
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
                case 5:
                    xpos = ((_width - crSize.Width) / 2) + crSize.Width / 2;    //奇怪的表达式
                    ypos = (_height - crSize.Height) / 2 + crSize.Height / 2;
                    break;
                case 1:

                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;

                case 4:
                default:
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
            }

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            //可设置透明度
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            g.DrawString(waterText, crFont, semiTransBrush, xpos, ypos, StrFormat);

            try
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType.IndexOf("jpeg") > -1)
                    {
                        ici = codec;
                    }
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];

                if (quality < 0 || quality > 100)
                {
                    quality = 80;
                }

                qualityParam[0] = quality;


                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;

                if (ici != null)
                {
                    bmp.Save(newFilePath, ici, encoderParams);
                }
                else
                {
                    bmp.Save(newFilePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                semiTransBrush.Dispose();
                image.Dispose();
                bmp.Dispose();
            }
        }

        /// <summary>
        /// 获得图片的类型
        /// </summary>
        /// <param name="_Photo"></param>
        /// <returns></returns>
        public static System.Drawing.Imaging.ImageFormat ImgFormat(string _Photo)
        {
            //获得图片的后缀,不带点，小写
            string imgExt = _Photo.Substring(_Photo.LastIndexOf(".") + 1, _Photo.Length - _Photo.LastIndexOf(".") - 1).ToLower();
            System.Drawing.Imaging.ImageFormat _ImgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
            switch (imgExt)
            {
                case "png":
                    _ImgFormat = System.Drawing.Imaging.ImageFormat.Png;
                    break;
                case "gif":
                    _ImgFormat = System.Drawing.Imaging.ImageFormat.Gif;
                    break;
                case "bmp":
                    _ImgFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;
                default:
                    _ImgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
            }
            return _ImgFormat;
        }
        #region 生成缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式
        /// <code>HW:指定高宽缩放（可能变形）</code>
        /// <code>W:指定宽，高按比例  </code>
        /// <code>H:指定高，宽按比例</code>
        /// <code>CUT:指定高宽裁减（不变形） </code>
        /// <code>FILL:填充</code>
        /// </param>    
        public static bool LocalImage2Thumbs(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            Image2Thumbs(originalImage, thumbnailPath, width, height, mode);
            originalImage.Dispose();
            return true;
        }
        public static bool LocalImage2Thumbs(string originalImagePath, string thumbnailPath, int width, int height)
        {
            return LocalImage2Thumbs(originalImagePath, thumbnailPath, width, height, "CUT");
        }
        /// <summary>
        /// 生成远程图片的缩略图
        /// </summary>
        /// <param name="remoteImageUrl"></param>
        /// <param name="thumbnailPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool RemoteImage2Thumbs(string remoteImageUrl, string thumbnailPath, int width, int height, string mode)
        {
            if (string.IsNullOrEmpty(remoteImageUrl)) return false;
            try
            {
                WebRequest request = WebRequest.Create(remoteImageUrl);
                request.Timeout = 20000;
                Stream stream = request.GetResponse().GetResponseStream();
                System.Drawing.Image originalImage = System.Drawing.Image.FromStream(stream);
                Image2Thumbs(originalImage, thumbnailPath, width, height, mode);
                originalImage.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool RemoteImage2Thumbs(string remoteImageUrl, string thumbnailPath, int width, int height)
        {
            return RemoteImage2Thumbs(remoteImageUrl, thumbnailPath, width, height, "CUT");
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImage">源图</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="photoWidth">最终缩略图宽度</param>
        /// <param name="height">最终缩略图高度</param>
        /// <param name="mode">生成缩略图的方式
        /// <code>HW:指定高宽缩放（可能变形）</code>
        /// <code>W:指定宽，高按比例  </code>
        /// <code>H:指定高，宽按比例</code>
        /// <code>CUT:指定高宽裁减（不变形） </code>
        /// <code>FILL:填充</code>
        /// </param> 
        public static void Image2Thumbs(System.Drawing.Image originalImage, string thumbnailPath, int photoWidth, int photoHeight, string mode)
        {
            #region 开始画图
            int lastPhotoWidth = photoWidth;//最后缩略图的宽度
            int lastPhotoHeight = photoHeight;//最后缩略图的高度

            int toWidth = photoWidth;//原图片被压缩的宽度
            int toHeight = photoHeight;//原图片被压缩的高度

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            int bg_x = 0;
            int bg_y = 0;
            switch (mode.ToUpper())
            {
                case "FILL"://压缩填充至指定区域                
                    toHeight = photoHeight;
                    toWidth = toHeight * ow / oh;
                    if (toWidth > photoWidth)
                    {
                        toHeight = toHeight * photoWidth / toWidth;
                        toWidth = photoWidth;
                    }
                    bg_x = (photoWidth - toWidth) / 2;
                    bg_y = (photoHeight - toHeight) / 2;
                    break;
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toHeight = lastPhotoHeight = originalImage.Height * photoWidth / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    toWidth = lastPhotoWidth = originalImage.Width * photoHeight / originalImage.Height;
                    break;
                case "CUT"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)lastPhotoWidth / (double)lastPhotoHeight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * lastPhotoWidth / lastPhotoHeight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * photoHeight / lastPhotoWidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(lastPhotoWidth, lastPhotoHeight);//新建一个bmp图片
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);//新建一个画板
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.Clear(System.Drawing.Color.White);//白色
            g.DrawImage(originalImage, new System.Drawing.Rectangle(bg_x, bg_y, toWidth, toHeight),//在指定位置并且按指定大小绘制原图片的指定部分
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);
            try
            {

                bitmap.Save(thumbnailPath, ImgFormat(thumbnailPath));
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {

                bitmap.Dispose();
                g.Dispose();
            }
            #endregion
        }
        #endregion
        /// <summary>
        /// 切割后生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="toW">缩略图最终宽度</param>
        /// <param name="toH">缩略图最终高度</param>
        /// <param name="X">X坐标（zoom为1时）</param>
        /// <param name="Y">Y坐标（zoom为1时）</param>
        /// <param name="W">选择区域宽（zoom为1时）</param>
        /// <param name="H">选择区域高（zoom为1时）</param>

        public static void MakeCutThumbs(string originalImagePath, string thumbnailPath, int toW, int toH, int X, int Y, int W, int H)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            int towidth = toW;
            int toheight = toH;
            int x = X;
            int y = Y;
            int ow = W;
            int oh = H;            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
            new System.Drawing.Rectangle(x, y, ow, oh),
            System.Drawing.GraphicsUnit.Pixel);
            try
            {
                bitmap.Save(thumbnailPath, ImgFormat(thumbnailPath));

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        #region 在图片上增加文字水印
        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        /// <param name="addText">水印文字</param>
        public static void AddWater(string Path, string Path_sy, string addText)
        {
            int[] sizes = new int[] { 32, 30, 28, 26, 24, 22, 20, 18, 16, 14, 12, 10, 8, 6, 4 };
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            int w = image.Width, h = image.Height;
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 14);
            System.Drawing.Color color = System.Drawing.Color.FromArgb(155, 155, 122);
            //  color = System.Drawing.Color.Silver;
            System.Drawing.Brush b = new System.Drawing.SolidBrush(color);

            float x = 10;
            g.DrawString(addText, f, b, x, h - 32);
            g.Dispose();
            image.Save(Path_sy);
            image.Dispose();
        }
        #endregion

        #region 在图片上生成图片水印
        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="watermarkFilename">水印文件名</param>
        /// <param name="watermarkStatus">图片水印位置:0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下</param>
        /// <param name="quality">是否是高质量图片 取值范围0--100</param> 
        /// <param name="watermarkTransparency">图片水印透明度 取值范围1--10 (10为不透明)</param>

        public static void AddImageSignPic(string Path, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);

            //设置高质量插值法
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            System.Drawing.Image watermark = new System.Drawing.Bitmap(watermarkFilename);

            if (watermark.Height >= img.Height || watermark.Width >= img.Width)
            {
                return;
            }

            System.Drawing.Imaging.ImageAttributes imageAttributes = new System.Drawing.Imaging.ImageAttributes();
            System.Drawing.Imaging.ColorMap colorMap = new System.Drawing.Imaging.ColorMap();

            colorMap.OldColor = System.Drawing.Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            System.Drawing.Imaging.ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, System.Drawing.Imaging.ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
            {
                transparency = (watermarkTransparency / 10.0F);
            }

            float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new System.Drawing.Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, System.Drawing.GraphicsUnit.Pixel, imageAttributes);

            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            System.Drawing.Imaging.ImageCodecInfo ici = null;
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                //if (codec.MimeType.IndexOf("jpeg") > -1)
                if (codec.MimeType.Contains("jpeg"))
                {
                    ici = codec;
                }
            }
            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
            {
                quality = 80;
            }
            qualityParam[0] = quality;

            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
            {
                img.Save(filename, ici, encoderParams);
            }
            else
            {
                img.Save(filename);
            }

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public static void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }
        #endregion
    }
}
