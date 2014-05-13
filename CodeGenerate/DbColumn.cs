using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coder
{
    class Class1
    {
    }

    public class DbColumn
    {
        public string ColumnName { get; set; }
        public Type ColumnType { get; set; }
        public string Description { get; set; }
        public bool IsAutoIncrement { get; set; }
        public string TableSchema { get; set; }

        public string IdentityKeys { get; set; }
    }
}
