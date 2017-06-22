using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public static class Extensions
    {
        public static int ToInt32(this string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }

        public static long ToInt64(this string value)
        {
            long result = 0;
            long.TryParse(value, out result);
            return result;
        }

        public static long ToTimestamp(this DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalSeconds;
        }

        public static string ToTimeString(this DateTime time)
        {
            long totalSeconds =(long) (DateTime.Now - time).TotalSeconds;

            if (totalSeconds <= 2) {
                return "刚刚";
            }
            if (totalSeconds > 2 && totalSeconds <60) {
                return $"{totalSeconds}秒前";
            }
            if (totalSeconds >= 60 && totalSeconds < 3600) {
                return $"{totalSeconds / 60}分钟前";
            }
            if (totalSeconds >= 3600 && totalSeconds < 3600 * 24) {
                return $"{totalSeconds / 3600}小时前";
            }
            if (totalSeconds >= 3600 * 24 && totalSeconds < 3600 * 24 * 7) {
                return $"{totalSeconds / (3600 * 24)}天前";
            }
           
            return time.ToString("yyyy-MM-dd");
        }
    }
}
