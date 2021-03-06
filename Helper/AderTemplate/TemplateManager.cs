﻿/*****************************************************
 * AderTemplates
 * (C) Andrew Deren 2004
 * http://www.adersoftware.com
 *
 *	This file is part of AderTemplate
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
 * along with Foobar; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *****************************************************/

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#endregion

using System.Reflection;
using System.IO;
using Ader.TemplateEngine.Parser.AST;

namespace Ader.TemplateEngine
{
	public delegate object TemplateFunction(object[] args);

	public class TemplateManager
	{
		Dictionary<string, TemplateFunction> functions;

		VariableScope variables;		// current variable scope
		Expression currentExpression;	// current expression being evaluated

		TextWriter writer;				// all output is sent here

		Template mainTemplate;			// main template to execute
		Template currentTemplate;		// current template being executed

		/// <summary>
		/// create template manager using a template
		/// </summary>
		public TemplateManager(Template template)
		{
			this.mainTemplate = template;
			this.currentTemplate = template;

			Init();
		}

		public static TemplateManager FromString(string template)
		{
			Template itemplate = Template.FromString("", template);
			return new TemplateManager(itemplate);
		}

		public static TemplateManager FromFile(string filename)
		{
			Template template = Template.FromFile("", filename);
			return new TemplateManager(template);
		}

		/// <summary>
		/// adds template that can be used within execution 
		/// </summary>
		/// <param name="template"></param>
		public void AddTemplate(Template template)
		{
			mainTemplate.Templates.Add(template.Name, template);
		}

		void Init()
		{
			this.functions = new Dictionary<string, TemplateFunction>(StringComparer.InvariantCultureIgnoreCase);

			this.variables = new VariableScope();

			functions.Add("equals", new TemplateFunction(FuncEquals));
			functions.Add("notequals", new TemplateFunction(FuncNotEquals));
			functions.Add("iseven", new TemplateFunction(FuncIsEven));
			functions.Add("isodd", new TemplateFunction(FuncIsOdd));
			functions.Add("isempty", new TemplateFunction(FuncIsEmpty));
			functions.Add("isnotempty", new TemplateFunction(FuncIsNotEmpty));
			functions.Add("isnumber", new TemplateFunction(FuncIsNumber));
			functions.Add("toupper", new TemplateFunction(FuncToUpper));
			functions.Add("tolower", new TemplateFunction(FuncToLower));
			functions.Add("isdefined", new TemplateFunction(FuncIsDefined));
			functions.Add("ifdefined", new TemplateFunction(FuncIfDefined));
			functions.Add("len", new TemplateFunction(FuncLen));
			functions.Add("tolist", new TemplateFunction(FuncToList));
			functions.Add("isnull", new TemplateFunction(FuncIsNull));
			functions.Add("not", new TemplateFunction(FuncNot));
			functions.Add("iif", new TemplateFunction(FuncIif));
			functions.Add("format", new TemplateFunction(FuncFormat));
			functions.Add("trim", new TemplateFunction(FuncTrim));
			functions.Add("filter", new TemplateFunction(FuncFilter));
			functions.Add("gt", new TemplateFunction(FuncGt));
			functions.Add("lt", new TemplateFunction(FuncLt));
			functions.Add("compare", new TemplateFunction(FuncCompare));
			functions.Add("or", new TemplateFunction(FuncOr));
			functions.Add("and", new TemplateFunction(FuncAnd));
			functions.Add("comparenocase", new TemplateFunction(FuncCompareNoCase));
			functions.Add("stripnewlines", new TemplateFunction(FuncStripNewLines));
		}

		#region Functions
		bool CheckArgCount(int count, string funcName, object[] args)
		{
			if (count != args.Length)
			{
				WriteError(string.Format("Function {0} requires {1} arguments and {2} were passed", funcName, count, args.Length), currentExpression.Line, currentExpression.Col);
				return false;
			}
			else
				return true;
		}

		bool CheckArgCount(int count1, int count2, string funcName, object[] args)
		{
			if (args.Length < count1 || args.Length > count2)
			{
				string msg = string.Format("Function {0} requires between {1} and {2} arguments and {3} were passed", funcName, count1, count2, args.Length);
				WriteError(msg, currentExpression.Line, currentExpression.Col);
				return false;
			}
			else
				return true;
		}

		object FuncIsEven(object[] args)
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
				WriteError("IsEven cannot convert parameter to int", currentExpression.Line, currentExpression.Col);
				return null;
			}
		}

		object FuncIsOdd(object[] args)
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
				WriteError("IsOdd cannot convert parameter to int", currentExpression.Line, currentExpression.Col);
				return null;
			}
		}

		object FuncIsEmpty(object[] args)
		{
			if (!CheckArgCount(1, "isempty", args))
				return null;

			string value = args[0].ToString();
			return value.Length == 0;
		}

		object FuncIsNotEmpty(object[] args)
		{
			if (!CheckArgCount(1, "isnotempty", args))
				return null;

			if (args[0] == null)
				return false;

			string value = args[0].ToString();
			return value.Length > 0;
		}


		object FuncEquals(object[] args)
		{
			if (!CheckArgCount(2, "equals", args))
				return null;

			return args[0].Equals(args[1]);
		}


		object FuncNotEquals(object[] args)
		{
			if (!CheckArgCount(2, "notequals", args))
				return null;

			return !args[0].Equals(args[1]);
		}


		object FuncIsNumber(object[] args)
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

		object FuncToUpper(object[] args)
		{
			if (!CheckArgCount(1, "toupper", args))
				return null;

			return args[0].ToString().ToUpper();
		}

		object FuncToLower(object[] args)
		{
			if (!CheckArgCount(1, "toupper", args))
				return null;

			return args[0].ToString().ToLower();
		}

		object FuncLen(object[] args)
		{
			if (!CheckArgCount(1, "len", args))
				return null;

			return args[0].ToString().Length;
		}


		object FuncIsDefined(object[] args)
		{
			if (!CheckArgCount(1, "isdefined", args))
				return null;

			return variables.IsDefined(args[0].ToString());
		}

		object FuncIfDefined(object[] args)
		{
			if (!CheckArgCount(2, "ifdefined", args))
				return null; 

			if (variables.IsDefined(args[0].ToString()))
			{
				return args[1];
			}
			else
				return string.Empty;
		}

		object FuncToList(object[] args)
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
				WriteError("argument 1 of tolist has to be IEnumerable", currentExpression.Line, currentExpression.Col);
				return null;
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
					sb.Append(EvalProperty(ienum.Current, property));
				}
				index++;
			}

			return sb.ToString();

		}

		object FuncIsNull(object[] args)
		{
			if (!CheckArgCount(1, "isnull", args))
				return null;

			return args[0] == null;
		}

		object FuncNot(object[] args)
		{
			if (!CheckArgCount(1, "not", args))
				return null;

			if (args[0] is bool)
				return !(bool)args[0];
			else
			{
				WriteError("Parameter 1 of function 'not' is not boolean", currentExpression.Line, currentExpression.Col);
				return null;
			}

		}

		object FuncIif(object[] args)
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
				WriteError("Parameter 1 of function 'iif' is not boolean", currentExpression.Line, currentExpression.Col);
				return null;
			}
		}

		object FuncFormat(object[] args)
		{
			if (!CheckArgCount(2, "format", args))
				return null;

			string format = args[1].ToString();

			if (args[0] is IFormattable)
				return ((IFormattable)args[0]).ToString(format, null);
			else
				return args[0].ToString();
		}

		object FuncTrim(object[] args)
		{
			if (!CheckArgCount(1, "trim", args))
				return null;

			return args[0].ToString().Trim();
		}

		object FuncFilter(object[] args)
		{
			if (!CheckArgCount(2, "filter", args))
				return null;

			object list = args[0];

			string property;
			property = args[1].ToString();

			if (!(list is IEnumerable))
			{
				WriteError("argument 1 of filter has to be IEnumerable", currentExpression.Line, currentExpression.Col);
				return null;
			}

			IEnumerator ienum = ((IEnumerable)list).GetEnumerator();
			List<object> newList = new List<object>();
			
			while (ienum.MoveNext())
			{
				object val = EvalProperty(ienum.Current, property);
				if (val is bool && (bool)val)
					newList.Add(ienum.Current);
			}

			return newList;

		}

		object FuncGt(object[] args)
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

		object FuncLt(object[] args)
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

		object FuncCompare(object[] args)
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

		object FuncOr(object[] args)
		{
			if (!CheckArgCount(2, "or", args))
				return null;

			if (args[0] is bool && args[1] is bool)
				return (bool)args[0] || (bool)args[1];
			else
				return false;
		}

		object FuncAnd(object[] args)
		{
			if (!CheckArgCount(2, "and", args))
				return null;

			if (args[0] is bool && args[1] is bool)
				return (bool)args[0] && (bool)args[1];
			else
				return false;
		}

		object FuncCompareNoCase(object[] args)
		{
			if (!CheckArgCount(2, "compareNoCase", args))
				return null;

			string s1 = args[0].ToString();
			string s2 = args[1].ToString();

			return string.Compare(s1, s2, true)==0;
		}

		object FuncStripNewLines(object[] args)
		{
			if (!CheckArgCount(1, "StripNewLines", args))
				return null;

			string s1 = args[0].ToString();
			return s1.Replace("\r\n", " ");

		}

		#endregion

		/// <summary>
		/// gets library of functions that are available
		/// for the tempalte execution
		/// </summary>
		public Dictionary<string, TemplateFunction> Functions
		{
			get { return functions; }
		}

		/// <summary>
		/// sets value for variable called name
		/// </summary>
		public void SetValue(string name, object value)
		{
			variables[name] = value;
		}

		/// <summary>
		/// gets value for variable called name.
		/// Returns null if variable is not found.
		/// </summary>
		public object GetValue(string name)
		{
			return variables[name];
		}

		/// <summary>
		/// processes current template and sends output to writer
		/// </summary>
		/// <param name="writer"></param>
		public void Process(TextWriter writer)
		{
			this.writer = writer;
			this.currentTemplate = mainTemplate;

			ProcessElements(mainTemplate.Elements);
		}

		/// <summary>
		/// processes templates and returns string value
		/// </summary>
		public string Process()
		{
			StringWriter writer = new StringWriter();
			Process(writer);
			return writer.ToString();
		}

		/// <summary>
		/// resets all variables. If TemplateManager is used to 
		/// process template multiple times, Reset() must be 
		/// called prior to Process if varialbes need to be cleared
		/// </summary>
		public void Reset()
		{
			variables.Clear();
		}

		protected void ProcessElements(List<Element> list)
		{
			foreach (Element elem in list)
			{
				ProcessElement(elem);
			}
		}

		protected void ProcessElement(Element elem)
		{
			if (elem is Text)
			{
				Text text = (Text)elem;
				WriteValue(text.Data);
			}
			else if (elem is Expression)
				ProcessExpression((Expression)elem);
			else if (elem is TagIf)
				ProcessIf((TagIf)elem);
			else if (elem is Tag)
				ProcessTag((Tag)elem);
		}

		protected void ProcessExpression(Expression exp)
		{
			object value = EvalExpression(exp);
			WriteValue(value);
		}

		protected object EvalExpression(Expression exp)
		{
			currentExpression = exp;

			if (exp is StringLiteral)
				return ((StringLiteral)exp).Content;
			else if (exp is Name)
			{
				object obj = variables[((Name)exp).Id];
				return obj;
			}
			else if (exp is FieldAccess)
			{
				FieldAccess fa = (FieldAccess)exp;
				object obj = variables[fa.Exp];
				string propertyName = fa.Field;
				return EvalProperty(obj, propertyName);
			}
			else if (exp is IntLiteral)
				return ((IntLiteral)exp).Value;
			else if (exp is FCall)
			{
				FCall fcall = (FCall)exp;
				if (!functions.ContainsKey(fcall.Name))
				{
					string msg = string.Format("Function {0} is not defined", fcall.Name);
					WriteError(msg, exp.Line, exp.Col);
					return null;
				}

				TemplateFunction func = functions[fcall.Name];
				object[] values = new object[fcall.Args.Length];
				for (int i = 0; i < values.Length; i++)
					values[i] = EvalExpression(fcall.Args[i]);

				return func(values);
			}
			else if (exp is StringExpression)
			{
				StringExpression stringExp = (StringExpression)exp;
				StringBuilder sb = new StringBuilder();
				foreach (Expression ex in stringExp.Expressions)
					sb.Append(EvalExpression(ex));

				return sb.ToString();
			}

			return null;
		}

		protected static object EvalProperty(object obj, string propertyName)
		{
			try
			{
				PropertyInfo finfo = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance);
				if (finfo != null)
				{
					object value = finfo.GetValue(obj, null);
					return value;
				}
				else
					return null;
			}
			catch 
			{
				return null;
			}
		}

		protected void ProcessIf(TagIf tagIf)
		{
			object value = EvalExpression(tagIf.Test);

			if (Util.ToBool(value))
				ProcessElements(tagIf.InnerElements);
			else
				ProcessElement(tagIf.FalseBranch);

		}

		protected void ProcessTag(Tag tag)
		{
			string name = tag.Name.ToLowerInvariant();
			switch (name)
			{
				case "template":
					// skip those, because those are processed first
					break;
				case "else":
					ProcessElements(tag.InnerElements);
					break;
				case "apply":
					object val = EvalExpression(tag.AttributeValue("template"));
					ProcessTemplate(val.ToString(), tag);
					break;
				case "foreach":
					ProcessForEach(tag);
					break;
				default:
					ProcessTemplate(tag.Name, tag);
					break;
			}
		}

		protected void ProcessForEach(Tag tag)
		{
			Expression expCollection = tag.AttributeValue("collection");
			if (expCollection == null)
			{
				WriteError("Foreach is missing required attribute: collection", tag.Line, tag.Col);
				return;
			}

			object collection = EvalExpression(expCollection);
			if (!(collection is IEnumerable))
			{
				WriteError("Collection used in foreach has to be enumerable", tag.Line, tag.Col);
				return;
			}

			Expression expVar = tag.AttributeValue("var");
			if (expCollection == null)
			{
				WriteError("Foreach is missing required attribute: var", tag.Line, tag.Col);
				return;
			}
			object varObject = EvalExpression(expVar);
			if (varObject == null)
				varObject = "foreach";
			string varname = varObject.ToString();

			Expression expIndex = tag.AttributeValue("index");
			string indexname = null;
			if (expIndex != null)
			{
				object obj = EvalExpression(expIndex);
				if (obj != null)
					indexname = obj.ToString();
			}

			IEnumerator ienum = ((IEnumerable)collection).GetEnumerator();
			int index = 0;
			while (ienum.MoveNext())
			{
				index++;
				object value = ienum.Current;
				variables[varname] = value;
				if (indexname != null)
					variables[indexname] = index;

				ProcessElements(tag.InnerElements);
			}
		}


		protected void ProcessTemplate(string name, Tag tag)
		{
			Template useTemplate = currentTemplate.FindTemplate(name);
			if (useTemplate == null)
			{
				string msg = string.Format("Template '{0}' not found", name);
				WriteError(msg, tag.Line, tag.Col);
				return;
			}

			// process inner elements and save content
			TextWriter saveWriter = writer;
			writer = new StringWriter();
			string content = string.Empty;

			try
			{
				ProcessElements(tag.InnerElements);

				content = writer.ToString();
			}
			catch
			{
				return; // on error, do not do tag inclusion
			}
			finally
			{
				writer = saveWriter;
			}

			Template saveTemplate = currentTemplate;
			variables = new VariableScope(variables);
			variables["innerText"] = content;

			try
			{
				foreach (TagAttribute attrib in tag.Attributes)
				{
					object val = EvalExpression(attrib.Expression);
					variables[attrib.Name] = val;
				}

				currentTemplate = useTemplate;
				ProcessElements(currentTemplate.Elements);
			}
			finally
			{
				variables = variables.Parent;
				currentTemplate = saveTemplate;
			}

			
		}

		private void WriteValue(object value)
		{
			if (value == null)
				writer.Write("[null]");
			else
				writer.Write(value);
		}

		private void WriteError(string msg, int line, int col)
		{
			writer.Write("[ERROR ({0}, {1}): {2}]", line, col, msg);
		}
	}
}
