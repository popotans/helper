using Helper.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestProject1
{


    /// <summary>
    ///这是 XmlHelperTest 的测试类，旨在
    ///包含所有 XmlHelperTest 单元测试
    ///</summary>
    [TestClass()]
    public class XmlHelperTest
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


        /// <summary>
        ///Save 的测试
        ///</summary>
        public void SaveTestHelper<T>()
        {
            T t = default(T); // TODO: 初始化为适当的值
            Dictionary<string, object> expected = null; // TODO: 初始化为适当的值
            Dictionary<string, object> actual;
            actual = XmlHelper.Save<T>(t);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SaveTest()
        {
            //SaveTestHelper<GenericParameterHelper>();
            Person p = new Person() { Age = 11, Bitrh = DateTime.Now, Name = "niejunhua" };

            XmlHelper.Save<Person>(p);

        }

        [TestMethod]
        public void TEstRead()
        {
            Person pp = XmlHelper.Read<Person>();

            if (pp == null) Console.WriteLine("is null");
            else
            {

                Console.WriteLine(pp.Name + " " + pp.Age + "  " + pp.Bitrh);
            }
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public DateTime Bitrh { get; set; }
        public int Age { get; set; }

    }
}
