using Helper.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Helper;

namespace TestProject1
{
    [TestClass()]
    public class ImageClassTest
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
        public void SaveTest()
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + "\\a.jpg";
              string filenamenew=filename+"_new.jpg";
            //    ImageHelper.AddWater(filename, filename + "_new.jpg", "www.im1024.com");
             // ImageHelper.LocalImage2Thumbs(filename, filenamenew, 400, 300);
              ImageHelper.RemoteImage2Thumbs("http://a.hiphotos.baidu.com/image/w%3D2048/sign=e9e1e60e5b82b2b7a79f3ec40595caef/b58f8c5494eef01f728c66cfe2fe9925bc317d1b.jpg", filename, 200, 250);
              ImageHelper.AddWater(filename, filename + "_new.jpg", "www.im1024.com");
        }

    }
}


