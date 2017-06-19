using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient.Model
{
   public class LayimChatMessageViewModel
    {
        public long uid { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string content { get; set; }
        public string addtime { get; set; }

        public bool self { get; set; }

        public long timestamp { get; set; }
    }

    public class LayimHistoryParam
    {
        public long UserId { get; set; }
        public long ToId { get; set; }
        public string Type { get; set; }
        public long MsgTimestamp { get; set; }

        private int _page = 20;
        public int Page
        {
            get { return _page; }
            set { if (value > 0) { _page = value; } }
        }
    }
}
