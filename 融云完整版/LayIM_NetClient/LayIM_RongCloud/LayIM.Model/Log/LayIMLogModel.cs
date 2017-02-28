using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Model.Log
{
    /// <summary>
    /// 日志实体类
    /// </summary>
    public class LayIMLogModel
    {
        /// <summary>
        /// 日志创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public bool IsExceptionLog { get { return Exception != null; } }
    }
}
