using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text;
using System.IO;
using System.Web.SessionState;
//using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Script.Serialization;

namespace Helper.NetAjax
{
    public class AjaxCoreHandler : IHttpHandler, IRequiresSessionState
    {
        private HttpContext context;

        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Request请求入口
        /// </summary>
        public void ProcessRequest(HttpContext context)
        {
            this.context = context;

            //获取函数签名并将其触发
            string inputMethod = GetInputMethod();
            object result = Invoke(inputMethod, GetParameterValue());

            //生成JS模板
            StringBuilder sbStr = GenerateJsTemplate();

            //将结果打印出去
            context.Response.Write(result);
        }

        /// <summary>
        /// 得到JS模板,并将其中的关键字做替换,能够解决目录或者是名称变更,找不到handler文件的问题.
        /// </summary>
        public StringBuilder GenerateJsTemplate()
        {
            Type type = this.GetType();

            Uri url = HttpContext.Current.Request.Url;
            string script = GetJsTemplate();
            script = script.Replace("%H_DESC%", "通过jQuery.ajax完成服务端函数调用");
            script = script.Replace("%H_DATE%", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            script = script.Replace("%URL%", url.Query.Length > 0 ? url.ToString().Replace(url.Query, "") : url.ToString());
            script = script.Replace("%CLS%", type.Name);

            StringBuilder scriptBuilder = new StringBuilder(script);

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (MethodInfo m in methods)
            {
                //ResponseAnnotationAttribute resAnn = this.GetAnnation(m.Name);

                //scriptBuilder.AppendLine("/*----------------------------------------------------------------------------");
                //scriptBuilder.AppendLine("功能说明:" + resAnn.Desc);
                //scriptBuilder.AppendLine("附加说明:缓存时间 " + resAnn.CacheDuration.ToString() + " 秒");
                //scriptBuilder.AppendLine("         输出类型 " + resAnn.ResponseFormat.ToString());
                //scriptBuilder.AppendLine("----------------------------------------------------------------------------*/");

                string func = GetFunctionTemplete(m);
                scriptBuilder.AppendLine(func);
            }
            return scriptBuilder;
        }

        /// <summary>
        /// 将后台业务代码动态添加到JS文件中,供前台调用
        /// </summary>
        private static string GetFunctionTemplete(MethodInfo method)
        {
            StringBuilder func = new StringBuilder(method.DeclaringType.Name);
            func.Append(".prototype." + method.Name);
            func.Append("=function");

            func.Append("(");
            foreach (ParameterInfo p in method.GetParameters())
            {
                func.Append(p.Name + ",");
            }
            func.Append("callback)");

            func.AppendLine("{");
            {
                func.Append("\tvar args = {");
                foreach (ParameterInfo p in method.GetParameters())
                {
                    func.Append(p.Name + ":" + p.Name + ",");
                }
                func.AppendLine("ajax:'jquery1.4.2'};");
                //switch (format)
                //{
                //    case ResponseFormat.Xml:
                //        func.AppendLine("\tvar options={dataType:'xml'};");
                //        break;
                //    case ResponseFormat.Json:
                //        func.AppendLine("\tvar options={dataType:'json'};");
                //        break;
                //    default:
                //        func.AppendLine("\tvar options={dataType:'text'};");
                //        break;
                //}
                func.AppendLine("\tvar options={dataType:'text'};");
                func.AppendLine("\t$.extend(true,options,{},this.Options);");
                func.AppendFormat("\t$.net.CallWebMethod(options,'{0}', args, callback);", method.Name);
                func.AppendLine();
            }
            func.AppendLine("}\t\t");

            return func.ToString();
        }

        /// <summary>
        /// 文件流操作,读取JS模板文件
        /// </summary>
        private string GetJsTemplate()
        {
            //Type type = typeof(AjaxCoreHandler);

            //AssemblyName asmName = new AssemblyName(type.Assembly.FullName);

            //Stream stream = type.Assembly.GetManifestResourceStream(asmName.Name + ".Scripts.Ajaxnet.js");
            string rs = "";
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Scripts\\Ajaxnet.js"))
            {
                using (StreamReader sw = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Scripts\\Ajaxnet.js"))
                {
                    rs = sw.ReadToEnd();
                }
            }
            else
            {
                throw new Exception("模版不存在");
            }
            if (!string.IsNullOrEmpty(rs)) return rs;
            else
            {
                throw new Exception("模版未找到");
            }

            //if (stream != null)
            //{
            //    byte[] buffer = new byte[stream.Length];
            //    int len = stream.Read(buffer, 0, (int)stream.Length);

            //    string temp = Encoding.UTF8.GetString(buffer, 0, len);

            //    return temp;
            //}
            //else
            //{
            //    throw new Exception("模版未找到");
            //}
        }

        /// <summary>
        /// 获取当前请求的信息,如果有请求函数,则转至请求函数,如果没有,则代表需要生成动态JS文件
        /// </summary>
        private string GetInputMethod()
        {
            string[] segmentCollection = this.context.Request.Url.Segments;
            int segmentLength = segmentCollection.Length;
            string inputMethod = segmentCollection[segmentLength - 1];

            if (inputMethod.LastIndexOf(".ashx") >= 0)
                inputMethod = "GenerateJsTemplate";

            return inputMethod;
        }

        /// <summary>
        /// 动态调用有参/无参methodName方法并返回结果
        /// </summary>
        /// <param name="methodName">函数签名</param>
        /// <param name="args">参数内容</param>
        /// <returns>返回内容</returns>
        private object Invoke(string methodName, Dictionary<string, object> args)
        {
            MethodInfo specificMethod = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            if (specificMethod == null)
                throw new Exception("The method is not exist, pls check.");

            List<object> argsList = new List<object>();

            ParameterInfo[] parameterInfo = specificMethod.GetParameters();  // Get the parameters

            foreach (ParameterInfo p in parameterInfo)  //Loop
            {
                if (args.ContainsKey(p.Name))
                {
                    object obj = args[p.Name]; // get parameter value
                    argsList.Add(Convert.ChangeType(obj, p.ParameterType));
                }
            }

            object[] parameters = argsList.ToArray();
            object result = specificMethod.Invoke(this, parameters);
            return result;
        }

        /// <summary>
        /// 动态获取参数并保存
        /// </summary>
        private Dictionary<string, object> GetParameterValue()
        {
            Stream inputStream = this.context.Request.InputStream;
            inputStream = HttpContext.Current.Request.InputStream;

            inputStream.Position = 0;  //reset the position to 0

            byte[] buffer = new byte[inputStream.Length];
            inputStream.Read(buffer, 0, buffer.Length); //read stream data into buffer
            //inputStream.Close();
            //inputStream.Dispose();

            Encoding inputEncoding = this.context.Request.ContentEncoding;
            string inputStr = inputEncoding.GetString(buffer);


            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            object obj = jsSerializer.DeserializeObject(inputStr);

            inputStream.Close();
            inputStream.Dispose();

            Dictionary<string, object> paramDict = obj as Dictionary<string, object>;
            NameValueCollection queryStr = this.context.Request.QueryString;
            foreach (string name in queryStr)
            {
                paramDict.Add(name, queryStr[name]);
            }
            return paramDict;
        }
    }
}
