using LayIM.Utils.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using LayIM.Cache.RedisUtil;
using LayIM.Utils.Extension;
using LayIM.Utils.Config;

namespace LayIM.Cache
{
   public class LayIMCache
    {
        #region 变量
        public static LayIMCache Instance
        {
            get
            {
                return SingleHelper<LayIMCache>.Instance;
            }
        }

        private ConnectionMultiplexer _redis;
        private static object _lock = new object();
        public ConnectionMultiplexer RedisClient
        {
            get
            {
                if (_redis == null)
                {
                    lock (_lock)
                    {
                        if (_redis == null)
                        {
                            var configuration = new ConfigurationOptions()
                            {
                                Password = "",//layim_cache_!@#
                                ConnectRetry = 3,
                                AllowAdmin = true,
                                Ssl = false,
                                EndPoints = { AppSettings.GetValue("Redis_HostName")}
                            };
                            _redis = ConnectionMultiplexer.Connect(configuration);

                        }
                    }
                }
                return _redis;
            }
        }
        private IDatabase db {
            get { return RedisClient.GetDatabase(); }
        }
        #endregion

        #region 通过反射，将实体以HashEntry的形式保存
        /// <summary>
        /// 保存实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SetHash<T>(string key, T model)
        {
            try
            {
                db.HashSet(key, model.ToHashEntries());
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        ///  读取实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetHash<T>(string key)
        {
            var result = db.HashGetAll(key).ConvertFromRedis<T>();
            return result;
        }

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
       public bool RemoveHash(string key)
        {

            return db.KeyDelete(key);
        }
        #endregion

        public bool Set(string key, string value)
        {
            return db.StringSet(key, value);
        }
        public string Get(string key)
        {
            return db.StringGet(key);
        }

    }
}
