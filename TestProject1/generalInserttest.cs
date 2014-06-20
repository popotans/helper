using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;

namespace TestProject1
{
    /// <summary>
    ///这是 DbMapTest 的测试类，旨在
    ///包含所有 DbMapTest 单元测试
    ///</summary>
    [TestClass()]
    public class DbMapTest222
    {
        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        [TestMethod()]
        public void InsertTest()
        {
            MyProcInstData mpdata = new MyProcInstData()
            {
                ID = 11,
                Name = "niasdja",
                ProcInstID = 222,
                Value = "value"
            };

            DbMap map = new DbMapMySql("");
            System.Data.IDbDataParameter[] arrr = new System.Data.IDbDataParameter[] { };
            string sql = map.GeneralUpdateRef<MyProcInstData>(mpdata, new List<string>() { "Name", "Birthday" }, ref arrr);
            Console.WriteLine(sql);
            foreach (IDbDataParameter p in arrr)
            {
                Console.WriteLine(p.ParameterName + "," + p.Value + "," + p.Value.GetType());
            }
        }
    }
}
