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
using System.Text.RegularExpressions;

namespace VMoom.TemplateEngine
{
	/// <summary>
	/// 提供常用功能的静态类。
	/// </summary>
	public static class Utility
	{
		#region 私有字段

		private static object _SyncObject = new object();
		private static Regex _RegExVarName;

		#endregion

		#region 公有方法

		/// <summary>
		/// 从指定的表达式获取 Boolea 型结果。
		/// </summary>
		/// <param name="obj">任何有效的表达式。</param>
		/// <returns>bool</returns>
		public static bool ToBool(object obj)
		{
			if (obj is bool)
				return (bool)obj;
			else if (obj is string)
			{
				string str = (string)obj;
				if (string.Compare(str, "true", true) == 0)
					return true;
				else if (string.Compare(str, "yes", true) == 0)
					return true;
				else if (str == "1")
					return true;
				else
					return false;
			}
			else if (obj is int)
			{
				int i = (int)obj;
				if (i == 1)
					return true;
				else
					return false;
			}
			else
				return false;
		}

		/// <summary>
		/// 判断指定的变量标签名称是否合法。
		/// </summary>
		/// <param name="name">包含变量标签名称的字符串。</param>
		/// <returns>bool</returns>
		public static bool IsValidVariableName(string name)
		{
			return RegExVarName.IsMatch(name);
		}

		#endregion

		#region 私有属性

		/// <summary>
		/// 获取用于匹配变量标签名称的正则表达式对象。
		/// </summary>
		private static Regex RegExVarName
		{
			get
			{
				if (_RegExVarName == null)
				{
					System.Threading.Monitor.Enter(_SyncObject);
					if (_RegExVarName == null)
					{
						try
						{
							_RegExVarName = new Regex("^[a-zA-Z_][a-zA-Z0-9_]*$", RegexOptions.Compiled);
						}
						finally
						{
							System.Threading.Monitor.Exit(_SyncObject);
						}
					}
				}

				return _RegExVarName;
			}
		}

		#endregion
	}
}
