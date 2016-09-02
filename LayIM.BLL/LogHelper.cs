using LayIM.Utils.Single;
using Macrosage.ElasticSearch.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.BLL
{
    public class LayIMLog
    {
        public static LayIMLog Instance
        {
            get { return SingleHelper<LayIMLog>.Instance; }
        }

        public void WriteLog(Exception ex)
        {
            ESLog.WriteLogException(System.Reflection.MethodBase.GetCurrentMethod(), ex);
        }
    }
}
