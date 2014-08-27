using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper;
using Helper.Database;
namespace TestConsole
{
    public class TestgeneratePagingSql
    {
        public static void d()
        {
            string s = SqlStrhelp.GetMysqlPageStrsql0("*", "tbname", "idx", 25, 1, "desc", " name='njh' and idx>50 ");
            Console.WriteLine(s);
        }
    }
}
