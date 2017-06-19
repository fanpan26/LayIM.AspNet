using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public class LayimOptions
    {
        public LayimOptions()
        {
            Serializer = new DefaultSerializer();
            UseRongCloud = true;
        }

        public string AppPath { get; set; }

        private bool _useRongCloud = true;
        public bool UseRongCloud
        {
            get { return _useRongCloud; }
            set
            {
                if (value)
                {
                    RongCloudSetting = new RongCloudSetting();
                }
                _useRongCloud = value;
            }
        }

        public RongCloudSetting RongCloudSetting { get; internal set; }

        internal IJsonSerializer Serializer { get; set; }

        /// <summary>
        /// 用户自定义序列化
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public LayimOptions UseSerializer(Func<IJsonSerializer> func)
        {
            var serializer = func();
            Serializer = serializer;
            return this;
        }
    }

    
}
