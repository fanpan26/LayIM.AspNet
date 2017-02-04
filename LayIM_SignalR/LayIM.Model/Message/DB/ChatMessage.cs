using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Model.Message.DB
{
    public class ChatMessage
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set;}
        public string ChatGroupId { get; set; }
        public string Msg { get; set; }
        public short ChatType { get; set; }
        public int AddTime { get; set; }
        public short MsgType { get; set; } = 0;
    }
}
