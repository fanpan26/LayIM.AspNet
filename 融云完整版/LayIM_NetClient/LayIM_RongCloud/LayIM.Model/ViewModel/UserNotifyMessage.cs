using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Model.ViewModel
{
    public class UserNotifyMessage
    {
        public int id { get; set; }
        public string content { get; set; }
        public int uid { get; set; }
        public int from { get; set; }
        public int type { get; set; }
        public string remark { get; set; }
        public int read { get; set; }
        public string time { get; set; }
        public string avatar { get; set; }
        public string username { get; set; }
        public int result { get; set; }
    }
}
