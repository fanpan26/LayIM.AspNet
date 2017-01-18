using Macrosage.IMServer.RMLib.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LayIM_RongCloud_Chat.Controllers
{
    public class IMController : Controller
    {
        /// <summary>
        /// 获取融云token
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public JsonResult GetToken(long? uid, bool refresh = false)
        {
            RMTool tool = new RMTool();
            if (uid.HasValue)
            {
                var result = tool.GetToken(uid.Value, refresh: refresh);
                var json = new { token = result };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return Json(new { token = string.Empty }, JsonRequestBehavior.AllowGet);
        }
    }
}