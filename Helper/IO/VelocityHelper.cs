using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons.Collections;
using NVelocity;
using NVelocity.Runtime;
using NVelocity.App;
using NVelocity.Context;
using System.Collections;

namespace Helper
{
    public class VelocityHelper
    {
        private VelocityEngine velocity = null;
        private IContext context = null;
        private VelocityHelper()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualDir">本程序目录的相对路径</param>
        public VelocityHelper(string virtualDir)
        {
            Init(virtualDir);
        }

        public void Init(string virtualDir)
        {
            //创建VelocityEngine实例对象
            velocity = new VelocityEngine();
            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();
            props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, string.Format("{0}\\{1}", Root.TrimEnd('\\'), virtualDir));
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            //  props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
            velocity.Init(props);

            //为模板变量赋值
            context = new VelocityContext();
        }

        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="key">模板变量</param>
        /// <param name="value">模板变量值</param>
        public void Put(string key, object value)
        {
            if (context == null)
            {
                throw new ApplicationException("使用默认构造函数。必须调用Init(string virtualDir)");

            }
            context.Put(key, value);
        }

        /// <summary>
        /// 模版文件名
        /// </summary>
        /// <param name="templatefile"></param>
        /// <returns></returns>
        public string Render(string templatefile)
        {
            Template tmp = velocity.GetTemplate(templatefile);
            System.IO.StringWriter writer = new System.IO.StringWriter();
            tmp.Merge(context, writer);
            return writer.ToString();
        }

        //public string Render(string templatePath, Hashtable data)
        //{
        //    if (data != null)
        //        foreach (string item in data.Keys)
        //            context.Put(item, data[item]);


        //    Template tmp = velocity.GetTemplate(templatePath);
        //    System.IO.StringWriter writer = new System.IO.StringWriter();
        //    tmp.Merge(context, writer);
        //    return writer.ToString();
        //}

        public string Root
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
