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

namespace VMoom.TemplateEngine
{
	/// <summary>
	/// 提供变量标签视图管理功能。
	/// </summary>
	public sealed class VariableScope
	{
		#region 私有字段

		private VariableScope _Parent;
		private Dictionary<string, object> _Values;

		#endregion

		#region 构造函数

		/// <summary>
		/// 初始化变量视图。
		/// </summary>
		public VariableScope()
			: this(null)
		{
		}

		/// <summary>
		/// 初始化变量视图，并为其指定父视图。
		/// </summary>
		/// <param name="parent"></param>
		public VariableScope(VariableScope parent)
		{
			_Parent = parent;
			_Values = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
		}

		#endregion

		#region 公有方法
		
		/// <summary>
		/// 清空视图。
		/// </summary>
		public void Clear()
		{
			_Values.Clear();
		}

		/// <summary>
		/// 判断指定的标签是否已经声明（同时会从父视图中查找）。
		/// </summary>
		public bool IsDefined(string name)
		{
			if (_Values.ContainsKey(name))
				return true;
			else if (_Parent != null)
				return _Parent.IsDefined(name);
			else
				return false;
		}

		#endregion

		#region 公有属性

		/// <summary>
		/// 获取指定名称的标签取值。
		/// </summary>
		/// <param name="name">包含视图标签名称的字符串。</param>
		/// <returns>object</returns>
		public object this[string name]
		{
			get
			{
				if (!_Values.ContainsKey(name))
				{
					if (_Parent != null)
						return _Parent[name];
					else
						return null;
				}
				else
					return _Values[name];
			}

			set { _Values[name] = value; }
		}

		/// <summary>
		/// 获取视图的父级视图。
		/// </summary>
		public VariableScope Parent
		{
			get { return _Parent; }
		}

		#endregion
	}
}
