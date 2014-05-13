using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public class DateHelper
    {
        /// <summary>
        /// 某周第一天
        /// WeekNum=0 为本周，
        /// WeekNum=1，为下一周
        /// WeekNum=-1 为上一周
        /// </summary>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime now, int WeekNum = 0)
        {
            int rs = 1;
            switch (now.DayOfWeek)
            {
                case DayOfWeek.Monday: 
                    rs = 1;
                    break;
                case DayOfWeek.Tuesday:
                    rs = 2;
                    break;
                case DayOfWeek.Wednesday:
                    rs = 3;
                    break;
                case DayOfWeek.Thursday:
                    rs = 4;
                    break;
                case DayOfWeek.Friday:
                    rs = 5;
                    break;
                case DayOfWeek.Saturday:
                    rs = 6;
                    break;
                case DayOfWeek.Sunday:
                    rs = 7;
                    break;
            }
            return now.AddDays((1 - rs)).AddDays(7 * WeekNum);
        }

        public static DateTime ConverttoCsharpTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(string.Format("{0}{1}", timeStamp.ToString(), "0000000"));
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static long ConverttoUnixTime(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}
