using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Globalization;
using System.Reflection.Emit;
using System.IO;

namespace Helper.Serialize
{
    public class JSON
    {
        public readonly static JSON Instance = new JSON();
        private JSON()
        {

        }

        public string ToJSON(object obj)
        {
            return new JSONSerializer().ConvertToJSON(obj);
        }

        public object ToObject(string json)
        {
            Dictionary<string, object> ht = (Dictionary<string, object>)JsonParser.JsonDecode(json);
            if (ht == null)
                return null;

            return ht;
            //return ParseDictionary(ht);
        }

        #region [   PROPERTY GET SET CACHE   ]
        SafeDictionary<string, Type> _typecache = new SafeDictionary<string, Type>();
        private Type GetTypeFromCache(string typename)
        {
            if (_typecache.ContainsKey(typename))
                return _typecache[typename];
            else
            {
                Type t = Type.GetType(typename);
                _typecache.Add(typename, t);
                return t;
            }
        }

        SafeDictionary<string, PropertyInfo> _propertycache = new SafeDictionary<string, PropertyInfo>();
        private PropertyInfo getproperty(Type type, string propertyname)
        {
            if (propertyname == "$type")
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append(type.Name);
            sb.Append("|");
            sb.Append(propertyname);
            string n = sb.ToString();

            if (_propertycache.ContainsKey(n))
            {
                return _propertycache[n];
            }
            else
            {
                PropertyInfo[] pr = type.GetProperties();
                foreach (PropertyInfo p in pr)
                {
                    StringBuilder sbb = new StringBuilder();
                    sbb.Append(type.Name);
                    sbb.Append("|");
                    sbb.Append(p.Name);
                    string nn = sbb.ToString();
                    if (_propertycache.ContainsKey(nn) == false)
                        _propertycache.Add(nn, p);
                }
                return _propertycache[n];
            }
        }

        private delegate void GenericSetter(object target, object value);

        private static GenericSetter CreateSetMethod(PropertyInfo propertyInfo)
        {
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod == null)
                return null;

            Type[] arguments = new Type[2];
            arguments[0] = arguments[1] = typeof(object);

            DynamicMethod setter = new DynamicMethod(
                String.Concat("_Set", propertyInfo.Name, "_"),
                typeof(void), arguments, propertyInfo.DeclaringType);
            ILGenerator il = setter.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            il.Emit(OpCodes.Ldarg_1);

            if (propertyInfo.PropertyType.IsClass)
                il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
            else
                il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);

            il.EmitCall(OpCodes.Callvirt, setMethod, null);
            il.Emit(OpCodes.Ret);

            return (GenericSetter)setter.CreateDelegate(typeof(GenericSetter));
        }

        public delegate object GenericGetter(object target);

        private static GenericGetter CreateGetMethod(PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            if (getMethod == null)
                return null;

            Type[] arguments = new Type[1];
            arguments[0] = typeof(object);

            DynamicMethod getter = new DynamicMethod(
                String.Concat("_Get", propertyInfo.Name, "_"),
                typeof(object), arguments, propertyInfo.DeclaringType);
            ILGenerator il = getter.GetILGenerator();
            il.DeclareLocal(typeof(object));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            il.EmitCall(OpCodes.Callvirt, getMethod, null);

            if (!propertyInfo.PropertyType.IsClass)
                il.Emit(OpCodes.Box, propertyInfo.PropertyType);

            il.Emit(OpCodes.Ret);

            return (GenericGetter)getter.CreateDelegate(typeof(GenericGetter));
        }

        SafeDictionary<Type, List<Getters>> _getterscache = new SafeDictionary<Type, List<Getters>>();
        internal List<Getters> GetGetters(Type type)
        {
            if (_getterscache.ContainsKey(type))
                return _getterscache[type];
            else
            {
                PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                List<Getters> getters = new List<Getters>();
                foreach (PropertyInfo p in props)
                {
                    GenericGetter g = CreateGetMethod(p);
                    if (g != null)
                    {
                        Getters gg = new Getters();
                        gg.Name = p.Name;
                        gg.Getter = g;
                        getters.Add(gg);
                    }

                }
                _getterscache.Add(type, getters);
                return getters;
            }
        }

        SafeDictionary<PropertyInfo, GenericSetter> _settercache = new SafeDictionary<PropertyInfo, GenericSetter>();
        private GenericSetter GetSetter(PropertyInfo prop)
        {
            if (_settercache.ContainsKey(prop))
                return _settercache[prop];
            else
            {
                GenericSetter s = CreateSetMethod(prop);
                _settercache.Add(prop, s);
                return s;
            }
        }
        #endregion

        private object ParseDictionary(Dictionary<string, object> d)
        {
            string tn = "" + d["$type"];
            Type type = GetTypeFromCache(tn);
            object o = Activator.CreateInstance(type);
            foreach (string name in d.Keys)
            {
                PropertyInfo pi = getproperty(type, name);
                if (pi != null)
                {
                    object v = d[name];

                    if (v != null)
                    {
                        object oset = null;
                        GenericSetter setter;
                        Type pt = pi.PropertyType;
                        object dic = pt.GetInterface("IDictionary");
                        if (pt.IsGenericType && pt.IsValueType == false && dic == null)
                        {
                            IList col = (IList)Activator.CreateInstance(pt);
                            // create an array of objects
                            foreach (object ob in (ArrayList)v)
                                col.Add(ParseDictionary((Dictionary<string, object>)ob));

                            oset = col;
                        }
                        else if (pt == typeof(byte[]))
                        {
                            oset = Convert.FromBase64String((string)v);
                        }
                        else if (pt.IsArray && pt.IsValueType == false)
                        {
                            ArrayList col = new ArrayList();
                            // create an array of objects
                            foreach (object ob in (ArrayList)v)
                                col.Add(ParseDictionary((Dictionary<string, object>)ob));

                            oset = col.ToArray(pi.PropertyType.GetElementType());
                        }
                        else if (pt == typeof(Guid) || pt == typeof(Guid?))
                            oset = new Guid("" + v);

                        else if (pt == typeof(DataSet))
                            oset = CreateDataset((Dictionary<string, object>)v);

                        else if (pt == typeof(Hashtable))
                            oset = CreateDictionary((ArrayList)v, pt);

                        else if (dic != null)
                            oset = CreateDictionary((ArrayList)v, pt);

                        else
                            oset = ChangeType(v, pt);

                        setter = GetSetter(pi);
                        setter(o, oset);
                    }
                }
            }
            return o;
        }

        private object CreateDictionary(ArrayList reader, Type pt)
        {
            IDictionary col = (IDictionary)Activator.CreateInstance(pt);
            Type[] types = col.GetType().GetGenericArguments();

            foreach (object o in reader)
            {
                Dictionary<string, object> values = (Dictionary<string, object>)o;

                object key;
                object val;


                if (values["k"] is Dictionary<string, object>)
                    key = ParseDictionary((Dictionary<string, object>)values["k"]);
                else
                    key = ChangeType(values["k"], types[0]);

                if (values["v"] is Dictionary<string, object>)
                    val = ParseDictionary((Dictionary<string, object>)values["v"]);
                else
                    val = ChangeType(values["v"], types[1]);

                col.Add(key, val);

            }

            return col;
        }

        public object ChangeType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {

                System.ComponentModel.NullableConverter nullableConverter
                    = new System.ComponentModel.NullableConverter(conversionType);

                conversionType = nullableConverter.UnderlyingType;
            }

            return Convert.ChangeType(value, conversionType);
        }

        private Hashtable CreateHashtable(ArrayList reader)
        {
            Hashtable ht = new Hashtable();

            foreach (object o in reader)
            {
                Dictionary<string, object> values = (Dictionary<string, object>)o;

                ht.Add(
                    ParseDictionary((Dictionary<string, object>)values["k"]),
                    ParseDictionary((Dictionary<string, object>)values["v"])
                );
            }
            return ht;
        }

        private DataSet CreateDataset(Dictionary<string, object> reader)
        {
            DataSet ds = new DataSet();

            // read dataset schema here
            string s = "" + reader["$schema"];
            TextReader tr = new StringReader(s);
            ds.ReadXmlSchema(tr);

            foreach (string key in reader.Keys)
            {
                if (key == "$type" || key == "$schema")
                    continue;
                object tb = reader[key];
                if (tb != null && tb.GetType() == typeof(ArrayList))
                {
                    ArrayList rows = (ArrayList)tb;
                    foreach (Dictionary<string, object> row in rows)
                    {
                        DataRow dr = ds.Tables[key].NewRow();
                        foreach (string col in row.Keys)
                        {
                            dr[col] = row[col];
                        }

                        ds.Tables[key].Rows.Add(dr);
                    }
                }
            }

            return ds;
        }
    }




    internal class JSONSerializer
    {
        private readonly StringBuilder _output = new StringBuilder();

        public static string ToJSON(object obj)
        {
            return new JSONSerializer().ConvertToJSON(obj);
        }

        internal string ConvertToJSON(object obj)
        {
            WriteValue(obj);

            return _output.ToString();
        }

        private void WriteValue(object obj)
        {
            if (obj == null)
                _output.Append("null");

            else if (obj is sbyte ||
                     obj is byte ||
                     obj is short ||
                     obj is ushort ||
                     obj is int ||
                     obj is uint ||
                     obj is long ||
                     obj is ulong ||
                     obj is decimal ||
                     obj is double ||
                     obj is float)
                _output.Append(Convert.ToString(obj, NumberFormatInfo.InvariantInfo));

            else if (obj is bool)
                _output.Append(obj.ToString().ToLower()); // conform to standard

            else if (obj is char || obj is Enum || obj is Guid || obj is string)
                WriteString(obj.ToString());

            else if (obj is DateTime)
            {
                _output.Append("\"");
                _output.Append(((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss"));// conform to standard
                _output.Append("\"");
            }

            else if (obj is DataSet)
                WriteDataset((DataSet)obj);

            else if (obj is byte[])
                WriteByteArray((byte[])obj);

            else if (obj is IDictionary)
                WriteDictionary((IDictionary)obj);

            else if (obj is Array || obj is IList || obj is ICollection)
                WriteArray((IEnumerable)obj);

            else
                WriteObject(obj);

        }

        private void WriteByteArray(byte[] bytes)
        {
            WriteString(Convert.ToBase64String(bytes));
        }

        //private void WriteHashTable(Hashtable hash)
        //{
        //    _output.Append("{");

        //    bool pendingSeparator = false;

        //    foreach (object entry in hash.Keys)
        //    {
        //        if (pendingSeparator)
        //            _output.Append(",");

        //        WriteValue(entry);
        //        _output.Append(":");
        //        WriteValue(hash[entry]);

        //        pendingSeparator = true;
        //    }

        //    _output.Append("}");
        //}

        private void WriteDataset(DataSet ds)
        {
            _output.Append("{");
            WritePair("$schema", ds.GetXmlSchema());
            _output.Append(",");

            foreach (DataTable table in ds.Tables)
            {
                _output.Append("\"");
                _output.Append(table.TableName);
                _output.Append("\":[");

                foreach (DataRow row in table.Rows)
                {
                    _output.Append("{");
                    foreach (DataColumn column in row.Table.Columns)
                    {
                        WritePair(column.ColumnName, row[column]);
                    }
                    _output.Append("}");
                }

                _output.Append("]");
            }
            // end dataset
            _output.Append("}");
        }

        private void WriteObject(object obj)
        {
            _output.Append("{");
            Type t = obj.GetType();
            WritePair("$type", t.AssemblyQualifiedName);
            _output.Append(",");

            List<Getters> g = JSON.Instance.GetGetters(t);
            foreach (Getters p in g)
            {
                WritePair(p.Name, p.Getter(obj));
            }
            _output.Append("}");
        }

        private void WritePair(string name, string value)
        {
            WriteString(name);

            _output.Append(":");

            WriteString(value);
        }

        private void WritePair(string name, object value)
        {
            WriteString(name);

            _output.Append(":");

            WriteValue(value);
        }

        private void WriteArray(IEnumerable array)
        {
            _output.Append("[");

            bool pendingSeperator = false;

            foreach (object obj in array)
            {
                if (pendingSeperator)
                    _output.Append(',');

                WriteValue(obj);

                pendingSeperator = true;
            }

            _output.Append("]");
        }

        private void WriteDictionary(IDictionary dic)
        {
            _output.Append("[");

            bool pendingSeparator = false;

            foreach (DictionaryEntry entry in dic)
            {
                if (pendingSeparator)
                    _output.Append(",");

                _output.Append("{");
                WritePair("k", entry.Key);
                _output.Append(",");
                WritePair("v", entry.Value);
                _output.Append("}");


                pendingSeparator = true;
            }

            _output.Append("]");
        }

        private void WriteString(string s)
        {
            _output.Append('\"');

            foreach (char c in s)
            {
                switch (c)
                {
                    case '\t': _output.Append("\\t"); break;
                    case '\r': _output.Append("\\r"); break;
                    case '\n': _output.Append("\\n"); break;
                    case '"':
                    case '\\': _output.Append("\\"); _output.Append(c); break;
                    default:
                        {
                            if (c >= ' ' && c < 128)
                                _output.Append(c);
                            else
                            {
                                _output.Append("\\u");
                                _output.Append(((int)c).ToString("X4"));
                            }
                        }
                        break;
                }
            }

            _output.Append('\"');
        }
    }


    internal class Getters
    {
        public string Name;
        public JSON.GenericGetter Getter;
    }

    /// <summary>
    /// This class encodes and decodes JSON strings.
    /// Spec. details, see http://www.json.org/
    /// 
    /// JSON uses Arrays and Objects. These correspond here to the datatypes ArrayList and Hashtable.
    /// All numbers are parsed to doubles.
    /// </summary>
    internal class JsonParser
    {
        private const int TOKEN_NONE = 0;
        private const int TOKEN_CURLY_OPEN = 1;
        private const int TOKEN_CURLY_CLOSE = 2;
        private const int TOKEN_SQUARED_OPEN = 3;
        private const int TOKEN_SQUARED_CLOSE = 4;
        private const int TOKEN_COLON = 5;
        private const int TOKEN_COMMA = 6;
        private const int TOKEN_STRING = 7;
        private const int TOKEN_NUMBER = 8;
        private const int TOKEN_TRUE = 9;
        private const int TOKEN_FALSE = 10;
        private const int TOKEN_NULL = 11;

        /// <summary>
        /// Parses the string json into a value
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An ArrayList, a dictionary, a double, a string, null, true, or false</returns>
        internal static object JsonDecode(string json)
        {
            bool success = true;

            return JsonDecode(json, ref success);
        }

        /// <summary>
        /// Parses the string json into a value; and fills 'success' with the successfullness of the parse.
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <param name="success">Successful parse?</param>
        /// <returns>An ArrayList, a Hashtable, a double, a string, null, true, or false</returns>
        private static object JsonDecode(string json, ref bool success)
        {
            success = true;
            if (json != null)
            {
                char[] charArray = json.ToCharArray();
                int index = 0;
                object value = ParseValue(charArray, ref index, ref success);
                return value;
            }
            else
            {
                return null;
            }
        }


        protected static Dictionary<string, object> ParseObject(char[] json, ref int index, ref bool success)
        {
            Dictionary<string, object> table = new Dictionary<string, object>();
            int token;

            // {
            NextToken(json, ref index);

            bool done = false;
            while (!done)
            {
                token = LookAhead(json, index);
                if (token == TOKEN_NONE)
                {
                    success = false;
                    return null;
                }
                else if (token == TOKEN_COMMA)
                {
                    NextToken(json, ref index);
                }
                else if (token == TOKEN_CURLY_CLOSE)
                {
                    NextToken(json, ref index);
                    return table;
                }
                else
                {

                    // name
                    string name = ParseString(json, ref index, ref success);
                    if (!success)
                    {
                        success = false;
                        return null;
                    }

                    // :
                    token = NextToken(json, ref index);
                    if (token != TOKEN_COLON)
                    {
                        success = false;
                        return null;
                    }

                    // value
                    object value = ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        success = false;
                        return null;
                    }

                    table[name] = value;
                }
            }

            return table;
        }

        protected static ArrayList ParseArray(char[] json, ref int index, ref bool success)
        {
            ArrayList array = new ArrayList();

            NextToken(json, ref index);

            bool done = false;
            while (!done)
            {
                int token = LookAhead(json, index);
                if (token == TOKEN_NONE)
                {
                    success = false;
                    return null;
                }
                else if (token == TOKEN_COMMA)
                {
                    NextToken(json, ref index);
                }
                else if (token == TOKEN_SQUARED_CLOSE)
                {
                    NextToken(json, ref index);
                    break;
                }
                else
                {
                    object value = ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        return null;
                    }

                    array.Add(value);
                }
            }

            return array;
        }

        protected static object ParseValue(char[] json, ref int index, ref bool success)
        {
            switch (LookAhead(json, index))
            {
                case TOKEN_NUMBER:
                    return ParseNumber(json, ref index, ref success);
                case TOKEN_STRING:
                    return ParseString(json, ref index, ref success);
                case TOKEN_CURLY_OPEN:
                    return ParseObject(json, ref index, ref success);
                case TOKEN_SQUARED_OPEN:
                    return ParseArray(json, ref index, ref success);
                case TOKEN_TRUE:
                    NextToken(json, ref index);
                    return true;
                case TOKEN_FALSE:
                    NextToken(json, ref index);
                    return false;
                case TOKEN_NULL:
                    NextToken(json, ref index);
                    return null;
                case TOKEN_NONE:
                    break;
            }

            success = false;
            return null;
        }

        protected static string ParseString(char[] json, ref int index, ref bool success)
        {
            StringBuilder s = new StringBuilder();
            char c;

            EatWhitespace(json, ref index);

            // "
            c = json[index++];

            bool complete = false;
            while (!complete)
            {

                if (index == json.Length)
                {
                    break;
                }

                c = json[index++];
                if (c == '"')
                {
                    complete = true;
                    break;
                }
                else if (c == '\\')
                {

                    if (index == json.Length)
                    {
                        break;
                    }
                    c = json[index++];
                    if (c == '"')
                    {
                        s.Append('"');
                    }
                    else if (c == '\\')
                    {
                        s.Append('\\');
                    }
                    else if (c == '/')
                    {
                        s.Append('/');
                    }
                    else if (c == 'b')
                    {
                        s.Append('\b');
                    }
                    else if (c == 'f')
                    {
                        s.Append('\f');
                    }
                    else if (c == 'n')
                    {
                        s.Append('\n');
                    }
                    else if (c == 'r')
                    {
                        s.Append('\r');
                    }
                    else if (c == 't')
                    {
                        s.Append('\t');
                    }
                    else if (c == 'u')
                    {
                        int remainingLength = json.Length - index;
                        if (remainingLength >= 4)
                        {
                            // parse the 32 bit hex into an integer codepoint
                            uint codePoint;
                            if (!(success = UInt32.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out codePoint)))
                            {
                                return "";
                            }
                            // convert the integer codepoint to a unicode char and add to string
                            s.Append(Char.ConvertFromUtf32((int)codePoint));
                            // skip 4 chars
                            index += 4;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                else
                {
                    s.Append(c);
                }

            }

            if (!complete)
            {
                success = false;
                return null;
            }

            return s.ToString();
        }

        protected static string ParseNumber(char[] json, ref int index, ref bool success)
        {
            EatWhitespace(json, ref index);

            int lastIndex = GetLastIndexOfNumber(json, index);
            int charLength = (lastIndex - index) + 1;

            string number = new string(json, index, charLength);
            success = true;

            index = lastIndex + 1;
            return number;
        }

        protected static int GetLastIndexOfNumber(char[] json, int index)
        {
            int lastIndex;

            for (lastIndex = index; lastIndex < json.Length; lastIndex++)
            {
                if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1)
                {
                    break;
                }
            }
            return lastIndex - 1;
        }

        protected static void EatWhitespace(char[] json, ref int index)
        {
            for (; index < json.Length; index++)
            {
                if (" \t\n\r".IndexOf(json[index]) == -1)
                {
                    break;
                }
            }
        }

        protected static int LookAhead(char[] json, int index)
        {
            int saveIndex = index;
            return NextToken(json, ref saveIndex);
        }

        protected static int NextToken(char[] json, ref int index)
        {
            EatWhitespace(json, ref index);

            if (index == json.Length)
            {
                return TOKEN_NONE;
            }

            char c = json[index];
            index++;
            switch (c)
            {
                case '{':
                    return TOKEN_CURLY_OPEN;
                case '}':
                    return TOKEN_CURLY_CLOSE;
                case '[':
                    return TOKEN_SQUARED_OPEN;
                case ']':
                    return TOKEN_SQUARED_CLOSE;
                case ',':
                    return TOKEN_COMMA;
                case '"':
                    return TOKEN_STRING;
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
                case '-':
                    return TOKEN_NUMBER;
                case ':':
                    return TOKEN_COLON;
            }
            index--;

            int remainingLength = json.Length - index;

            // false
            if (remainingLength >= 5)
            {
                if (json[index] == 'f' &&
                    json[index + 1] == 'a' &&
                    json[index + 2] == 'l' &&
                    json[index + 3] == 's' &&
                    json[index + 4] == 'e')
                {
                    index += 5;
                    return TOKEN_FALSE;
                }
            }

            // true
            if (remainingLength >= 4)
            {
                if (json[index] == 't' &&
                    json[index + 1] == 'r' &&
                    json[index + 2] == 'u' &&
                    json[index + 3] == 'e')
                {
                    index += 4;
                    return TOKEN_TRUE;
                }
            }

            // null
            if (remainingLength >= 4)
            {
                if (json[index] == 'n' &&
                    json[index + 1] == 'u' &&
                    json[index + 2] == 'l' &&
                    json[index + 3] == 'l')
                {
                    index += 4;
                    return TOKEN_NULL;
                }
            }

            return TOKEN_NONE;
        }

        protected static bool SerializeValue(object value, StringBuilder builder)
        {
            bool success = true;

            if (value is string)
            {
                success = SerializeString((string)value, builder);
            }
            else if (value is Hashtable)
            {
                success = SerializeObject((Hashtable)value, builder);
            }
            else if (value is ArrayList)
            {
                success = SerializeArray((ArrayList)value, builder);
            }
            else if (IsNumeric(value))
            {
                success = SerializeNumber(Convert.ToDouble(value), builder);
            }
            else if ((value is Boolean) && ((Boolean)value == true))
            {
                builder.Append("true");
            }
            else if ((value is Boolean) && ((Boolean)value == false))
            {
                builder.Append("false");
            }
            else if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                success = false;
            }
            return success;
        }

        protected static bool SerializeObject(Hashtable anObject, StringBuilder builder)
        {
            builder.Append("{");

            IDictionaryEnumerator e = anObject.GetEnumerator();
            bool first = true;
            while (e.MoveNext())
            {
                string key = e.Key.ToString();
                object value = e.Value;

                if (!first)
                {
                    builder.Append(", ");
                }

                SerializeString(key, builder);
                builder.Append(":");
                if (!SerializeValue(value, builder))
                {
                    return false;
                }

                first = false;
            }

            builder.Append("}");
            return true;
        }

        protected static bool SerializeArray(ArrayList anArray, StringBuilder builder)
        {
            builder.Append("[");

            bool first = true;
            for (int i = 0; i < anArray.Count; i++)
            {
                object value = anArray[i];

                if (!first)
                {
                    builder.Append(", ");
                }

                if (!SerializeValue(value, builder))
                {
                    return false;
                }

                first = false;
            }

            builder.Append("]");
            return true;
        }

        protected static bool SerializeString(string aString, StringBuilder builder)
        {
            builder.Append("\"");

            char[] charArray = aString.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];
                if (c == '"')
                {
                    builder.Append("\\\"");
                }
                else if (c == '\\')
                {
                    builder.Append("\\\\");
                }
                else if (c == '\b')
                {
                    builder.Append("\\b");
                }
                else if (c == '\f')
                {
                    builder.Append("\\f");
                }
                else if (c == '\n')
                {
                    builder.Append("\\n");
                }
                else if (c == '\r')
                {
                    builder.Append("\\r");
                }
                else if (c == '\t')
                {
                    builder.Append("\\t");
                }
                else
                {
                    int codepoint = Convert.ToInt32(c);
                    if ((codepoint >= 32) && (codepoint <= 126))
                    {
                        builder.Append(c);
                    }
                    else
                    {
                        builder.Append("\\u" + Convert.ToString(codepoint, 16).PadLeft(4, '0'));
                    }
                }
            }

            builder.Append("\"");
            return true;
        }

        protected static bool SerializeNumber(double number, StringBuilder builder)
        {
            builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture));
            return true;
        }

        protected static bool IsNumeric(object o)
        {
            double result;

            return (o == null) ? false : Double.TryParse(o.ToString(), out result);
        }
    }

    public class SafeDictionary<TKey, TValue>
    {
        private readonly object _Padlock = new object();
        private readonly Dictionary<TKey, TValue> _Dictionary = new Dictionary<TKey, TValue>();

        public bool ContainsKey(TKey key)
        {
            return _Dictionary.ContainsKey(key);
        }

        public TValue this[TKey key]
        {
            get
            {
                return _Dictionary[key];
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (_Padlock)
            {
                _Dictionary.Add(key, value);
            }
        }
    }
}
