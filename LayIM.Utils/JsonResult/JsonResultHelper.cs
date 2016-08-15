using LayIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Utils.JsonResult
{
    public static class JsonResultHelper
    {
        public static JsonResultModel CreateJson(object data, bool success = true, string msg = null)
        {
            return new JsonResultModel { code = success ? JsonResultType.Success : JsonResultType.Failed, data = data, msg = msg };
        }
    }
}
