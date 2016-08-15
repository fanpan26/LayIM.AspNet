using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Utils.Serialize
{
    public sealed class JsonHelper
    {
        #region 序列化
        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
        #endregion

        #region 反序列化
        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        #endregion
    }
}
