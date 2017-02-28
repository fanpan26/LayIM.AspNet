using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Model.Result
{
   public class SendMessageResult
    {
        private bool _result;
        public SendMessageResult(bool success)
        {
            _result = success;
        }
        public bool Success
        {
            get { return _result; }
        }
        public string msg { get; set; }
        public object data { get; set; }
    }
}
