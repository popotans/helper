using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.TemplateEngine.Parser
{
	/// <summary>
	/// Token 类型的枚举。
	/// </summary>
	public enum TokenKind
	{
		/// <summary>
		/// 已经到达文档末尾。
		/// </summary>
		EOF,

		/// <summary>
		/// 注释。
		/// </summary>
		Comment,
		
		/// <summary>
		/// 唯一标识符。
		/// </summary>
		ID,



		/// <summary>
		/// 文本内容。
		/// </summary>
		TextData,



		/// <summary>
		/// 开始标签。
		/// </summary>
		TagStart,		// <ad: 

		/// <summary>
		/// 开始标签的闭合标签。
		/// </summary>
		TagEnd,			// > 

		/// <summary>
		/// 自闭合标签。
		/// </summary>
		TagEndClose,	// />

		/// <summary>
		/// 结束标签。
		/// </summary>
		TagClose,		// </ad:

		/// <summary>
		/// 属性赋值符号。
		/// </summary>
		TagEquals,		// =


		// expression

		/// <summary>
		/// 表达式开始标记。
		/// </summary>
		ExpStart,		// #, {$ at the beginning

		/// <summary>
		/// 表达式结束标记。
		/// </summary>
		ExpEnd,			// #, } at the end

		/// <summary>
		/// 表达式参数列表的开始标记。
		/// </summary>
		LParen,			// (

		/// <summary>
		/// 表达式参数列表的结束标记。
		/// </summary>
		RParen,			// )

		/// <summary>
		/// 表达式对象访问符。
		/// </summary>
		Dot,			// .

		/// <summary>
		/// 参数分隔符。
		/// </summary>
		Comma,			// ,

		/// <summary>
		/// 整形数字。
		/// </summary>
		Integer,		// integer number

		/// <summary>
		/// 双精度数字。
		/// </summary>
		Double,			// double number

		/// <summary>
		/// 索引或指针访问器的开始标记。
		/// </summary>
		LBracket,		// [

		/// <summary>
		/// 索引或指针访问器的结束标记。
		/// </summary>
		RBracket,		// ]

		// operators

		/// <summary>
		/// or 运算符。
		/// </summary>
		OpOr,           // "or" keyword

		/// <summary>
		/// and 运算符。
		/// </summary>
		OpAnd,          // "and" keyword

		/// <summary>
		/// is 运算符。
		/// </summary>
		OpIs,			// "is" keyword

		/// <summary>
		/// isnot 运算符。
		/// </summary>
		OpIsNot,		// "isnot" keyword

		/// <summary>
		/// lt（小于） 运算符。
		/// </summary>
		OpLt,			// "lt" keyword

		/// <summary>
		/// gt（大于） 运算符。
		/// </summary>
		OpGt,			// "gt" keyword

		/// <summary>
		/// lte（小于等于） 运算符。
		/// </summary>
		OpLte,			// "lte" keyword

		/// <summary>
		/// gte（大于等于） 运算符。
		/// </summary>
		OpGte,			// "gte" keyword


		/// <summary>
		/// 字符串开始标记。
		/// </summary>
		StringStart,	// "

		/// <summary>
		/// 字符串结束标记。
		/// </summary>
		StringEnd,		// "

		/// <summary>
		/// 字符串文本。
		/// </summary>
		StringText		// text within the string
	}
}
