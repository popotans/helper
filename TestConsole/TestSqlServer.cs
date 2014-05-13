using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper.Database;
namespace TestConsole
{
    //class TestSqlServer
    //{
    //    static NjhData GetDb()
    //    {
    //        NjhData db = new NjhData(DataType.Mssql, "server=192.168.104.117;uid=wftsa;password=jd7nTF#wM;database=K2Sln");
    //        return db;
    //    }
    //    public static void getList()
    //    {
    //        GetDb().Insert<BudgetItemList>(new BudgetItemList()
    //        {
    //            SN = "SH-WFYF-02,36789,134",
    //            Folio = "GFYBX11000130",
    //            ProcessInstanceID = "367890",
    //            ProcessInstanceName = "盛大游戏费用报销流程",
    //            Company = "SNDA",
    //            Creator = "snda\\niejunhua",
    //            CreateDay = DateTime.Now,
    //            BudgetID = "1022",
    //            Members = "FY11.Q4.867005321.CEOExpense",
    //            Msg = "msg",
    //            TotalAmout = "2222"
    //        });


    //        MyWatch mw = new MyWatch();
    //        mw.Start();
    //        List<BudgetItemList> list = GetDb().GetEntityList<BudgetItemList>(" select top 10 * from  BudgetItemList ");
    //        mw.End();
    //        int i = 0;
    //        foreach (BudgetItemList bi in list)
    //        {
    //            i++;
    //            Console.WriteLine(i + ">" + bi.Creator);
    //        }
    //    }
    //}
}
