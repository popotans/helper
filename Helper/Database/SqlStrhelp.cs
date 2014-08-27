using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Helper.Database
{
    public class SqlStrhelp
    {
        /// <summary>
        /// 分页sql语句，支持多个字段
        /// </summary>
        /// <param name="SelectFields">输出字段，以逗号隔开（记住：一定要将排序所用到的字段一并输出）</param>
        /// <param name="TblName">表名</param>
        /// <param name="TotalCount">总记录数</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="Order">排序方式</param>
        /// <param name="whereStr">搜索条件</param>
        /// <returns></returns>
        public static string GetSql1(string SelectFields, string TblName, int TotalCount, int PageSize, int PageIndex, NameValueCollection Order, string whereStr)
        {
            //排序字段 
            string orderList = "";//用户期望的排序 
            string orderList2 = "";//对用户期望的排序的反排序 
            string orderList3 = "";//用户期望的排序,去掉了前缀.复合查询里的外层的排序不能是类似这样的table1.id,要去掉table1.。 
            if (Order.Count > 0)
            {
                string[] str = Order.AllKeys;
                foreach (string s in str)
                {
                    string direction = "asc";//默认一个方向 
                    if (Order[s].ToString() == "asc")
                        direction = "desc";
                    //去掉前缀的字段名称 
                    string s2 = "";
                    int index = s.IndexOf(".") + 1;
                    s2 = s.Substring(index);
                    orderList = orderList + s + " " + Order[s] + ",";
                    orderList2 = orderList2 + s2 + " " + direction + ",";
                    orderList3 = orderList3 + s2 + " " + Order[s] + ",";
                }
                //去掉最后的,号 
                orderList = orderList.Substring(0, orderList.Length - 1);
                orderList2 = orderList2.Substring(0, orderList2.Length - 1);
                orderList3 = orderList3.Substring(0, orderList3.Length - 1);
            }
            //形成SQL 
            string strTemp;
            //判断是不是最后一页
            if ((TotalCount > 0) && (TotalCount % PageSize > 0) && (PageIndex > (TotalCount / PageSize)))
            {
                strTemp = "select * from ( select top {5} {0} from {1} ";
                if (whereStr != "")
                    strTemp = strTemp + " where {2} ";
                if (orderList != "")
                    strTemp = strTemp + " order by {4})  as tmp order by {3}";
                strTemp = string.Format(strTemp, SelectFields, TblName, whereStr, orderList, orderList2, TotalCount % PageSize);
            }
            else
            {
                strTemp = "select * from ( select top {7} * from ( select top {6} {0} from {1} ";
                if (whereStr != "")
                    strTemp = strTemp + " where {2} ";
                if (orderList != "")
                    strTemp = strTemp + " order by {3} ) as tmp order by {4} ) as tmp2 order by {5} ";
                strTemp = string.Format(strTemp, SelectFields, TblName, whereStr, orderList, orderList2, orderList3, PageIndex * PageSize, PageSize);
            }

            return strTemp;
        }

        /// <summary>
        /// 分页sql语句生成代码
        /// </summary>
        /// <param name="SelectFields"></param>
        /// <param name="TblName"></param>
        /// <param name="OrderFieldName">排序字段,唯一性</param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex">从1开始</param>
        /// <param name="OrderType">desc asc</param>
        /// <param name="whereStr">name='njh'</param>
        /// <returns></returns>

        public static string GetPageStrSql0(string SelectFields, string TblName, string OrderFieldName, int PageSize, int PageIndex, string OrderType, string whereStr)
        {
            string StrTemp = "";
            string StrSql = "";
            string StrOrder = "";
            //根据排序方式生成相关代码
            if (OrderType.ToUpper() == "ASC")
            {
                StrTemp = "> (SELECT MAX(" + OrderFieldName + ")";
                StrOrder = " ORDER BY " + OrderFieldName + " ASC";
            }
            else
            {
                StrTemp = "< (SELECT MIN(" + OrderFieldName + ")";
                StrOrder = " ORDER BY " + OrderFieldName + " DESC";
            }
            PageIndex = PageIndex < 1 ? 1 : PageIndex;
            //若是第1页则无须复杂的语句
            if (PageIndex == 1)
            {
                StrTemp = "";
                if (whereStr != "")
                    StrTemp = " Where " + whereStr;
                StrSql = "SELECT TOP " + PageSize + " " + SelectFields + " From " + TblName + "" + StrTemp + StrOrder;
            }
            else
            {
                //若不是第1页，构造sql语句
                StrSql = "SELECT TOP " + PageSize + " " + SelectFields + " From " + TblName + " WHERE " + OrderFieldName + "" + StrTemp + " From (SELECT TOP " + (PageIndex - 1) * PageSize + " " + OrderFieldName + " From " + TblName + "";
                if (whereStr != "")
                    StrSql += " Where " + whereStr;
                StrSql += StrOrder + ") As Tbltemp)";
                if (whereStr != "")
                    StrSql += " And " + whereStr;
                StrSql += StrOrder;
            }
            //返回sql语句
            return StrSql;
        }

        public static string GetMysqlPageStrsql0(string SelectFields, string TblName, string OrderFieldName, int PageSize, int PageIndex, string OrderType, string whereStr)
        {
            string sql = " select {0} from `{1}` where {2} order by {3} {4} limit {5},{6}";
            sql = string.Format(sql, SelectFields, TblName, whereStr, OrderFieldName, OrderType, (PageIndex - 1) * PageSize, PageSize);
            return sql;
        }

    }
}
