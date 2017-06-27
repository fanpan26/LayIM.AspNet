using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    /// <summary>
    /// 请求抽象类
    /// </summary>
    public abstract class LayimRequest
    {
        public abstract string Method { get;}
        public abstract string Path { get; }
        public abstract string PathBase { get; }

        public abstract string LocalIpAddress { get; }
        public abstract string RemoteIpAddress { get; }

        public abstract RequestCookieCollection Cookies { get; }

        public abstract IReadableStringCollection Query { get; }

        public abstract IHeaderDictionary Header { get; }

        public abstract Stream Body { get; }

        public abstract string GetQuery(string key);

        public abstract Task<IList<string>> GetFormValuesAsync(string key);

        /// <summary>
        /// 根据Key集合获取Form数据信息
        /// </summary>
        /// <param name="key">例如 key 为 {"uid","toid","msg","time"}</param>
        /// <returns>返回一个字典，key为对应的key，value为form中的value值</returns>
        public abstract Task<IDictionary<string, string>> GetFormKeyValuesAsync(params string[] key);


    }
}
