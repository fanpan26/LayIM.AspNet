using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    /// <summary>
    /// 序列化器
    /// </summary>
    public interface IJsonSerializer
    {
        T DeserializeObject<T>(string json);
        string SerializeObject(object value);
    }
}
