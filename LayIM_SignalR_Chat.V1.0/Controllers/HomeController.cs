using LayIM.BLL;
using LayIM.Cache;
using LayIM.Utils.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LayIM_SignalR_Chat.V1._0.Controllers
{
    //from home
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexV3()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public  async Task<JsonResult> UserLogin(string username, string password)
        {
            int userid = 0;
            var result = LayimUserBLL.Instance.UserLoginOrRegister(username, password, out userid);
            var context = HttpContext.Response;
            if (userid > 0)
            {
              await LayIMCache.Instance.CacheUserAfterLogin(userid);
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        //
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

        /// <summary>
        /// V3.0提供的模板 消息盒子
        /// </summary>
        /// <returns></returns>
        public ActionResult MsgBox()
        {
            return View();
        }
        public ActionResult CreateGroup()
        {
            return View();
        }
    }
}