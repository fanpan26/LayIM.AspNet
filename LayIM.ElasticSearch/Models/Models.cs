using Macrosage.ElasticSearch.Core;
using Macrosage.ElasticSearch.Model;
using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Macrosage.ElasticSearch.Models
{
    /// <summary>
    /// 平台信息
    /// </summary>
    public class ChatInfo : BaseEntity
    {
        public ChatInfo()
        {
            id = Guid.NewGuid().ToString();
        }
        public long qq { get; set; }
        public string avatar { get; set; }
        public DateTime addtime { get; set; }
        public long timespan { get; set; }
        public string nickname { get; set; }
        public string content { get; set; }
        public string roomid { get; set; }
        public bool isimg { get; set; }
        public bool isfile { get; set; }
    }

    public class LayImUser : BaseEntity
    {
        public LayImUser() {
            province = 1;
            provincestr = "北京";
            city = 11;
            citystr = "海淀区";
        }
        public long im { get; set; }
        public string avatar { get; set; }
        public string nickname { get; set; }
        public int province { get; set; }
        public string provincestr { get; set; }
        public int city { get; set; }
        public string citystr { get; set; }
        public bool ismale { get; set; }
        public string sign { get; set; }
        public DateTime addtime { get; set; }
        public string addtimestr { get { return addtime.ToString("yyyy/MM/dd HH:mm"); } }
        public int timestamp { get; set; }
    }

    public class LayImGroup : BaseEntity
    {
        public string avatar { get; set; }
        public string groupdesc { get; set; }
        public int im { get; set; }
        public string groupname { get; set; }
        public int allcount { get; set;}
        public int limitcount{ get; set; }
        public DateTime addtime { get; set; }
        public string addtimestr {
            get {
                return addtime.ToString("yyyy/MM/dd");
            }
        }
    }

    /// <summary>
    /// 日志信息
    /// </summary>
    public class LogInfo : BaseEntity
    {
        public LogInfo()
        {
            logtime =DateTime.Now;
            ip = getIP();
        }

        private string getIP()
        {
            string hostName = Dns.GetHostName();//本机名   
            IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
            if (addressList != null && addressList.Length >1)
            {
                return addressList[1].ToString();
            }
            return "未知";
        }

        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime logtime
        {
            get; set;
        }
        /// <summary>
        /// 日志类型 0 异常 1 普通记录
        /// </summary>
        public LogType logtype { get { return _logtype; } set { _logtype = value; } }
        public string logtypestr
        {
            get
            {
                switch (logtype)
                {
                    case LogType.Exception:
                        return "程序异常";
                    case LogType.Info:
                        return "普通日志";
                    case LogType.Quartz:
                        return "任务调度";
                }
                return "";
            }
        }
        private LogType _logtype;
        /// <summary>
        /// 日志标题
        /// </summary>
        public string logtitle { get; set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string methodname { get; set; }
        /// <summary>
        /// 方法参数
        /// </summary>
        public string methodparameters { get; set; }
        /// <summary>
        /// 日志详情
        /// </summary>
        public string detail { get; set; }
        public string ip { get; set; }
    }
    public enum LogType
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        Exception = 0,
        /// <summary>
        /// 普通记录
        /// </summary>
        Info = 1,
        /// <summary>
        /// 任务日志
        /// </summary>
        Quartz = 2

    }

    public class ElasticChat : Elastic<ChatInfo>
    {
        public override IEnumerable<ChatInfo> HitsToList(SearchResult<ChatInfo>.SearchHits hits)
        {
            var result = new List<ChatInfo>();

            var hitsList = hits.hits.ToList();
            hitsList.ForEach(x => {
                if (x.highlight != null)
                {
                    x._source.content = x.highlight["content"][0];
                }
                result.Add(x._source);
            });
            return result;
        }
    }
}
