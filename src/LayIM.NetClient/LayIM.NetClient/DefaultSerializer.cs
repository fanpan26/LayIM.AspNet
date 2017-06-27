using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LayIM.NetClient
{
    /// <summary>
    /// 默认序列化器
    /// </summary>
    internal class DefaultSerializer : IJsonSerializer
    {
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        public T DeserializeObject<T>(string json)
        {
            return serializer.Deserialize<T>(json);
        }

        public string SerializeObject(object value)
        {
            return serializer.Serialize(value);
        }
    }
}
