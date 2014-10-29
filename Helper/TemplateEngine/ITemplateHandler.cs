﻿/* 请提高版权意识，不要删除这些版权声明。
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
	/// 为模板提供自定义方法的接口。
	/// </summary>
	public interface ITemplateHandler
	{
		/// <summary>
		/// 在模板处理过程之前运行的方法。
		/// </summary>
		/// <param name="manager">指定一个模板管理器实例。</param>
		void BeforeProcess(TemplateManager manager);

		/// <summary>
		/// 在模板处理过程之后运行的方法。
		/// </summary>
		/// <param name="manager">指定一个模板管理器实例。</param>
		void AfterProcess(TemplateManager manager);
	}
}
