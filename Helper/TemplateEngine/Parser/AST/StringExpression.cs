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
	/// 表示字符串表达式。
	/// </summary>
	public class StringExpression : Expression
	{
		#region 私有字段
		
		private List<Expression> _Exps;

		#endregion

		#region 构造函数
		
		/// <summary>
		/// 初始化字符串表达式。
		/// </summary>
		/// <param name="line">指定字符串表达式所在位置的行数。</param>
		/// <param name="col">指定字符串表达式所在位置的列数。</param>
		public StringExpression(int line, int col)
			: base(line, col)
		{
			_Exps = new List<Expression>();
		}

		#endregion

		#region 公有方法
		
		/// <summary>
		/// 添加字符串表达式的实例。
		/// </summary>
		/// <param name="exp">指定一个表达式。</param>
		public void Add(Expression exp)
		{
			_Exps.Add(exp);
		}

		#endregion

		#region 公有属性
		
		/// <summary>
		/// 获取字符串表达式的数量。
		/// </summary>
		public int ExpCount
		{
			get { return _Exps.Count; }
		}

		/// <summary>
		/// 获取指定索引位置的字符串表达式。
		/// </summary>
		/// <param name="index">指定索引数字。</param>
		/// <returns>Expression</returns>
		public Expression this[int index]
		{
			get { return _Exps[index]; }
		}

		/// <summary>
		/// 获取字符串表达式集合。
		/// </summary>
		public IEnumerable<Expression> Expressions
		{
			get
			{
				for (int i = 0; i < _Exps.Count; i++)
					yield return _Exps[i];
			}
		}

		#endregion
	}
}
