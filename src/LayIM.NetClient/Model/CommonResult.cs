using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient.Model
{
    public class CommonResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}
