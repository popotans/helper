using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helper;
namespace TestProject1
{
    [TestClass]
    public class MySqlArticleTest
    {
        MySqlModeCreate create = new MySqlModeCreate("server=localhost;User Id=root;password=niejunhua;Database=nq_article;charset=utf8 ");
        [TestMethod]
        public void TestMethod1()
        {
            create.CreateAll("CommonClass", "nq_article", "");
        }
    }
}
