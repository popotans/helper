using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper.Database;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Xml;
using TestProject1;
using Helper;
using System.Diagnostics;
namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            //TestSqlServer.getList();

            //Console.ReadLine();

            //string sss = "njhloveyangjing";

            //string rs = Helper.Security.SecurityHelper.Encrypt62(sss);
            //Console.WriteLine(rs);

            //begin query

            DbMap map = new DbMapSqlServer("server=192.168.104.117;uid=wftsa;password=jd7nTF#wM;database=K2Sln");
            int i = 0;
            while (i <= 100)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                string sql = "SELECT  top 100000 * FROM [K2Log].[dbo].[_ProcInstDataAudit]";
                List<_ProcInstDataAudit> list = map.GetList<_ProcInstDataAudit>(sql);
                //List<_ProcInstDataAudit> list = new List<_ProcInstDataAudit>();
                //    System.Data.IDataReader reader = map.DbContext.GetReader(sql);

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
                i++;
            }

        }
    }

    class TestOledb
    {
        NjhData nda = new Helper.Database.NjhData(Helper.Database.DataType.Oledb, "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=d:\\bak\\company.mdb;");
        public void DataTable1()
        {
            DataTable dt = null;
            for (int i = 0; i < 1; i++)
            {
                string sql = " select top 1111 * from companyinfo  ";
                dt = nda.ExecuteDataTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    // Console.WriteLine(dr[4]);
                }
            }
            MyWatch sp = new MyWatch();
            sp.Start();
            //  List<Company> list = nda.GetEntityList<Company>(dt);
            sp.End();
        }
        public void GetEntity()
        {
            //Company channel = nda.GetEntity<Company>(" select top 1 * from companyinfo where id=354");
            //if (channel == null) Console.WriteLine("Channel s null");
            //else Console.WriteLine(channel.CompanyName);
        }

        public void GetList()
        {
            //int iii = 304;
            //MyWatch sp = new MyWatch();
            //sp.Start();
            //for (int i = 1; i <= 1; i++)
            //{
            //    // iii += 50;
            //    List<Company> list = nda.GetEntityList<Company>("select top 1111 * from companyinfo where id>" + iii);
            //    foreach (Company c in list)
            //    {
            //        //  Console.WriteLine(c.CompanyName + "..." + c.Contact + "..." + c.ID);
            //    }
            //    //  GC.Collect();
            //    //  System.Threading.Thread.Sleep(1500);
            //}
            //sp.End();
        }

    }

    class TestMysql
    {
        //NjhData nda = new Helper.Database.NjhData(Helper.Database.DataType.Mysql, "server=127.0.0.1;uid=root;password=;database=nsys");
        //public void DataTable1()
        //{
        //    for (int i = 0; i < 1; i++)
        //    {
        //        string sql = " select * from channel  ";
        //        DataTable dt = nda.ExecuteDataTable(sql);
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            Console.WriteLine(dr[1]);
        //        }
        //    }
        //}

        //public void GetEntity()
        //{
        //    Channel channel = nda.GetEntity<Channel>("select * from channel limit 1,1");
        //    if (channel == null) Console.WriteLine("Channel s null");
        //    else Console.WriteLine(channel.Title);
        //}

        //public void GetList()
        //{
        //    for (int i = 1; i <= 100; i++)
        //    {
        //        List<Channel> list = nda.GetEntityList<Channel>("select * from channel");
        //        foreach (Channel c in list)
        //        {
        //            Console.WriteLine(c.Title + "..." + c.Url + "..." + c.OrderIndex);
        //        }
        //    }
        //}
    }

    class TestDataBase
    {

    }
}

class MyWatch
{

    System.Diagnostics.Stopwatch sp = new System.Diagnostics.Stopwatch();
    public void Start()
    {
        sp.Start();
    }

    public double End()
    {
        sp.Stop();
        double s = sp.Elapsed.TotalMilliseconds / 1000;
        Console.WriteLine("the seconds:" + s);
        //  Console.WriteLine(s);
        return s;
    }

}

