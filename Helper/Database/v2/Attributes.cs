using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class PrimaryKeyAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class AutoIncreaseAttribute : Attribute { }

}
