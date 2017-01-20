using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Utils.Extension
{
   public static class DateTimeExtension
    {
        /// <summary>
        /// 时间转换为时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int ToTimestamp(this DateTime dt)
        {

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(dt - startTime).TotalSeconds;
        }

        public static DateTime ToDateTime(this object obj, bool throwExceptionIfNotDateTime = false)
        {
            var t = obj.ToString();
            DateTime result;
            bool isTime = DateTime.TryParse(t, out result);
            if (!isTime)
            {
                throw new ArgumentException("not a valid datetime");
            }
            return isTime ? result : DateTime.Now;
        }
    }
}
