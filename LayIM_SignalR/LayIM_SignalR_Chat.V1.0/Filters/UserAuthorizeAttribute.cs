using LayIM.Cache;
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
            var userid = LayIMCache.Instance.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userid))
            {
                filterContext.Result = new JsonResult
                {
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = JsonResultHelper.CreateJson(null, false, "unauthorized")
                };
            }
            else
            {
                filterContext.ActionParameters["userid"] = userid.ToInt();
            }

            base.OnActionExecuting(filterContext);
        }

    }
}