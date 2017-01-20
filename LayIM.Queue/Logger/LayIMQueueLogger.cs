using EasyNetQ;
using LayIM.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Queue.Logger
{
    public class LayIMQueueLogger : IEasyNetQLogger
    {
        public void DebugWrite(string format, params object[] args)                   
        {
            LogHelper.WriteLog(format, args);
        }

        public void ErrorWrite(Exception exception)
        {
            LogHelper.WriteLog(exception);
        }

        public void ErrorWrite(string format, params object[] args)
        {
            LogHelper.WriteLog(format, args);
        }

        public void InfoWrite(string format, params object[] args)
        {
            LogHelper.WriteLog(format, args);
        }
    }
}
