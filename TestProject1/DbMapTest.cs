using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestProject1
{
    /// <summary>
    ///这是 DbMapTest 的测试类，旨在
    ///包含所有 DbMapTest 单元测试
    ///</summary>
    [TestClass()]
    public class DbMapTest
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

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void InsertTest()
        {

            //   DbMap map = new DbMapAccess(@"E:\快盘\project\CaiDown\CaiDown\down.mdb");

            //   BaseModeCreate create = new SqlServerModeCreate("server=192.168.104.117;uid=wftsa;password=jd7nTF#wM;database=K2Sln");
            //  create.CreateAll("TestProject1", "K2log", "");

            //string s = Helper.Str.StringHelper.SubStrContain("11111@ebasdasdas@ee19851022", "@eb", "@ee");
            //Console.WriteLine(s);

            //begin query

            DbMap map = new DbMapSqlServer("server=192.168.104.117;uid=wftsa;password=jd7nTF#wM;database=K2Sln");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string sql = "SELECT  top 100000 * FROM [K2Log].[dbo].[_ProcInstDataAudit]";
            List<_ProcInstDataAudit> list = map.GetList<_ProcInstDataAudit>(sql);
            //List<_ProcInstDataAudit> list = new List<_ProcInstDataAudit>();
            System.Data.IDataReader reader = map.DbContext.GetReader(sql);

            //while (reader.Read())
            //{
            //    _ProcInstDataAudit m = new _ProcInstDataAudit();
            //    m.Date = DateTime.Parse(reader["date"].ToString());
            //    m.Location = reader["location"].ToString();
            //    m.Name = reader["name"].ToString();
            //    m.ProcInstID = int.Parse(reader["ProcInstID"].ToString());
            //    m.User = reader["user"].ToString();
            //    m.Value = reader["value"].ToString();
            //  //  list.Add(m);
            //}
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            //endquery

        }
    }
}
