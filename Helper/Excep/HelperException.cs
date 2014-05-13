using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.Excep
{
    internal class HelperException : System.Exception
    {
        public HelperException()
            : base()
        {
           
        }

        public HelperException(string ex) : base(ex) { }


    }
}
