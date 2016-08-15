using LayIM.BLL;
using LayIM.BLL.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LayIM_SignalR_Chat.V1._0.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        [ActionName("users")]
        ///type 1 people 2 group
        public JsonResult UserInfo(string keyword = null, int type = 1, int pageindex = 1, int pagesize = 50)
        {
            var result = type == 1 ? LayimUserBLL.Instance.SearchLayImUsers(keyword, pageindex, pagesize) : ElasticGroup.Instance.SearchLayimGroup(keyword, pageindex, pagesize);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}