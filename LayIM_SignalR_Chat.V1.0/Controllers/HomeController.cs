using LayIM.BLL;
using LayIM.Utils.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LayIM_SignalR_Chat.V1._0.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public JsonResult UserLogin(string username, string password)
        {
            int userid = 0;
            var result = LayimUserBLL.Instance.UserLoginOrRegister(username, password, out userid);
            var context = HttpContext.Response;
            if (userid > 0)
            {
                context.Cookies.Add(new HttpCookie(LayIMConst.LayIM_SignalR_UserId) { Value = userid.ToString() });
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        public ActionResult ChatHistory()
        {
            return View();
        }
        public ActionResult Find()
        {
            return View();
        }
        public ActionResult HandleMessage()
        {
            return View();
        }
        public ActionResult CreateGroup()
        {
            return View();
        }
    }
}