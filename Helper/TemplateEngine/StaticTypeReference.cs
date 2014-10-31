/* ����߰�Ȩ��ʶ����Ҫɾ����Щ��Ȩ������
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
 * ��Ʒ���ƣ���ĭģ������
 * ��Ʒ���ߣ�Guidy
 * ��Ȩ���У���ĭ������ [vMoom Studio]
 * --------------------------------------------------------------------------------------
 * ��ģ�������޸��� AderTemplates���ʱ������Ȩע�ͣ�ͬʱ��� GNU Э����з�����
 * --------------------------------------------------------------------------------------
 * �ٷ���վ��http://www.vmoom.net/
 * ������̳��http://bbs.vmoom.net/
 * ------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.TemplateEngine
{
	/// <summary>
	/// Ϊ���þ�̬�����ṩ֧�֡�
	/// </summary>
	public sealed class StaticTypeReference
	{
		#region ˽���ֶ�

		private readonly Type type;

		#endregion

		#region ���캯��

		/// <summary>
		/// ��ʼ����̬����ʵ����
		/// </summary>
		/// <param name="type">����Ҫ���õķ��������Եȶ���� System.Type��</param>
		public StaticTypeReference(Type type)
		{
			this.type = type;
		}

		#endregion

		#region ��������

		/// <summary>
		/// ��ȡҪ���õķ��������Եȶ���� System.Type��
		/// </summary>
		public Type Type
		{
			get { return type; }
		}

		#endregion
	}
}
