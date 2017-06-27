using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    /// <summary>
    /// 融云配置
    /// </summary>
    public sealed class RongCloudSetting
    {

        private string _appKey;
        private string _appSecret;

        public RongCloudSetting(string key, string secret)
        {
            Error.ThrowIfNull(key, nameof(key));
            Error.ThrowIfNull(secret, nameof(secret));

            _appKey = key;
            _appSecret = secret;
        }

        public RongCloudSetting()
        {
            _appKey = ConfigurationManager.AppSettings["RongCloud_AppKey"]?.ToString();
            _appSecret = ConfigurationManager.AppSettings["RongCloud_AppSecret"]?.ToString(); 
        }

        public string AppKey => _appKey;
        public string AppSecret => _appSecret;
    }
}
