using LayIM.NetClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient.Model
{
    public class LayimApplyModel
    {
        public long id { get; set; }
        public long uid { get; set; }
        public long gid { get; set; }
        public int type { get; set; }
        public string avatar { get; set; }
        public string name { get; set; }
        public string sign { get; set; }
        public string groupname { get; set; }
        public string msg => getMsg();
        public string other { get; set; }

        public bool self { get; set; }

        private string _addtime;
        public string addtime { get { return _addtime; } set { _addtime = DateTime.Parse(value).ToTimeString(); } }
        public int result { get; set; }


        private string getMsg()
        {
            //type  result

            if (type == 0)
            {
                if (self)
                {
                    return "申请添加你为好友";
                }
                else
                {
                    if (result == 1)
                    {
                        return "通过了你的好友请求";
                    }
                    return "拒绝了你的好友请求";
                }
            }
            if (type == 1) {
                if (self)
                {
                    return $"申请加入[{groupname}]";
                }
                else
                {
                    if (result == 1)
                    {
                        return "群管理员通过了你的加群请求";
                    }
                    return "群管理员拒绝了你的加群请求";
                }
            }
            return "";
        }
    }
}
