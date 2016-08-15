using LayIM.Utils.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Utils.Extension
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 转int
        /// </summary>
        /// <param name="value">原来的值</param>
        /// <param name="throwIfZero">如果为0 是否抛异常</param>
        /// <returns></returns>
        public static int ToInt(this object value,bool throwIfZero=false)
        {
            string oldvalue = value.ToString();
            int result = 0;
            bool trans = int.TryParse(oldvalue, out result);
            if (trans) {
                return result;
            }
            if (throwIfZero) {
                throw new ArgumentException("value is zero");
            }
            return result;
        }

        public static short ToShort(this object value, bool throwIfZero = false)
        {
            string oldvalue = value.ToString();
            short result = 0;
            bool trans = short.TryParse(oldvalue, out result);
            if (trans)
            {
                return result;
            }
            if (throwIfZero)
            {
                throw new ArgumentException("value is zero");
            }
            return result;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson(this object value)
        {
            return JsonHelper.SerializeObject(value);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string value)
        {
            return JsonHelper.DeserializeObject<T>(value);
        }
    }
}
