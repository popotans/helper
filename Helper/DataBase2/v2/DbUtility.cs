using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Helper.Database
{
    public static class DbUtility
    {
        #region db common
        public static T GetDbValue<T>(IDataReader dr, string columnName)
        {
            object obj = null;
            if (dr[columnName] == DBNull.Value) return default(T);
            if (typeof(T) == typeof(int))
            {
                int rs = 0;
                if (int.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            else if (typeof(T) == typeof(string))
            {
                obj = dr[columnName].ToString();
            }
            else if (typeof(T) == typeof(long))
            {
                long rs = 0;
                if (long.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            else if (typeof(T) == typeof(double))
            {
                double rs = 0;
                if (double.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            else if (typeof(T) == typeof(float))
            {
                float rs = 0;
                if (float.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            else if (typeof(T) == typeof(decimal))
            {
                decimal rs = 0;
                if (decimal.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            else if (typeof(T) == typeof(DateTime))
            {

                DateTime rs = new DateTime(1970, 1, 1);
                if (DateTime.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            if (obj != null) return (T)obj;
            return default(T);
        }
        public static T GetDbValue<T>(DataRow dr, string columnName)
        {
            object obj = null;
            if (dr[columnName] == DBNull.Value) return default(T);
            if (typeof(T) == typeof(int))
            {
                int rs = 0;
                if (int.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            else if (typeof(T) == typeof(string))
            {
                obj = dr[columnName].ToString();
            }
            else if (typeof(T) == typeof(long))
            {
                long rs = 0;
                if (long.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            else if (typeof(T) == typeof(double))
            {
                double rs = 0;
                if (double.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            else if (typeof(T) == typeof(float))
            {
                float rs = 0;
                if (float.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            else if (typeof(T) == typeof(decimal))
            {
                decimal rs = 0;
                if (decimal.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            else if (typeof(T) == typeof(DateTime))
            {

                DateTime rs = new DateTime(1970, 1, 1);
                if (DateTime.TryParse(dr[columnName].ToString(), out rs))
                {
                    obj = rs;
                }
                else obj = -1;
            }
            if (obj != null) return (T)obj;
            return default(T);
        }
        #endregion

        #region T


        #endregion

    }
}
