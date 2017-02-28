using LayIM.Model;
using LayIM_RongCloud_Chat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LayIM_RongCloud_Chat.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UserAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userid = UserProvider.GetUserId();
            if (userid == null || userid == 0)
            {
                filterContext.Result = new JsonResult
                {
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new JsonResultModel { code = JsonResultType.Success, msg = "unauthorized" }
                };
            }
            else
            {
                filterContext.ActionParameters["userId"] = userid;
            }

            base.OnActionExecuting(filterContext);
        }

    }
}