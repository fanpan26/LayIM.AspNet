using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Lib
{
    public static class TimeExtension
    {

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ToTimeStamp(this DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 将时间戳转换为时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this int timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static string ToTimeText(this DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;

            if (span.TotalDays > 7)
            {
                return dt.ToString("yyyy/MM/dd HH:mm");
            }

            if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }

            if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }

            if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }

            if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }

            return "刚刚";

        }
    }
}
