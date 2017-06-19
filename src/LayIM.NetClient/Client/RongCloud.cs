using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    internal class RongCloud
    {
        //融云接口
        private static readonly string domain = "http://api.cn.ronghub.com";

        private static object _rcLock = new object();

        private RestClient _client;
        private RestClient Client
        {
            get
            {
                if (_client == null)
                {
                    lock (_rcLock)
                    {
                        if (_client == null)
                        {
                            _client = CreateClient();
                        }
                    }
                }
                return _client;
            }
        }

        private RongCloudSetting _setting;
        private RestClient CreateClient()
        {
            RestClient client = new RestClient(domain);
          
            client.Encoding = Encoding.UTF8;

            return client;
        }

        public RongCloud(RongCloudSetting setting)
        {
            Error.ThrowIfNull(setting, nameof(setting));
            if (string.IsNullOrEmpty(setting.AppKey) || string.IsNullOrEmpty(setting.AppSecret)) {
                throw new ArgumentException("RongCloud_AppKey 或 RongCloud_AppSecret 未配置");
            }
            _setting = setting;
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private T Execute<T>(string uri,Method method= Method.GET, Dictionary<string, object> parameters=null) where T:class,new()
        {
            RestRequest request = new RestRequest(uri, method);
            //添加通用请求头部信息
            request.AddRongCloudHeader(_setting.AppKey,_setting.AppSecret);

            if (parameters != null) {
                if (method == Method.GET)
                {
                    foreach (KeyValuePair<string, object> kv in parameters)
                    {
                        request.AddQueryParameter(kv.Key, kv.Value?.ToString());
                    } 
                }
                /*
                 *POST请求 注意Type 一定是 ParameterType.GetOrPost 否则会接不到Post请求的参数
                 */
                if (method == Method.POST)
                {
                    foreach (KeyValuePair<string, object> kv in parameters)
                    {
                        request.AddParameter(new Parameter { Type = ParameterType.GetOrPost, Name = kv.Key, Value = kv.Value });
                    }
                }

            }

            //{"code":200,"userId":"100000","token":"i9XkP8ilAhpeDnoDOjc7f+emrJqyqVNKmklPjy4zV1B8UtBDxKuOEVbo0CmgFwJs+bqG7kANb/Pt3DD2aoW6nA=="}
            var response = Client.Execute<T>(request);

            if (response?.StatusCode == 0)
            {
                //记录出错日志
                return default(T);
            }
            return response.Data;
        }

        /// <summary>
        /// 获取 用户Token
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="userName">用户名可为空</param>
        /// <param name="portraitUri">用户头像 可为空</param>
        /// <returns></returns>
        public dynamic GetToken(string userId, string userName = "", string portraitUri = "")
        {
            var parameter = new Dictionary<string, object> {
                { "userId", userId },
                { "name", userName },
                { "portraitUri", portraitUri }
            };

            var token = Execute<RongCloudToken>("/user/getToken.json", Method.POST, parameters: parameter);

            return new { token = token?.token };
        }
    }

    internal static class RongCloudContainer
    {
        private static ConcurrentDictionary<string, RongCloud> RongCloudCache = new ConcurrentDictionary<string, RongCloud>();
        public static RongCloud CreateInstance(RongCloudSetting setting)
        {
            RongCloud rc;
            if (RongCloudCache.TryGetValue(setting.AppKey, out rc))
            {
                return rc;
            }
            else
            {
                rc = new RongCloud(setting);
                RongCloudCache.TryAdd(setting.AppKey, rc);
                return rc;
            }
        }
    }

    internal class RongCloudToken
    {
        public int code { get; set; }
        public long userId { get; set; }
        public string token { get; set; }
    }
}
