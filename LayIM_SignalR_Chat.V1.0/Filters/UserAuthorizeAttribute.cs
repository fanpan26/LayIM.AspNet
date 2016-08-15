using LayIM.Utils.Consts;
using LayIM.Utils.Extension;
using LayIM.Utils.JsonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LayIM_SignalR_Chat.V1._0.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UserAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            var userid = HttpContext.Current.Request.Cookies[LayIMConst.LayIM_SignalR_UserId];
            if (userid == null || userid.Value.ToInt() == 0)
            {
                filterContext.Result = new JsonResult
                {
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = JsonResultHelper.CreateJson(null, false, "用户没有登录")
                };
            }
            else
            {
                filterContext.ActionParameters["userid"] = userid.Value.ToInt();
            }

            base.OnActionExecuting(filterContext);
        }

    }
}