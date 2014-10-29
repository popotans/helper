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
using System.IO;

namespace VMoom.TemplateEngine.Parser
{
	/// <summary>
	/// 模板词汇分析器。
	/// </summary>
	public class TemplateLexer
	{
		#region 私有字段

		private const char _EOF = (char)0;

		private LexMode _CurrentMode;
		private Stack<LexMode> _Modes;

		private int _Line;
		private int _Column;
		private int _Pos;	// position within data

		private string _Data;

		private int _SaveLine;
		private int _SaveCol;
		private int _SavePos;

		private static Dictionary<string, TokenKind> keywords;

		#endregion

		#region 构造函数

		/// <summary>
		/// 静态构造函数。初始化关键字库。
		/// </summary>
		static TemplateLexer()
		{
			keywords = new Dictionary<string, TokenKind>(StringComparer.InvariantCultureIgnoreCase);

			keywords["or"] = TokenKind.OpOr;
			keywords["and"] = TokenKind.OpAnd;
			keywords["is"] = TokenKind.OpIs;
			keywords["isnot"] = TokenKind.OpIsNot;
			keywords["lt"] = TokenKind.OpLt;
			keywords["gt"] = TokenKind.OpGt;
			keywords["lte"] = TokenKind.OpLte;
			keywords["gte"] = TokenKind.OpGte;
		}

		/// <summary>
		/// 初始化模板词汇解析器。
		/// </summary>
		/// <param name="reader">包含模板内容的 TextReader。</param>
		public TemplateLexer(TextReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			_Data = reader.ReadToEnd();

			Reset();
		}

		/// <summary>
		/// 初始化模板词汇解析器。
		/// </summary>
		/// <param name="data">要分析的模板内容。</param>
		public TemplateLexer(string data)
		{
			if (data == null) throw new ArgumentNullException("data");

			_Data = data;

			Reset();
		}

		#endregion

		#region 私有方法

		private void EnterMode(LexMode mode)
		{
			_Modes.Push(_CurrentMode);
			_CurrentMode = mode;
		}

		private void LeaveMode()
		{
			_CurrentMode = _Modes.Pop();
		}

		private void Reset()
		{
			_Modes = new Stack<LexMode>();
			_CurrentMode = LexMode.Text;
			_Modes.Push(_CurrentMode);

			_Line = 1;
			_Column = 1;
			_Pos = 0;
		}

		private void NewLine()
		{
			_Line++;
			_Column = 1;
		}

		private void StartRead()
		{
			_SaveLine = _Line;
			_SaveCol = _Column;
			_SavePos = _Pos;
		}

		private Token NextExpression()
		{
			StartRead();
			char ch = LA(0);
			switch (ch)
			{
				case _EOF:
					return CreateToken(TokenKind.EOF);
				case ',':
					Consume();
					return CreateToken(TokenKind.Comma);
				case '.':
					Consume();
					return CreateToken(TokenKind.Dot);
				case '(':
					Consume();
					return CreateToken(TokenKind.LParen);
				case ')':
					Consume();
					return CreateToken(TokenKind.RParen);
				//case '#':
				//    Consume();
				//    LeaveMode();
				//    return CreateToken(TokenKind.ExpEnd);
				case '}':
					Consume();
					LeaveMode();
					return CreateToken(TokenKind.ExpEnd);
				case '[':
					Consume();
					return CreateToken(TokenKind.LBracket);
				case ']':
					Consume();
					return CreateToken(TokenKind.RBracket);
				case ' ':
				case '\t':
				case '\r':
				case '\n':
					ReadWhitespace();
					return NextExpression();

				case '"':
					Consume();
					EnterMode(LexMode.String);
					return CreateToken(TokenKind.StringStart);

				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return ReadNumber();

				case '-':
					{
						if (Char.IsDigit(LA(1)))
							return ReadNumber();

						goto default;
					}

				default:
					if (Char.IsLetter(ch) || ch == '_')
						return ReadId();
					else
						throw new ParseException("Invalid character in expression: " + ch, _Line, _Column);
			}
		}

		private Token NextTag()
		{
			StartRead();
		StartTagRead:
			char ch = LA(0);
			switch (ch)
			{
				case _EOF:
					return CreateToken(TokenKind.EOF);
				case '=':
					Consume();
					return CreateToken(TokenKind.TagEquals);
				case '"':
					Consume();
					EnterMode(LexMode.String);
					return CreateToken(TokenKind.StringStart);
				case ' ':
				case '\t':
				case '\r':
				case '\n':
					ReadWhitespace();	// ignore whitespace
					StartRead();		// remark current position
					goto StartTagRead;	// start again
				case '>':
					Consume();
					LeaveMode();
					return CreateToken(TokenKind.TagEnd);
				case '/':
					if (LA(1) == '>')
					{
						Consume(2); // consume />
						LeaveMode();
						return CreateToken(TokenKind.TagEndClose);
					}
					break;
				default:
					if (Char.IsLetter(ch) || ch == '_')
						return ReadId();
					break;

			}
			throw new ParseException("Invalid character in tag: " + ch, _Line, _Column);
		}

		private Token NextString()
		{
			StartRead();
		StartStringRead:
			char ch = LA(0);
			switch (ch)
			{
				case _EOF:
					return CreateToken(TokenKind.EOF);

				//case '#':
				//    if (LA(1) == '#') // just escape
				//    {
				//        Consume(2);
				//        goto StartStringRead;
				//    }
				//    else if (savePos == pos)
				//    {
				//        Consume();
				//        EnterMode(LexMode.Expression);
				//        return CreateToken(TokenKind.ExpStart);
				//    }
				//    else
				//        break; // just break and we will return the text token

				case '{': // 实现 {$.....}
					char ch1 = LA(1);

					if (ch1 == '$')
					{
						if (_SavePos == _Pos)
						{
							Consume(2); // 销毁：{$
							EnterMode(LexMode.Expression);
							return CreateToken(TokenKind.ExpStart);
						}
						else
							break;
					}

					Consume();
					goto StartStringRead;

				case '\r':
				case '\n':
					ReadWhitespace();
					goto StartStringRead;
				case '"':
					if (LA(1) == '"')
					{
						// just escape
						Consume(2);
						goto StartStringRead;
					}
					else if (_Pos == _SavePos)
					{
						Consume();
						LeaveMode();
						return CreateToken(TokenKind.StringEnd);
					}
					else
						break; // just break so that text is returned
				default:
					Consume();
					goto StartStringRead;

			}

			return CreateToken(TokenKind.StringText);
		}

		private Token NextText()
		{
			StartRead();

		StartTextRead:
			switch (LA(0))
			{
				case _EOF:
					if (_SavePos == _Pos)
						return CreateToken(TokenKind.EOF);
					else
						break;

				//case '#':
				//    if (LA(1) == '#') // # was just escape
				//    {
				//        Consume(2); // consume both #
				//        goto StartTextRead;
				//    }
				//    else if (savePos == pos)
				//    {
				//        Consume();
				//        EnterMode(LexMode.Expression);
				//        return CreateToken(TokenKind.ExpStart);
				//    }
				//    else
				//        break; // even if we have exp, we break because we read some characters that need to be returned as text

				case '{': // 实现 {$.....}
					// 123456{{$VMoom.length}}7890
					char ch1 = LA(1);
					string s = String.Format("{0},{1}", _SavePos, _Pos);

					if (ch1 == '$')
					{
						if (_SavePos == _Pos)
						{
							Consume(2); // 销毁：{$
							EnterMode(LexMode.Expression);
							return CreateToken(TokenKind.ExpStart);
						}
						else
							break;
					}

					Consume();
					goto StartTextRead;

				case '<':
					// <ad:
					if (LA(1) == 'a' && LA(2) == 'd' && LA(3) == ':')
					{
						if (_SavePos == _Pos)
						{
							Consume(4); // consume <ad:
							EnterMode(LexMode.Tag);
							return CreateToken(TokenKind.TagStart);
						}
						else
							break;
					}
					else if (LA(1) == '/' && LA(2) == 'a' && LA(3) == 'd' && LA(4) == ':')
					{
						if (_SavePos == _Pos)
						{
							Consume(5); // consume </ad:
							EnterMode(LexMode.Tag);
							return CreateToken(TokenKind.TagClose);
						}
						else
							break;
					}

					Consume();
					goto StartTextRead;
				case '\n':
				case '\r':
					ReadWhitespace();	// handle newlines specially so that line number count is kept
					goto StartTextRead;

				default:
					Consume();
					goto StartTextRead;
			}

			return CreateToken(TokenKind.TextData);
		}

		#endregion

		#region 受保护方法

		/// <summary>
		/// 从上一个被分析的词汇信息中获取指定位置的字符。
		/// </summary>
		/// <param name="count">要获取的字符的位置。</param>
		/// <returns>char</returns>
		protected char LA(int count)
		{
			if (_Pos + count >= _Data.Length)
				return _EOF;
			else
				return _Data[_Pos + count];
		}

		/// <summary>
		/// 销毁上一个词汇信息的末尾的第一个字符，并将分析指针移动到下一条词汇前端。
		/// </summary>
		/// <returns>char</returns>
		protected char Consume()
		{
			char ret = _Data[_Pos];
			_Pos++;
			_Column++;

			return ret;
		}

		/// <summary>
		/// 销毁上一个词汇信息的末尾的指定数量的字符，并将分析指针移动到下一条词汇前端。
		/// </summary>
		/// <param name="count">要销毁的字符的数量。</param>
		/// <returns>char</returns>
		protected char Consume(int count)
		{
			if (count <= 0) throw new ArgumentOutOfRangeException("count", "count has to be greater than 0");

			char ret = ' ';
			while (count > 0)
			{
				ret = Consume();
				count--;
			}

			return ret;
		}

		/// <summary>
		/// 创建 Token 实例。
		/// </summary>
		/// <param name="kind">指定 Token 类型。</param>
		/// <param name="value">指定 Token 的内容。</param>
		/// <returns>Token</returns>
		protected Token CreateToken(TokenKind kind, string value)
		{
			return new Token(kind, value, _Line, _Column);
		}

		/// <summary>
		/// 创建 Token 实例。
		/// </summary>
		/// <param name="kind">指定 Token 类型。</param>
		/// <returns>Token</returns>
		protected Token CreateToken(TokenKind kind)
		{
			string tokenData = _Data.Substring(_SavePos, _Pos - _SavePos);
			if (kind == TokenKind.StringText)
				tokenData = tokenData.Replace("\"\"", "\""); // replace double "" with single "
			//if (kind == TokenKind.StringText || kind == TokenKind.TextData)
			//    tokenData = tokenData.Replace("##", "#");	// replace ## with #

			return new Token(kind, tokenData, _SaveLine, _SaveCol);
		}

		/// <summary>
		/// 读取并分析所有连续的空白字符。
		/// </summary>
		protected void ReadWhitespace()
		{
			while (true)
			{
				char ch = LA(0);
				switch (ch)
				{
					case ' ':
					case '\t':
						Consume();
						break;
					case '\n':
						Consume();
						NewLine();
						break;

					case '\r':
						Consume();
						if (LA(0) == '\n')
							Consume();
						NewLine();
						break;
					default:
						return;
				}
			}
		}

		/// <summary>
		/// 读取标识符。
		/// </summary>
		/// <returns>Token</returns>
		protected Token ReadId()
		{
			StartRead();

			Consume(); // consume first character of the word

			while (true)
			{
				char ch = LA(0);
				if (Char.IsLetterOrDigit(ch) || ch == '_')
					Consume();
				else
					break;
			}

			string tokenData = _Data.Substring(_SavePos, _Pos - _SavePos);

			if (keywords.ContainsKey(tokenData))
				return CreateToken(keywords[tokenData]);
			else
				return CreateToken(TokenKind.ID, tokenData);
		}

		/// <summary>
		/// 读取并分析所有的数字型字符。
		/// </summary>
		/// <returns>Token</returns>
		protected Token ReadNumber()
		{
			StartRead();
			Consume(); // consume first digit or -

			bool hasDot = false;

			while (true)
			{
				char ch = LA(0);
				if (Char.IsNumber(ch))
					Consume();

				// if "." and didn't see "." yet, and next char
				// is number, than starting to read decimal number
				else if (ch == '.' && !hasDot && Char.IsNumber(LA(1)))
				{
					Consume();
					hasDot = true;
				}
				else
					break;
			}

			return CreateToken(hasDot ? TokenKind.Double : TokenKind.Integer);
		}

		#endregion

		#region 公有方法

		/// <summary>
		/// 继续分析下一条词汇，并返回分析结果。
		/// </summary>
		/// <returns>Token</returns>
		public Token Next()
		{
			switch (_CurrentMode)
			{
				case LexMode.Text: return NextText();
				case LexMode.Expression: return NextExpression();
				case LexMode.Tag: return NextTag();
				case LexMode.String: return NextString();
				default: throw new ParseException("Encountered invalid lexer mode: " + _CurrentMode.ToString(), _Line, _Column);
			}
		}

		#endregion

		#region 内嵌枚举

		/// <summary>
		/// 包含词位分析模式的枚举。
		/// </summary>
		private enum LexMode
		{
			/// <summary>
			/// 文本模式。
			/// </summary>
			Text,
			/// <summary>
			/// 标记模式。
			/// </summary>
			Tag,
			/// <summary>
			/// 表达式模式。
			/// </summary>
			Expression,
			/// <summary>
			/// 字符串模式。
			/// </summary>
			String
		}

		#endregion
	}
}
