using LayIM.LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LayIM_RongCloud_Chat.Controllers
{
    /// <summary>
    /// LayIM数据服务
    /// </summary>
    public class DataController : Controller
    {
        /// <summary>
        /// 获取基础数据
        /// </summary>
        /// <param name="userId">根据用户id</param>
        /// <returns></returns>
        public JsonResult GetUserBaseList(int userId)
        {

            UserService service = new UserService();
            var model = service.GetUserBase(userId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddMsg(int from, int to, string type, string message)
        {
            MessageService service = new MessageService();
            service.AddMessage(new LayIM.Model.Message.DB.ChatMessage
            {
                 
            });
            return null;
        }
    }
}