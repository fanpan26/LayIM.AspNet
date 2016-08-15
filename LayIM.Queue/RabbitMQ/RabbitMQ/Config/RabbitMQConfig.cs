using LayIM.Utils.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.RabbitMQ.Server.Config
{
    public sealed class RabbitMQConfig
    {
        #region 私有变量和方法
        const string RabbitMQ_HostUri_Key = "RabbitMQ_HostUri";
        const string RabbitMQ_HostName_Key = "RabbitMQ_HostName";
        const string RabbitMQ_UserName_Key = "RabbitMQ_UserName";
        const string RabbitMQ_Password_Key = "RabbitMQ_Password";
        const string RabbitMQ_VirtualHost_Key = "RabbitMQ_VirtualHost";


        private static string getConfig(string key)
        {
            return AppSettings.GetValue(key);
        }

        private static string getKey(string key)
        {
            var _val = getConfig(key);
            ExceptionHandler.ThrowArgumentNull(key, _val);
            return _val;
        }

        #endregion


        /// <summary>
        /// 全局队列
        /// </summary>
        public const string PUBLISH_GLOBAL_QUEUE = "PUBLISH_GLOBAL_QUEUE";

      
        /// <summary>
        /// 主机服务地址
        /// </summary>
        public static string HostUri
        {
            get
            {
                return getKey(RabbitMQ_HostUri_Key);
            }
        }
        /// <summary>
        /// 主机服务地址
        /// </summary>
        public static string HostName
        {
            get
            {
                return getKey(RabbitMQ_HostName_Key);
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName
        {
            get
            {
                return getKey(RabbitMQ_UserName_Key);
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public static string PassWord
        {
            get
            {
                return getKey(RabbitMQ_Password_Key);
            }
        }

        public static string VirtualHost
        {
            get
            {
                return getKey(RabbitMQ_VirtualHost_Key);
            }
        }

        /// <summary>
        /// 是否持久化
        /// </summary>
        public static bool IsDurable
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 线程个数
        /// </summary>
        public static int ThreadCount
        {
            get
            {
                return 3;
            }
        }
    }
}
