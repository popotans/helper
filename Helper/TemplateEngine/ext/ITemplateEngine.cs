using System;
using System.Collections.Generic;
using System.Text;

namespace VMoom.TemplateEngine
{
    /// <summary>
    /// 所有引擎构建器接口
    /// </summary>
    public interface ITemplateEngine
    {
        /// <summary>
        /// 解析模板
        /// </summary>
        /// <returns></returns>
        string Parser();

        /// <summary>
        /// 解析指定模板内容
        /// </summary>
        /// <param name="templateContent"></param>
        /// <returns></returns>
        string Parser(string templateContent);

        /// <summary>
        /// 模板内容
        /// </summary>
        string TemplateContent { get; set; }

        /// <summary>
        /// 模板引擎管理器
        /// </summary>
        TemplateManager TemplateManager { get; }

        void Set(string key, object val);
    }
}
