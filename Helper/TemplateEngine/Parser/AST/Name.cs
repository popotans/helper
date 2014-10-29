/* 请提高版权意识，不要删除这些版权声明。
 * --------------------------------------------------------------------------------------
 * AderTemplates
 * (C) Andrew Deren 2004
 * http://www.adersoftware.com
 *
 * This file is part of AderTemplate
 * AderTemplate is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * AderTemplate is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with AderTemplate; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 * --------------------------------------------------------------------------------------
 * 产品名称：问沫模板引擎
 * 产品作者：Guidy
 * 版权所有：问沫工作室 [vMoom Studio]
 * --------------------------------------------------------------------------------------
 * 本模板引擎修改自 AderTemplates，故保留其版权注释，同时遵从 GNU 协议进行发布。
 * --------------------------------------------------------------------------------------
 * 官方网站：http://www.vmoom.net/
 * 技术论坛：http://bbs.vmoom.net/
 * ------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;


namespace VMoom.TemplateEngine.Parser.AST
{
	/// <summary>
	/// 表示命名表达式。
	/// </summary>
	public class Name : Expression
	{
		#region 私有字段
		
		private string _Id;

		#endregion

		#region 构造函数

		/// <summary>
		/// 初始化命名表达式。
		/// </summary>
		/// <param name="line">指定命名表达式所在位置的行数。</param>
		/// <param name="col">指定命名表达式所在位置的列数。</param>
		/// <param name="id">指定命名表达式的标识符。</param>
		public Name(int line, int col, string id)
			: base(line, col)
		{
			_Id = id;
		}

		#endregion

		#region 公有属性

		/// <summary>
		/// 获取命名表达式的标识符。
		/// </summary>
		public string Id
		{
			get { return _Id; }
		}

		#endregion
	}
}
