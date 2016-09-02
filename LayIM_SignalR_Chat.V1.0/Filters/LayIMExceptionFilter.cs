using LayIM.BLL;
using LayIM.Utils.JsonResult;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LayIM_SignalR_Chat.V1._0.Filters
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class LayIMExceptionFilter : HandleErrorAttribute
    {
        public LayIMExceptionFilter() { }

        public override void OnException(ExceptionContext filterContext)
        {

            string path = filterContext.RequestContext.HttpContext.Server.MapPath($"/log/{DateTime.Now.ToString("yyyy-MM-dd")}.txt");
            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("========= " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ============");
                }
            }

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("========= " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ============"); sw.WriteLine("This");
                sw.WriteLine("Message:" + filterContext.Exception.Message);
                sw.WriteLine("StackTrace:" + filterContext.Exception.StackTrace);
            };
            //如果是ajax请求，返回相应的处理
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = JsonResultHelper.CreateJson(null, false, "系统内部错误，请联系管理员")
                };
                filterContext.ExceptionHandled = true;
            }
            //否则执行自带方法
            base.OnException(filterContext);
           
        }
    }
}