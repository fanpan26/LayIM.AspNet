using LayIM.Container;
using LayIM.LogicLayer;
using LayIM.Model;
using LayIM.Service;
using LayIM_RongCloud_Chat.Filter;
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
        private UserService service = new UserService();
        #region 基本信息
        [UserAuthorize]
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
        #endregion

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public JsonResult Login(string loginName, string password)
        {
           
            var model = service.LoginOrRegister(loginName, password);
            return Json(model, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 添加消息
        /// <summary>
        /// 数据库添加消息
        /// </summary>
        /// <param name="userId">消息发送者</param>
        /// <param name="to">消息接收者</param>
        /// <param name="type">发送类型 friend 单聊 group 群聊</param>
        /// <param name="content">发送内容</param>
        /// <returns></returns>
        public JsonResult AddMsg(int userId, int to, string type, string content)
        {
            var service = LayIMGlobalServiceContainer.GlobalContainer.Resolve<IMessageService>();
            bool result = service.AddMsg(userId, to, type, content);
            return Json(new JsonResultModel { code = result ? JsonResultType.Success : JsonResultType.Failed }, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 搜索
        /// <summary>
        /// 搜索好友
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public JsonResult SearchUser(string name, int pageindex, int pagesize)
        {
            var service = LayIMGlobalServiceContainer.GlobalContainer.Resolve<ISearchService>();
            var result = service.SearchUser(name, pageindex, pagesize);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索群组
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public JsonResult SearchGroup(string name, int pageindex, int pagesize)
        {
            var service = LayIMGlobalServiceContainer.GlobalContainer.Resolve<ISearchService>();
            var result = service.SearchGroup(name, pageindex, pagesize);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 申请

        [UserAuthorize]
        public JsonResult ApplyFriend(int userId,int targetId,string other=null)
        {
            var result = service.ApplyToFriend(userId, targetId, other);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [UserAuthorize]
        public JsonResult ApplyGroup(int userId, int targetId,int gid, string other = null)
        {
            var result = service.ApplyToGroup(userId, targetId,gid, other);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [UserAuthorize]
        public JsonResult GetApplyList(int userId)
        {
           var result = service.GetApplyByUserId(userId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [UserAuthorize]
        public JsonResult HandleFriendApply(int userId, int friendId, int groupId=0)
        {
            var result = service.AgreeOrDenyFriend(userId, friendId, groupId);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #endregion
    }
}