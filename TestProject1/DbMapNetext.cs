using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper;
using NUnit.Framework;
using njh;
using Xinshijie;
namespace TestProject1
{
    [TestFixture]
    public class DbMapNetext
    {
        DbMap map = new DbMapAccess(@"E:\快盘\project\CaiDown\CaiDown\down.mdb");
        [Test]
        public void InsertTest()
        {

            Console.WriteLine(map.db.ConnStr + "#" + map.db.ToString());

            njh.list list = new njh.list()
            {
                classname = "classname2",
                href = Guid.NewGuid().ToString(),
                idx = 1,
                title = "Title" + DateTime.Now.Ticks

            };

            int i = map.Insert<njh.list>(list);
            Assert.AreEqual(1, i);
        }
        [Test]
        public void TestLEntity()
        {
            njh.list list = map.Get<njh.list>("select * from list where idx=20");
            Assert.AreNotEqual(list, null);
            if (list != null)
            {
                IDictionary<string, object> dic = list.Serialize();
                foreach (string iyem in dic.Keys)
                {
                    Console.WriteLine(iyem + " = " + dic[iyem]);
                }
            }
        }

        [Test]
        public void TestLEntityList()
        {
            //njh.list list = map.Get<njh.list>("select * from list where idx=20");

            List<list> list11 = new List<njh.list>();
            list11 = map.GetList<list>("select top 200 * from list");

            Console.WriteLine(list11.Count);
            Assert.AreNotEqual(list11, null);
            Assert.AreEqual(list11.Count, 200);
            foreach (list li in list11)
            {
                if (li != null)
                {
                    IDictionary<string, object> dic = li.Serialize();
                    foreach (string iyem in dic.Keys)
                    {
                        Console.WriteLine(iyem + " = " + dic[iyem]);
                    }
                }
                Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            }
        }

        [Test]
        public void TestLUpdate()
        {
            njh.list listObj = map.Get<njh.list>("select * from list where idx=20");
            Assert.AreNotEqual(listObj, null);
            Console.WriteLine("Before Update:");
            if (listObj != null)
            {
                IDictionary<string, object> dic = listObj.Serialize();
                foreach (string iyem in dic.Keys)
                {
                    Console.WriteLine(iyem + " = " + dic[iyem]);
                }
            }
            Console.WriteLine("Now Updates:");
            listObj.classname = listObj.classname.Substring(2);
            listObj.href = listObj.href.Substring(32); ;
            listObj.title = listObj.title.Substring(32); ;
            map.Update<list>(listObj, "", map.Include("classname").Include("href").Columns);

            Console.WriteLine("After Updates:");
            listObj = map.Get<njh.list>("select * from list where idx=20");
            if (listObj != null)
            {
                IDictionary<string, object> dic = listObj.Serialize();
                foreach (string iyem in dic.Keys)
                {
                    Console.WriteLine(iyem + " = " + dic[iyem]);
                }
            }
        }

        Helper.DbMap mapMysql = new DbMapMySql("Server=localhost;Uid=root;Pwd=niejunhua;database=xinshijiecp");
        [Test]
        public void TestMySqlINsert()
        {
            cqssc ssc = new cqssc()
            {
                CpType = 1,
                Idx = 1,
                kjshijian = DateTime.Now,
                Nums = "1|2|3|4|5",
                pdate = 20140505,
                pnum = 023
            };

            int rows = mapMysql.Insert<cqssc>(ssc);
            Assert.AreEqual(1, rows);

        }

        [Test]
        public void TestGetMysql()
        {
            cqssc obj = mapMysql.Get<cqssc>("select * from cqssc where idx=124");
            Assert.AreNotEqual(obj, null);
            Console.WriteLine(obj.pnum + "," + obj.pdate + "," + obj.Nums + "," + obj.kjshijian + "," + obj.Idx);

            List<cqssc> litss = new List<cqssc>();
            litss = mapMysql.GetList<cqssc>("select * from cqssc order by idx  limit 0,10 ");

            foreach (cqssc obj1 in litss)
            {
                Console.WriteLine(obj1.pnum + "," + obj1.pdate + "," + obj1.Nums + "," + obj1.kjshijian + "," + obj1.Idx);
            }

            Assert.AreEqual(10, litss.Count);
        }

        [Test]
        public void TestUpdateMysql()
        {
            cqssc obj = mapMysql.Get<cqssc>("select * from cqssc where idx=124");
            Assert.AreNotEqual(obj, null);
            Console.WriteLine("before update:");
            Console.WriteLine(obj.pnum + "," + obj.pdate + "," + obj.Nums + "," + obj.kjshijian + "," + obj.Idx);

            obj.Nums = "55666555";
            obj.kjshijian = DateTime.Now;
            obj.pnum = 6;
            mapMysql.Update<cqssc>(obj, "", mapMysql.Include("nums").Columns);
            Console.WriteLine("after update:");
            Console.WriteLine(obj.pnum + "," + obj.pdate + "," + obj.Nums + "," + obj.kjshijian + "," + obj.Idx);


        }

    }
}
