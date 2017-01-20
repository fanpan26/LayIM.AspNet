using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LayIM_RongCloud_Chat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //获取token
            routes.MapRoute(
               name: "gettoken",
               url: "token",
               defaults: new { controller = "IM", action = "getToken" }
           );
            //获取用户基础书
            routes.MapRoute(
               name: "base",
               url: "base",
               defaults: new { controller = "Data", action = "GetUserBaseList" }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
