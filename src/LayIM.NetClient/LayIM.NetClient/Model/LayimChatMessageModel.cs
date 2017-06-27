using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient.Model
{
    public class LayimChatMessageModel
    {
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime CreateAt { get; set; }
        public long TimeStamp => CreateAt.ToTimestamp();
    }
}
