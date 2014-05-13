using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper.DbDataType;

namespace Helper
{
    public interface IDicSerialize
    {
        string TableName { get; }
        System.Collections.Generic.IDictionary<string, MyField> ToDic();
        object ToObj(System.Collections.Generic.IDictionary<string, MyField> dic);
    }
}
