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

namespace Helper.TemplateEngine.Parser
{
	/// <summary>
	/// 表示模板解析过程中发生的错误。
	/// </summary>
	public class ParseException : Exception
	{
		#region 私有字段

		private int _Line;
		private int _Col;

		#endregion

		#region 构造函数
		
		/// <summary>
		/// 初始化模板解析错误。
		/// </summary>
		/// <param name="msg">错误消息内容。</param>
		/// <param name="line">获取引发异常的代码所在的行数。</param>
		/// <param name="col">获取引发异常的代码所在的列数。</param>
		public ParseException(string msg, int line, int col)
			: base(msg)
		{
			_Line = line;
			_Col = col;
		}

		#endregion

		#region 公有属性

		/// <summary>
		/// 获取引发异常的代码所在的列数。
		/// </summary>
		public int Col
		{
			get { return _Col; }
		}

		/// <summary>
		/// 获取引发异常的代码所在的行数。
		/// </summary>
		public int Line
		{
			get { return _Line; }
		}

		#endregion
	}
}
