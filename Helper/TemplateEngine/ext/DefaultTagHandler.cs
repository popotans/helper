using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using VMoom.TemplateEngine.Parser.AST;
using VMoom.TemplateEngine;

namespace VMoom.TemplateEngine
{
    /// <summary>
    /// 内容自定义裁剪模板标签
    /// <#cut len="10">要裁剪的内容,将自动过滤HTML代码<#/cut>
    /// </summary>
    public class ContentCutTagHandler : ITagHandler
    {
        public void TagBeginProcess(TemplateManager manager, Tag tag, ref bool processInnerElements, ref bool captureInnerContent)
        {
            processInnerElements = true;
            captureInnerContent = true;
        }

        public void TagEndProcess(TemplateManager manager, Tag tag, string innerContent)
        {
            Expression exp;
            string _len, _cutstring;
            exp = tag.AttributeValue("len");
            if (exp == null)
                throw new TemplateRuntimeException("cut标签必须有 len 属性.", tag.Line, tag.Col);
            _len = manager.EvalExpression(exp).ToString();
            //对内容进行裁剪并添加省略号
            _cutstring = GetSummary(innerContent, int.Parse(_len));
            manager.WriteValue(_cutstring);
        }

        /// <summary>
        /// 获取文本内容指定长度的摘要
        /// </summary>
        /// <param name="html">要处理的包含HTML代码的文本内容</param>
        /// <param name="summaryLength">返回的长度</param>
        /// <returns></returns>
        public static string GetSummary(string html, int summaryLength)
        {
            string text = GetAllInnerText(html);
            if (summaryLength >= text.Length)
                return text;
            return text.Substring(0, summaryLength);
        }

        /// <summary>
        /// 去除HTML标记返回纯文本
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetAllInnerText(string html)
        {
            return System.Text.RegularExpressions.Regex.Replace(html, @"<[^>]*>", "");
        }
    }

    /// <summary>
    /// 文件引用模板标签,要嵌入的文件必须首先调用引擎解释完毕
    /// <#include file="master/index.htm" source="file/web" charset="utf-8" />
    /// </summary>
    public class IncludeTagHandler : ITagHandler
    {
        public void TagBeginProcess(TemplateManager manager, Tag tag, ref bool processInnerElements, ref bool captureInnerContent)
        {
            processInnerElements = true;
            captureInnerContent = false;
        }

        public void TagEndProcess(TemplateManager manager, Tag tag, string innerContent)
        {
            Expression exp;
            string strFile, strResult;
            string strSource = "", strCharset = "";
            //获取文件路径参数
            exp = tag.AttributeValue("file");
            if (exp == null)
                throw new TemplateRuntimeException("include 标签必须有 file 属性.", tag.Line, tag.Col);
            strFile = manager.EvalExpression(exp).ToString();
            //获取文件来源参数，file-文件系统，web-远程网页
            exp = tag.AttributeValue("source");
            if (exp == null)
                strSource = "file";
            strSource = manager.EvalExpression(exp).ToString();
            //获取字符集
            exp = tag.AttributeValue("charset");
            if (exp == null)
                strSource = "utf-8";
            strCharset = manager.EvalExpression(exp).ToString();

            //读取文件内容并插入到模板中
            if (strSource == "file")
            {
                //读取本地文件内容
                if (System.IO.File.Exists(strFile))
                    strResult = System.IO.File.ReadAllText(strFile);
                else
                    strResult = string.Format("[{0}文件不存在.]", strFile);
            }
            else
            {
                //抓取远程网页内容
                string strSiteUrl = string.Format("{0}://{1}", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Authority);
                strFile = strFile.Replace("~", strSiteUrl);
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                MyWebClient.Encoding = Encoding.GetEncoding(strCharset);
                strResult = MyWebClient.DownloadString(strFile);
            }
            manager.WriteValue(strResult);
        }
    }


}
