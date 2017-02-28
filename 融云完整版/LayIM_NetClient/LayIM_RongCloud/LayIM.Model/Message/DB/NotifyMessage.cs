using LayIM.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Model.Message.DB
{
    public class NotifyMessage
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public int ToUserId { get; set; }
        public NotifyType Type { get; set; }
        public string Other { get; set; }
        public int AddTime { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsRead { get; set; }
        public int OperateResult { get; set; }
        public int UpdateTime { get; set; }
    }

    
}
