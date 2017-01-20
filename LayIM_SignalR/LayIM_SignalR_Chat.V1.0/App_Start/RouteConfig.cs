using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LayIM_SignalR_Chat.V1._0
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

          
            routes.MapRoute(
             name: "login",
             url: "userlogin",
             defaults: new { controller = "Home", action = "UserLogin"}
         );
            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );
        }
    }
}
