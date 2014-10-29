using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;

namespace VMoom.TemplateEngine
{
    /// <summary>
    /// 基础模板构建器
    /// </summary>
    public class DefaultEngine : ITemplateEngine
    {

        public DefaultEngine(string templateContent)
        {
            _templateContent = templateContent;
            //初始化模板引擎
            InitTemplateManager();
        }

        public DefaultEngine(string templateFile, Encoding encoding)
        {
            _templateContent = System.IO.File.ReadAllText(templateFile, encoding);
            //初始化模板引擎
            InitTemplateManager();
        }

        #region 保护方法
        /// <summary>
        /// 对构建器进行初始化，如果要增加设置，请重载本方法
        /// </summary>
        protected virtual void InitTemplateManager()
        {
            manager = TemplateManager.FromString(_templateContent);

            manager.RegisterCustomTag("cut", new ContentCutTagHandler());   //注册字符裁剪标签
            manager.RegisterCustomTag("include", new IncludeTagHandler());   //注册文件引用标签
        }
        #endregion

        #region 属性
        private string _templateContent = "";
        /// <summary>
        /// 模板内容
        /// </summary>
        public string TemplateContent
        {
            get { return _templateContent; }
            set { _templateContent = value; }
        }

        private TemplateManager manager = null;
        /// <summary>
        /// 模板引擎管理器
        /// </summary>
        public TemplateManager TemplateManager
        {
            get { return manager; }
        }

        #endregion

        #region 公用方法

        /// <summary>
        /// 解析指定的模板文件中的内容
        /// </summary>
        /// <param name="templatecontent">模板内容</param>
        /// <returns></returns>
        public virtual string Parser(string templateContent)
        {
            if (String.IsNullOrEmpty(templateContent))
                throw new ArgumentException("templateContent 参数不能为空.", "templateContent");

            _templateContent = templateContent;

            return Parser();
        }

        /// <summary>
        /// 解析模板
        /// </summary>
        /// <returns></returns>
        public virtual string Parser()
        {
            if (String.IsNullOrEmpty(_templateContent))
                throw new InvalidOperationException("TemplateContent 属性不能为空.");

            return manager.Process();
        }

        public virtual void Set(string key, object val)
        {
            if (val.GetType() == typeof(Template)) manager.AddTemplate((Template)val);
            else
                manager.SetValue(key, val);
        }

        #endregion
    }
}