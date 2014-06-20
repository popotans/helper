using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace TestProject1
{
    public partial class YFZYSQliucheng_GID03_rptNotes
    {

        [AutoIncrease, PrimaryKey]
        public Int32 ID { get; set; }

        public String ProcInstID { get; set; }
    }
}
