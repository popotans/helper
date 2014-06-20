using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace TestProject1
{
    public partial class MyProcInstData
    {

        public Int32 ProcInstID { get; set; }
        [PrimaryKey,AutoIncrease]
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Value { get; set; }
        public DateTime Birthday { get; set; }
    }
}
