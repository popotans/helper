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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using Helper.TemplateEngine.Parser.AST;
using Helper.TemplateEngine.Parser;

namespace Helper.TemplateEngine
{
	/// <summary>
	/// 为模板引擎提供模板管理支持的外观类。
	/// </summary>
	/// <remarks>推荐从此类继承并派生一个自定义类来使用本模板引擎。</remarks>
	public class TemplateManager
	{
		#region 私有字段

		private bool _SilentErrors;

		private Dictionary<string, TemplateFunction> _Functions;
		private Dictionary<string, ITagHandler> _CustomTags;

		private VariableScope _Variables;		// current variable scope
		private Expression _CurrentExpression;	// current expression being evaluated

		private TextWriter _Writer;				// all output is sent here

		private Template _MainTemplate;			// main template to execute
		private Template _CurrentTemplate;		// current template being executed

		private ITemplateHandler _Handler;		// handler will be set as "this" object

		#endregion

		#region 构造函数

		/// <summary>
		/// 使用模板初始化模板管理器实例。
		/// </summary>
		/// <param name="template">指定一个模板实例。</param>
		public TemplateManager(Template template)
		{
			_MainTemplate = template;
			_CurrentTemplate = template;
			_SilentErrors = false;

			_Init();
		}

		#endregion

		#region 私有方法

		protected virtual void _Init()
		{
			_Functions = new Dictionary<string, TemplateFunction>(StringComparer.InvariantCultureIgnoreCase);
			_Variables = new VariableScope();

			this.Reset();

			_Functions.Add("equals", new TemplateFunction(FuncEquals));
			_Functions.Add("notequals", new TemplateFunction(FuncNotEquals));
			_Functions.Add("iseven", new TemplateFunction(FuncIsEven));
			_Functions.Add("isodd", new TemplateFunction(FuncIsOdd));
			_Functions.Add("isempty", new TemplateFunction(FuncIsEmpty));
			_Functions.Add("isnotempty", new TemplateFunction(FuncIsNotEmpty));
			_Functions.Add("isnumber", new TemplateFunction(FuncIsNumber));
			_Functions.Add("toupper", new TemplateFunction(FuncToUpper));
			_Functions.Add("tolower", new TemplateFunction(FuncToLower));
			_Functions.Add("isdefined", new TemplateFunction(FuncIsDefined));
			_Functions.Add("ifdefined", new TemplateFunction(FuncIfDefined));
			_Functions.Add("len", new TemplateFunction(FuncLen));
			_Functions.Add("tolist", new TemplateFunction(FuncToList));
			_Functions.Add("isnull", new TemplateFunction(FuncIsNull));
			_Functions.Add("not", new TemplateFunction(FuncNot));
			_Functions.Add("iif", new TemplateFunction(FuncIif));
			_Functions.Add("format", new TemplateFunction(FuncFormat));
			_Functions.Add("trim", new TemplateFunction(FuncTrim));
			_Functions.Add("filter", new TemplateFunction(FuncFilter));
			_Functions.Add("isgt", new TemplateFunction(FuncGt));
			_Functions.Add("islt", new TemplateFunction(FuncLt));
			_Functions.Add("compare", new TemplateFunction(FuncCompare));
			_Functions.Add("ior", new TemplateFunction(FuncOr));
			_Functions.Add("iand", new TemplateFunction(FuncAnd));
			_Functions.Add("comparenocase", new TemplateFunction(FuncCompareNoCase));
			_Functions.Add("stripnewlines", new TemplateFunction(FuncStripNewLines));
			_Functions.Add("typeof", new TemplateFunction(FuncTypeOf));
			_Functions.Add("cbool", new TemplateFunction(FuncCBool));
			_Functions.Add("cint", new TemplateFunction(FuncCInt));
			_Functions.Add("cdouble", new TemplateFunction(FuncCDouble));
			_Functions.Add("cdate", new TemplateFunction(FuncCDate));
			_Functions.Add("now", new TemplateFunction(FuncNow));
			_Functions.Add("createtypereference", new TemplateFunction(FuncCreateTypeReference));
		}

		#endregion

		#region 受保护方法

		/// <summary>
		/// 输出错误消息。
		/// </summary>
		/// <param name="ex">指定一个异常。</param>
		protected virtual void DisplayError(Exception ex)
		{
			if (ex is TemplateRuntimeException)
			{
				TemplateRuntimeException tex = (TemplateRuntimeException)ex;
				this.DisplayError(ex.Message, tex.Line, tex.Col);
			}
			else
				this.DisplayError(ex.Message, 0, 0);
		}

		/// <summary>
		/// 输出错误消息。
		/// </summary>
		/// <param name="msg">消息内容。</param>
		/// <param name="line">引发异常的代码所在的行数。</param>
		/// <param name="col">引发异常的代码所在的列数。</param>
		protected virtual void DisplayError(string msg, int line, int col)
		{
			if (!_SilentErrors)
				_Writer.Write("[ERROR ({0}, {1}): {2}]", line, col, msg);
		}

		/// <summary>
		/// 执行数组访问，并返回执行结果。
		/// </summary>
		/// <param name="arrayAccess">指定一个数组实例。</param>
		/// <returns>object</returns>
		protected object EvalArrayAccess(ArrayAccess arrayAccess)
		{
			object obj = this.EvalExpression(arrayAccess.Exp);
			object index = this.EvalExpression(arrayAccess.Index);

			if (obj is Array)
			{
				Array array = (Array)obj;

				if (index is Int32)
					return array.GetValue((int)index);
				else
					throw new TemplateRuntimeException("数组索引必须是整形数字。", arrayAccess.Line, arrayAccess.Col);
			}
			else
				return this.EvalMethodCall(obj, "get_Item", new object[] { index });
		}

		/// <summary>
		/// 执行预定义运算符，并返回执行结果。
		/// </summary>
		/// <param name="exp">指定一个预定义运算符。</param>
		/// <returns>object</returns>
		protected object EvalBinaryExpression(BinaryExpression exp)
		{
			switch (exp.Operator)
			{
				case TokenKind.OpOr:
					{
						object lhsValue = this.EvalExpression(exp.Lhs);
						if (Utility.ToBool(lhsValue))
							return true;

						object rhsValue = this.EvalExpression(exp.Rhs);
						return Utility.ToBool(rhsValue);
					}
				case TokenKind.OpAnd:
					{
						object lhsValue = this.EvalExpression(exp.Lhs);
						if (!Utility.ToBool(lhsValue))
							return false;

						object rhsValue = this.EvalExpression(exp.Rhs);
						return Utility.ToBool(rhsValue);
					}
				case TokenKind.OpIs:
					{
						object lhsValue = this.EvalExpression(exp.Lhs);
						object rhsValue = this.EvalExpression(exp.Rhs);

						return lhsValue.Equals(rhsValue);
					}
				case TokenKind.OpIsNot:
					{
						object lhsValue = this.EvalExpression(exp.Lhs);
						object rhsValue = this.EvalExpression(exp.Rhs);

						return !lhsValue.Equals(rhsValue);
					}
				case TokenKind.OpGt:
					{
						object lhsValue = this.EvalExpression(exp.Lhs);
						object rhsValue = this.EvalExpression(exp.Rhs);

						IComparable c1 = lhsValue as IComparable;
						IComparable c2 = rhsValue as IComparable;
						if (c1 == null || c2 == null)
							return false;
						else
							return c1.CompareTo(c2) == 1;
					}
				case TokenKind.OpLt:
					{
						object lhsValue = this.EvalExpression(exp.Lhs);
						object rhsValue = this.EvalExpression(exp.Rhs);

						IComparable c1 = lhsValue as IComparable;
						IComparable c2 = rhsValue as IComparable;
						if (c1 == null || c2 == null)
							return false;
						else
							return c1.CompareTo(c2) == -1;
					}
				case TokenKind.OpGte:
					{
						object lhsValue = this.EvalExpression(exp.Lhs);
						object rhsValue = this.EvalExpression(exp.Rhs);

						IComparable c1 = lhsValue as IComparable;
						IComparable c2 = rhsValue as IComparable;
						if (c1 == null || c2 == null)
							return false;
						else
							return c1.CompareTo(c2) >= 0;
					}
				case TokenKind.OpLte:
					{
						object lhsValue = this.EvalExpression(exp.Lhs);
						object rhsValue = this.EvalExpression(exp.Rhs);

						IComparable c1 = lhsValue as IComparable;
						IComparable c2 = rhsValue as IComparable;
						if (c1 == null || c2 == null)
							return false;
						else
							return c1.CompareTo(c2) <= 0;
					}
				default:
					throw new TemplateRuntimeException("遭遇未受支持的运算符：" + exp.Operator.ToString(), exp.Line, exp.Col);
			}
		}

		/// <summary>
		/// 执行参数列表，并返回执行结果。
		/// </summary>
		/// <param name="args">包含参数列表的表达式。</param>
		/// <returns>object[]</returns>
		protected object[] EvalArguments(Expression[] args)
		{
			object[] values = new object[args.Length];
			for (int i = 0; i < values.Length; i++)
				values[i] = this.EvalExpression(args[i]);

			return values;
		}

		/// <summary>
		/// 执行对象属性运算，并返回执行结果。
		/// </summary>
		/// <param name="obj">指定一个对象。</param>
		/// <param name="propertyName">指定该对象的属性名。</param>
		/// <returns>object</returns>
		protected static object EvalProperty(object obj, string propertyName)
		{
			if (obj is StaticTypeReference)
			{
				Type type = (obj as StaticTypeReference).Type;

				PropertyInfo pinfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Static);
				if (pinfo != null)
					return pinfo.GetValue(null, null);

				FieldInfo finfo = type.GetField(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.GetField | BindingFlags.Static);
				if (finfo != null)
					return finfo.GetValue(null);
				else
					throw new Exception("在对象 '" + type.Name + "' 中未找到字段或属性 '" + propertyName + "'。");
			}
			else
			{
				PropertyInfo pinfo = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance);

				if (pinfo != null)
					return pinfo.GetValue(obj, null);

				FieldInfo finfo = obj.GetType().GetField(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.GetField | BindingFlags.Instance);

				if (finfo != null)
					return finfo.GetValue(obj);
				else
					throw new Exception("在对象 '" + obj.GetType().Name + "' 中未找到字段或属性 '" + propertyName + "'");
			}
		}

		/// <summary>
		/// 执行自定义回调方法，并返回执行结果。
		/// </summary>
		/// <param name="obj">指定一个对象。</param>
		/// <param name="methodName">指定该对象的方法名。</param>
		/// <param name="args">指定该方法的参数列表。</param>
		/// <returns>object</returns>
		protected object EvalMethodCall(object obj, string methodName, object[] args)
		{
			Type[] types = new Type[args.Length];
			for (int i = 0; i < args.Length; i++)
				types[i] = args[i].GetType();

			if (obj is StaticTypeReference)
			{
				Type type = (obj as StaticTypeReference).Type;
				MethodInfo method = type.GetMethod(methodName,
					BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Static,
					null, types, null);

				if (method == null)
					throw new Exception(string.Format("在类型 {1} 中未找到方法 {0}。", methodName, type.Name));

				return method.Invoke(null, args);
			}
			else
			{
				MethodInfo method = obj.GetType().GetMethod(methodName,
					BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance,
					null, types, null);

				if (method == null)
					throw new Exception(string.Format("在类型 {1} 中未找到方法 {0}。", methodName, obj.GetType().Name));

				return method.Invoke(obj, args);
			}
		}

		/// <summary>
		/// 处理 If 语句。
		/// </summary>
		/// <param name="tagIf">包含 if 语句的标签实例。</param>
		protected void ProcessIf(TagIf tagIf)
		{
			bool condition = false;

			try
			{
				object value = this.EvalExpression(tagIf.Test);

				condition = Utility.ToBool(value);
			}
			catch (Exception ex)
			{
				this.DisplayError("遭遇非法的 If 语句。" + ex.Message,
					tagIf.Line, tagIf.Col);
				return;
			}

			if (condition)
				this.ProcessElements(tagIf.InnerElements);
			else
				this.ProcessElement(tagIf.FalseBranch);
		}

		/// <summary>
		/// 处理标签。
		/// </summary>
		/// <param name="tag">指定一个标签实例。</param>
		protected void ProcessTag(Tag tag)
		{
			string name = tag.Name.ToLowerInvariant();
			try
			{
				switch (name)
				{
					case "template":
						break;
					case "else":
						this.ProcessElements(tag.InnerElements);
						break;
					case "apply":
						object val = this.EvalExpression(tag.AttributeValue("template"));
						this.ProcessTemplate(val.ToString(), tag);
						break;
					case "foreach":
						this.ProcessForEach(tag);
						break;
					case "for":
						this.ProcessFor(tag);
						break;
					case "set":
						this.ProcessTagSet(tag);
						break;
					default:
						this.ProcessTemplate(tag.Name, tag);
						break;
				}
			}
			catch (TemplateRuntimeException ex)
			{
				this.DisplayError(ex);
			}
			catch (Exception ex)
			{
				this.DisplayError("处理标记时出错：'" + name + "': " + ex.Message, tag.Line, tag.Col);
			}
		}

		/// <summary>
		/// 处理 Set 标签。
		/// </summary>
		/// <param name="tag">包含 set 方法的标签实例。</param>
		protected void ProcessTagSet(Tag tag)
		{
			Expression expName = tag.AttributeValue("name");
			if (expName == null) throw new TemplateRuntimeException("Set 找不到必须的属性：name", tag.Line, tag.Col);

			Expression expValue = tag.AttributeValue("value");
			if (expValue == null) throw new TemplateRuntimeException("Set 找不到必须的属性：value", tag.Line, tag.Col);

			string name = this.EvalExpression(expName).ToString();
			if (!Utility.IsValidVariableName(name))
				throw new TemplateRuntimeException("指定了非法的标签名：'" + name + "'", expName.Line, expName.Col);

			object value = this.EvalExpression(expValue);

			this.SetValue(name, value);
		}

		/// <summary>
		/// 处理 foreach 标签。
		/// </summary>
		/// <param name="tag">包含 foreach 语句的标签实例。</param>
		protected void ProcessForEach(Tag tag)
		{
			Expression expCollection = tag.AttributeValue("collection");
			if (expCollection == null) throw new TemplateRuntimeException("Foreach 找不到必须的属性：collection", tag.Line, tag.Col);

			object collection = this.EvalExpression(expCollection);
			if (!(collection is IEnumerable)) throw new TemplateRuntimeException("foreach 的 Collection 必须可以转换为 enumerable", tag.Line, tag.Col);

			Expression expVar = tag.AttributeValue("var");
			if (expCollection == null) throw new TemplateRuntimeException("Foreach 找不到必须的属性：var", tag.Line, tag.Col);

			object varObject = this.EvalExpression(expVar);
			if (varObject == null) varObject = "foreach";

			string varname = varObject.ToString();

			Expression expIndex = tag.AttributeValue("index");
			string indexname = null;
			if (expIndex != null)
			{
				object obj = this.EvalExpression(expIndex);
				if (obj != null)
					indexname = obj.ToString();
			}

			IEnumerator ienum = ((IEnumerable)collection).GetEnumerator();
			int index = 0;
			while (ienum.MoveNext())
			{
				index++;
				object value = ienum.Current;
				_Variables[varname] = value;
				if (indexname != null)
					_Variables[indexname] = index;

				this.ProcessElements(tag.InnerElements);
			}
		}

		/// <summary>
		/// 处理 for 标签。
		/// </summary>
		/// <param name="tag">包含 for 语句的标签实例。</param>
		protected void ProcessFor(Tag tag)
		{
			Expression expFrom = tag.AttributeValue("from");
			if (expFrom == null) throw new TemplateRuntimeException("For 找不到必须的属性：from", tag.Line, tag.Col);

			Expression expTo = tag.AttributeValue("to");
			if (expTo == null) throw new TemplateRuntimeException("For 找不到必须的属性：to", tag.Line, tag.Col);

			Expression expIndex = tag.AttributeValue("index");
			if (expIndex == null) throw new TemplateRuntimeException("For 找不到必须的属性：index", tag.Line, tag.Col);

			object obj = this.EvalExpression(expIndex);
			string indexName = obj.ToString();

			int start = Convert.ToInt32(this.EvalExpression(expFrom));
			int end = Convert.ToInt32(this.EvalExpression(expTo));

			for (int index = start; index <= end; index++)
			{
				SetValue(indexName, index);
				this.ProcessElements(tag.InnerElements);
			}
		}

		/// <summary>
		/// 处理自定义标签。
		/// </summary>
		/// <param name="tag">指定一个自定义标签实例。</param>
		protected void ExecuteCustomTag(Tag tag)
		{
			ITagHandler tagHandler = _CustomTags[tag.Name];

			bool processInnerElements = true;
			bool captureInnerContent = false;

			tagHandler.TagBeginProcess(this, tag, ref processInnerElements, ref captureInnerContent);

			string innerContent = null;

			if (processInnerElements)
			{
				TextWriter saveWriter = _Writer;

				if (captureInnerContent)
					_Writer = new StringWriter();

				try
				{
					this.ProcessElements(tag.InnerElements);

					innerContent = _Writer.ToString();
				}
				finally
				{
					_Writer = saveWriter;
				}
			}

			tagHandler.TagEndProcess(this, tag, innerContent);
		}

		/// <summary>
		/// 处理模板。
		/// </summary>
		/// <param name="name">指定模板名。</param>
		/// <param name="tag">指定一个标签实例。</param>
		protected void ProcessTemplate(string name, Tag tag)
		{
			if (_CustomTags != null && _CustomTags.ContainsKey(name))
			{
				ExecuteCustomTag(tag);
				return;
			}

			Template useTemplate = _CurrentTemplate.FindTemplate(name);
			if (useTemplate == null)
			{
				string msg = string.Format("模板未找到：'{0}'", name);
				throw new TemplateRuntimeException(msg, tag.Line, tag.Col);
			}

			// process inner elements and save content
			TextWriter saveWriter = _Writer;
			_Writer = new StringWriter();
			string content = string.Empty;

			try
			{
				this.ProcessElements(tag.InnerElements);

				content = _Writer.ToString();
			}
			finally
			{
				_Writer = saveWriter;
			}

			Template saveTemplate = _CurrentTemplate;
			_Variables = new VariableScope(_Variables);
			_Variables["innerText"] = content;

			try
			{
				foreach (TagAttribute attrib in tag.Attributes)
				{
					object val = this.EvalExpression(attrib.Expression);
					_Variables[attrib.Name] = val;
				}

				_CurrentTemplate = useTemplate;
				this.ProcessElements(_CurrentTemplate.Elements);
			}
			finally
			{
				_Variables = _Variables.Parent;
				_CurrentTemplate = saveTemplate;
			}
		}

		/// <summary>
		/// 处理指定的元标签。
		/// </summary>
		/// <param name="elem">指定一个元标签实例。</param>
		protected void ProcessElement(Element elem)
		{
			if (elem is Text)
			{
				Text text = (Text)elem;
				this.WriteValue(text.Data);
			}
			else if (elem is Expression)
				this.ProcessExpression((Expression)elem);
			else if (elem is TagIf)
				this.ProcessIf((TagIf)elem);
			else if (elem is Tag)
				this.ProcessTag((Tag)elem);
		}

		/// <summary>
		/// 处理表达式。
		/// </summary>
		/// <param name="exp">指定一个表达式实例。</param>
		protected void ProcessExpression(Expression exp)
		{
			object value = this.EvalExpression(exp);
			this.WriteValue(value);
		}

		#endregion

		#region 公有方法

		/// <summary>
		/// 从模板代码初始化模板管理器实例。
		/// </summary>
		/// <param name="template">包含模板代码的字符串。</param>
		/// <returns>TemplateManager</returns>
		public static TemplateManager FromString(string template)
		{
			Template itemplate = Template.FromString("", template);
			return new TemplateManager(itemplate);
		}

		/// <summary>
		/// 从模板文件初始化模板管理器实例。
		/// </summary>
		/// <param name="filename">包含模板文件完整路径的字符串。</param>
		/// <returns>TemplateManager</returns>
		public static TemplateManager FromFile(string filename)
		{
			Template template = Template.FromFile("", filename);
			return new TemplateManager(template);
		}

		/// <summary>
		/// 注册自定义标签处理器。
		/// </summary>
		public void RegisterCustomTag(string tagName, ITagHandler handler)
		{
			this.CustomTags.Add(tagName, handler);
		}

		/// <summary>
		/// 获取一个 Boolea 值，确定指定的标签名是否已经存在。
		/// </summary>
		public bool IsCustomTagRegistered(string tagName)
		{
			return this.CustomTags.ContainsKey(tagName);
		}

		/// <summary>
		/// 注销自定义标签处理器。
		/// </summary>
		public void UnRegisterCustomTag(string tagName)
		{
			this.CustomTags.Remove(tagName);
		}

		/// <summary>
		/// 添加一个模板实例。
		/// </summary>
		/// <param name="template">指定一个模板实例。</param>
		public void AddTemplate(Template template)
		{
			_MainTemplate.Templates.Add(template.Name, template);
		}

		/// <summary>
		/// 执行表达式，并返回执行结果。
		/// </summary>
		/// <param name="exp">指定一个表达式实例。</param>
		/// <returns>object</returns>
		public object EvalExpression(Expression exp)
		{
			_CurrentExpression = exp;

			try
			{
				if (exp is StringLiteral)
					return ((StringLiteral)exp).Content;
				else if (exp is Name)
				{
					return GetValue(((Name)exp).Id);
				}
				else if (exp is FieldAccess)
				{
					FieldAccess fa = (FieldAccess)exp;
					object obj = this.EvalExpression(fa.Exp);
					string propertyName = fa.Field;
					return EvalProperty(obj, propertyName);
				}
				else if (exp is MethodCall)
				{
					MethodCall ma = (MethodCall)exp;
					object obj = this.EvalExpression(ma.CallObject);
					string methodName = ma.Name;

					return EvalMethodCall(obj, methodName, EvalArguments(ma.Args));
				}
				else if (exp is IntLiteral)
					return ((IntLiteral)exp).Value;
				else if (exp is DoubleLiteral)
					return ((DoubleLiteral)exp).Value;
				else if (exp is FCall)
				{
					FCall fcall = (FCall)exp;
					if (!_Functions.ContainsKey(fcall.Name))
					{
						string msg = string.Format("函数未定义：{0}", fcall.Name);
						throw new TemplateRuntimeException(msg, exp.Line, exp.Col);
					}

					TemplateFunction func = _Functions[fcall.Name];
					object[] values = EvalArguments(fcall.Args);

					return func(values);
				}
				else if (exp is StringExpression)
				{
					StringExpression stringExp = (StringExpression)exp;
					StringBuilder sb = new StringBuilder();
					foreach (Expression ex in stringExp.Expressions)
						sb.Append(this.EvalExpression(ex));

					return sb.ToString();
				}
				else if (exp is BinaryExpression)
					return EvalBinaryExpression(exp as BinaryExpression);
				else if (exp is ArrayAccess)
					return EvalArrayAccess(exp as ArrayAccess);
				else
					throw new TemplateRuntimeException("无效的表达式类型：" + exp.GetType().Name, exp.Line, exp.Col);

			}
			catch (TemplateRuntimeException ex)
			{
				this.DisplayError(ex);
				return null;
			}
			catch (Exception ex)
			{
				this.DisplayError(new TemplateRuntimeException(ex.Message, _CurrentExpression.Line, _CurrentExpression.Col));
				return null;
			}
		}

		/// <summary>
		/// 将指定内容写入输出流。
		/// </summary>
		/// <param name="value">要输出的内容。</param>
		public void WriteValue(object value)
		{
			if (value == null)
				_Writer.Write("[null]");
			else
				_Writer.Write(value);
		}

		/// <summary>
		/// 为指定的模板变量赋值。
		/// </summary>
		/// <param name="name">模板变量名称。</param>
		/// <param name="value">指定一个变量值。</param>
		public void SetValue(string name, object value)
		{
			_Variables[name] = value;
		}

		/// <summary>
		/// 获取指定模板变量的值。
		/// </summary>
		/// <param name="name">模板变量名称。</param>
		/// <returns>object</returns>
		public object GetValue(string name)
		{
			if (_Variables.IsDefined(name))
				return _Variables[name];
			else
				throw new Exception("没有找到变量 '" + name + "' 的值，或者该变量没有定义过。");
		}

		/// <summary>
		/// 处理模板。将处理结果保存到字符编写器中。
		/// </summary>
		/// <param name="writer">指定一个字符编写器。</param>
		public void Process(TextWriter writer)
		{
			_Writer = writer;
			_CurrentTemplate = _MainTemplate;

			if (_Handler != null)
			{
				SetValue("this", _Handler);
				_Handler.BeforeProcess(this);
			}

			this.ProcessElements(_MainTemplate.Elements);

			if (_Handler != null)
				_Handler.AfterProcess(this);
		}

		/// <summary>
		/// 处理模板。并将结果作为字符串返回。
		/// </summary>
		/// <returns></returns>
		public string Process()
		{
			StringWriter writer = new StringWriter();
			Process(writer);
			return writer.ToString();
		}

		/// <summary>
		/// 重置模板变量库。
		/// </summary>
		public void Reset()
		{
			_Variables.Clear();

			this.SetValue("true", true);
			this.SetValue("false", false);
			this.SetValue("null", null);
		}

		/// <summary>
		/// 处理元标签库。
		/// </summary>
		/// <param name="list">包含元标签集合的列表。</param>
		public void ProcessElements(List<Element> list)
		{
			foreach (Element elem in list)
			{
				this.ProcessElement(elem);
			}
		}

		#region 内置函数

		/// <summary>
		/// 检查自定义函数的形式参数的数量是否正确。
		/// </summary>
		/// <param name="count">标准数量。</param>
		/// <param name="funcName">函数名。</param>
		/// <param name="args">参数列表。</param>
		/// <returns>bool</returns>
		public bool CheckArgCount(int count, string funcName, object[] args)
		{
			if (count != args.Length)
			{
				DisplayError(string.Format("函数 {0} 需要 {1} 个参数，但是找到了 {2} 个。", funcName, count, args.Length), _CurrentExpression.Line, _CurrentExpression.Col);
				return false;
			}
			else
				return true;
		}

		/// <summary>
		/// 检查自定义函数的形式参数的数量是否正确。
		/// </summary>
		/// <param name="count1">标准数量最小值。</param>
		/// <param name="count2">标准数量最大值。</param>
		/// <param name="funcName">函数名。</param>
		/// <param name="args">参数列表。</param>
		/// <returns>bool</returns>
		public bool CheckArgCount(int count1, int count2, string funcName, object[] args)
		{
			if (args.Length < count1 || args.Length > count2)
			{
				string msg = string.Format("函数 {0} 需要 {1} 至 {2} 个参数，但是找到了 {3} 个。", funcName, count1, count2, args.Length);
				DisplayError(msg, _CurrentExpression.Line, _CurrentExpression.Col);
				return false;
			}
			else
				return true;
		}

		/// <summary>
		/// 内置函数。确定当前处理的标签索引是否是偶数。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncIsEven(object[] args)
		{
			if (!CheckArgCount(1, "iseven", args))
				return null;

			try
			{
				int value = Convert.ToInt32(args[0]);
				return value % 2 == 0;
			}
			catch (FormatException)
			{
				throw new TemplateRuntimeException("IsEven 无法将指定的值转换为 int", _CurrentExpression.Line, _CurrentExpression.Col);
			}
		}

		/// <summary>
		/// 内置函数。确定当前处理的标签索引是否是奇数。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncIsOdd(object[] args)
		{
			if (!CheckArgCount(1, "isdd", args))
				return null;

			try
			{
				int value = Convert.ToInt32(args[0]);
				return value % 2 == 1;
			}
			catch (FormatException)
			{
				throw new TemplateRuntimeException("IsOdd 无法将指定的值转换为 int", _CurrentExpression.Line, _CurrentExpression.Col);
			}
		}

		/// <summary>
		/// 内置函数。确定指定值是否为空。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncIsEmpty(object[] args)
		{
			if (!CheckArgCount(1, "isempty", args))
				return null;

			string value = args[0].ToString();
			return value.Length == 0;
		}

		/// <summary>
		/// 内置函数。确定指定值是否不为空。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncIsNotEmpty(object[] args)
		{
			if (!CheckArgCount(1, "isnotempty", args))
				return null;

			if (args[0] == null)
				return false;

			string value = args[0].ToString();
			return value.Length > 0;
		}

		/// <summary>
		/// 内置函数。确定两个输入值是否相等。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncEquals(object[] args)
		{
			if (!CheckArgCount(2, "equals", args))
				return null;

			return args[0].Equals(args[1]);
		}

		/// <summary>
		/// 内置函数。确定两个输入值是否不相等。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncNotEquals(object[] args)
		{
			if (!CheckArgCount(2, "notequals", args))
				return null;

			return !args[0].Equals(args[1]);
		}

		/// <summary>
		/// 内置函数。确定输入值是否为数字。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncIsNumber(object[] args)
		{
			if (!CheckArgCount(1, "isnumber", args))
				return null;

			try
			{
				int value = Convert.ToInt32(args[0]);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		/// <summary>
		/// 内置函数。返回输入值的大写副本。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncToUpper(object[] args)
		{
			if (!CheckArgCount(1, "toupper", args))
				return null;

			return args[0].ToString().ToUpper();
		}

		/// <summary>
		/// 内置函数。返回输入值的小写副本。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncToLower(object[] args)
		{
			if (!CheckArgCount(1, "toupper", args))
				return null;

			return args[0].ToString().ToLower();
		}

		/// <summary>
		/// 内置函数。返回输入值的长度。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncLen(object[] args)
		{
			if (!CheckArgCount(1, "len", args))
				return null;

			return args[0].ToString().Length;
		}

		/// <summary>
		/// 内置函数。确定输入值是否已经定义。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncIsDefined(object[] args)
		{
			if (!CheckArgCount(1, "isdefined", args))
				return null;

			return _Variables.IsDefined(args[0].ToString());
		}

		/// <summary>
		/// 内置函数。如果第一输入值已经定义，则返回第二输入值。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncIfDefined(object[] args)
		{
			if (!CheckArgCount(2, "ifdefined", args))
				return null;

			if (_Variables.IsDefined(args[0].ToString()))
			{
				return args[1];
			}
			else
				return string.Empty;
		}

		/// <summary>
		/// 内置函数。将输入的集合转换为指定字符分隔的列表。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncToList(object[] args)
		{
			if (!CheckArgCount(2, 3, "tolist", args))
				return null;

			object list = args[0];

			string property;
			string delim;

			if (args.Length == 3)
			{
				property = args[1].ToString();
				delim = args[2].ToString();
			}
			else
			{
				property = string.Empty;
				delim = args[1].ToString();
			}

			if (!(list is IEnumerable))
			{
				throw new TemplateRuntimeException("tolist 的第一参数必须可以转化为 IEnumerable", _CurrentExpression.Line, _CurrentExpression.Col);
			}

			IEnumerator ienum = ((IEnumerable)list).GetEnumerator();
			StringBuilder sb = new StringBuilder();
			int index = 0;
			while (ienum.MoveNext())
			{
				if (index > 0)
					sb.Append(delim);

				if (args.Length == 2) // do not evalulate property
					sb.Append(ienum.Current);
				else
				{
					sb.Append(TemplateManager.EvalProperty(ienum.Current, property));
				}
				index++;
			}

			return sb.ToString();

		}

		/// <summary>
		/// 内置函数。确定输入值是否为 null。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncIsNull(object[] args)
		{
			if (!CheckArgCount(1, "isnull", args))
				return null;

			return args[0] == null;
		}

		/// <summary>
		/// 内置函数。对输入值执行逻辑“非”运算。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncNot(object[] args)
		{
			if (!CheckArgCount(1, "not", args))
				return null;

			if (args[0] is bool)
				return !(bool)args[0];
			else
			{
				throw new TemplateRuntimeException("'not'函数的参数不是布尔型", _CurrentExpression.Line, _CurrentExpression.Col);
			}
		}

		/// <summary>
		/// 内置函数。对输入值执行条件（三目）运算。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncIif(object[] args)
		{
			if (!CheckArgCount(3, "iif", args))
				return null;

			if (args[0] is bool)
			{
				bool test = (bool)args[0];
				return test ? args[1] : args[2];
			}
			else
			{
				throw new TemplateRuntimeException("'iif'函数的参数不是布尔型", _CurrentExpression.Line, _CurrentExpression.Col);
			}
		}

		/// <summary>
		/// 内置函数。为输入值执行格式化。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncFormat(object[] args)
		{
			if (!CheckArgCount(2, "format", args))
				return null;

			string format = args[1].ToString();

			if (args[0] is IFormattable)
				return ((IFormattable)args[0]).ToString(format, null);
			else
				return args[0].ToString();
		}

		/// <summary>
		/// 内置函数。从输入值移除数组中指定的所有首部和尾部匹配项。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncTrim(object[] args)
		{
			if (!CheckArgCount(1, 3, "trim", args))
				return null;

			string text = args[0].ToString();

			char[] cs = new char[] { };
			if (args.Length >= 2) cs = args[1].ToString().ToCharArray();
			if (cs.Length == 0) cs = new char[] { ' ' };

			string sd = "";
			if (args.Length == 3) sd = args[2].ToString();
			sd = sd.ToLower();

			switch (sd)
			{
				case "l":
				case "left":
				case "s":
				case "start":
					text = text.TrimStart(cs);
					break;
				case "r":
				case "right":
				case "e":
				case "end":
					text = text.TrimEnd(cs);
					break;
				default:
					text = text.Trim(cs);
					break;
			}

			return text;
		}

		/// <summary>
		/// 内置函数。从输入集合过滤指定内容。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncFilter(object[] args)
		{
			if (!CheckArgCount(2, "filter", args))
				return null;

			object list = args[0];

			string property;
			property = args[1].ToString();

			if (!(list is IEnumerable))
			{
				throw new TemplateRuntimeException("filter 的第一参数必须可以转化为 IEnumerable", _CurrentExpression.Line, _CurrentExpression.Col);
			}

			IEnumerator ienum = ((IEnumerable)list).GetEnumerator();
			List<object> newList = new List<object>();

			while (ienum.MoveNext())
			{
				object val = TemplateManager.EvalProperty(ienum.Current, property);
				if (val is bool && (bool)val)
					newList.Add(ienum.Current);
			}

			return newList;
		}

		/// <summary>
		/// 内置函数。确认第一输入值是否大于第二输入值。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncGt(object[] args)
		{
			if (!CheckArgCount(2, "gt", args))
				return null;

			IComparable c1 = args[0] as IComparable;
			IComparable c2 = args[1] as IComparable;
			if (c1 == null || c2 == null)
				return false;
			else
				return c1.CompareTo(c2) == 1;
		}

		/// <summary>
		/// 内置函数。确认第一输入值是否小于第二输入值。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncLt(object[] args)
		{
			if (!CheckArgCount(2, "lt", args))
				return null;

			IComparable c1 = args[0] as IComparable;
			IComparable c2 = args[1] as IComparable;
			if (c1 == null || c2 == null)
				return false;
			else
				return c1.CompareTo(c2) == -1;
		}

		/// <summary>
		/// 内置函数。确认第一输入值是否等于第二输入值。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncCompare(object[] args)
		{
			if (!CheckArgCount(2, "compare", args))
				return null;

			IComparable c1 = args[0] as IComparable;
			IComparable c2 = args[1] as IComparable;
			if (c1 == null || c2 == null)
				return false;
			else
				return c1.CompareTo(c2);
		}

		/// <summary>
		/// 内置函数。为两个输入值执行逻辑“或”运算。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncOr(object[] args)
		{
			if (!CheckArgCount(2, "or", args))
				return null;

			if (args[0] is bool && args[1] is bool)
				return (bool)args[0] || (bool)args[1];
			else
				return false;
		}

		/// <summary>
		/// 内置函数。为两个输入值执行逻辑“与”运算。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncAnd(object[] args)
		{
			if (!CheckArgCount(2, "add", args))
				return null;

			if (args[0] is bool && args[1] is bool)
				return (bool)args[0] && (bool)args[1];
			else
				return false;
		}

		/// <summary>
		/// 内置函数。确定两个输入值是否相等，而忽略大小写。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncCompareNoCase(object[] args)
		{
			if (!CheckArgCount(2, "compareNoCase", args))
				return null;

			string s1 = args[0].ToString();
			string s2 = args[1].ToString();

			return string.Compare(s1, s2, true) == 0;
		}

		/// <summary>
		/// 内置函数。清除空行，并将其转换为一个空格。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncStripNewLines(object[] args)
		{
			if (!CheckArgCount(1, "StripNewLines", args))
				return null;

			string s1 = args[0].ToString();
			return s1.Replace(Environment.NewLine, " ");
		}

		/// <summary>
		/// 内置函数。获取指定实例的 System.Type。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncTypeOf(object[] args)
		{
			if (!CheckArgCount(1, "TypeOf", args))
				return null;

			return args[0].GetType().Name;

		}

		/// <summary>
		/// 内置函数。将输入值转换为 bool。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncCBool(object[] args)
		{
			return Utility.ToBool(args[0]);
		}

		/// <summary>
		/// 内置函数。将输入值转换为 int。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncCInt(object[] args)
		{
			if (!CheckArgCount(1, "cint", args))
				return null;

			return Convert.ToInt32(args[0]);
		}

		/// <summary>
		/// 内置函数。将输入值转换为 double。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncCDouble(object[] args)
		{
			if (!CheckArgCount(1, "cdouble", args))
				return null;

			return Convert.ToDouble(args[0]);
		}

		/// <summary>
		/// 内置函数。将输入值转换为 DateTime。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncCDate(object[] args)
		{
			if (!CheckArgCount(1, "cdate", args))
				return null;

			return Convert.ToDateTime(args[0]);
		}

		/// <summary>
		/// 内置函数。返回当前表示当前时间的 DateTime。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncNow(object[] args)
		{
			if (!CheckArgCount(0, "now", args))
				return null;

			return DateTime.Now;
		}

		/// <summary>
		/// 内置函数。创建类型引用。
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public object FuncCreateTypeReference(object[] args)
		{
			if (!CheckArgCount(1, "createtypereference", args))
				return null;

			string typeName = args[0].ToString();


			Type type = System.Type.GetType(typeName, false, true);
			if (type != null)
				return new StaticTypeReference(type);

			Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly asm in asms)
			{
				type = asm.GetType(typeName, false, true);
				if (type != null)
					return new StaticTypeReference(type);
			}

			throw new TemplateRuntimeException("无法创建类型 " + typeName + ".", _CurrentExpression.Line, _CurrentExpression.Col);
		}

		#endregion

		#endregion

		#region 公有属性

		/// <summary>
		/// 获取自定义标签的集合。
		/// </summary>
		public Dictionary<string, ITagHandler> CustomTags
		{
			get
			{
				if (_CustomTags == null)
					_CustomTags = new Dictionary<string, ITagHandler>(StringComparer.CurrentCultureIgnoreCase);
				return _CustomTags;
			}
		}

		/// <summary>
		/// 设置或获取模板自定义方法接口。
		/// </summary>
		public ITemplateHandler Handler
		{
			get { return _Handler; }
			set { _Handler = value; }
		}

		/// <summary>
		/// 设置或获取是否在输出流中隐藏错误信息。默认为 false，即显示错误信息。
		/// </summary>
		public bool SilentErrors
		{
			get { return _SilentErrors; }
			set { _SilentErrors = value; }
		}

		/// <summary>
		/// 设置或获取主模板。
		/// </summary>
		public Template MainTemplate
		{
			get { return _MainTemplate; }
			set { _MainTemplate = value; }
		}

		/// <summary>
		/// 设置或获取当前模板。
		/// </summary>
		public Template CurrentTemplate
		{
			get { return _CurrentTemplate; }
			set { _CurrentTemplate = value; }
		}

		/// <summary>
		/// 获取当前模板使用的函数库。
		/// </summary>
		public Dictionary<string, TemplateFunction> Functions
		{
			get { return _Functions; }
		}

		#endregion
	}
}
