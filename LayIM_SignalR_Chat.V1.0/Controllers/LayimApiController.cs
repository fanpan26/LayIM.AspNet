using LayIM.BLL;
using LayIM.ChatServer.Hubs;
using LayIM.Model.Enum;
using LayIM.Model.Message;
using LayIM.Utils.Consts;
using LayIM.Utils.Extension;
using LayIM.Utils.FileUpload;
using LayIM.Utils.JsonResult;
using LayIM_SignalR_Chat.V1._0.Filters;
using LayIM_SignalR_Chat.V1._0.HubServer;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LayIM_SignalR_Chat.V1._0.Controllers
{
    public class LayimApiController : Controller
    {
        // GET: Layim
        public ActionResult Index()
        {
            return View();
        }

        #region 获取基本信息和群信息
        [HttpGet]
        [ActionName("base")]
        /// <summary>
        /// 获取基本列表 layimapi/base
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JsonResult GetBaseList(int userid)
        {
            var result = LayimUserBLL.Instance.GetChatRoomBaseInfo(userid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("member")]
        /// <summary>
        /// 获取群组员信息
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public JsonResult GetMembersList(int id)
        {
            var result = LayimUserBLL.Instance.GetGroupMembers(id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 上传文件和图片

        [HttpPost]
        [ActionName("upload_img")]
        public JsonResult UploadImg(HttpPostedFileBase file)
        {
            return Json(FileUploadHelper.Upload(file, Server.MapPath("/upload/"), true), JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        [ActionName("upload_file")]
        public JsonResult UploadFile(HttpPostedFileBase file)
        {
            return Json(FileUploadHelper.Upload(file, Server.MapPath("/upload/"), false), JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region 好友或者用户群组申请
        [HttpPost]
        [ActionName("apply")]
        public JsonResult AddFriendOrJoinInGroup(int userid, int targetid, string other = "", bool isfriend = true)
        {
            if (userid == targetid) { throw new ArgumentException("userid can't equal with targetid"); }
            var result = LayIMUserJoinBLL.Instance.AddApply(userid, targetid, other, isfriend);
            string[] toUserIds = result.data as string[];
            if (toUserIds.Length > 0)
            {
                string userIdsStr = string.Join(",", toUserIds);
                //给对方发送好友消息 传targetid
                HubServerHelper.SendMessage(new ApplySendedMessage { msg = isfriend ? "您有1条加好友消息" : "您有1条加群消息" }, userIdsStr, ChatToClientType.ApplySendedToClient, !isfriend);

            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 获取需要我处理的好友请求
        [HttpGet]
        [ActionName("myapply")]
        public JsonResult GetUserNeedHandleApply(int userid)
        {
            var result = LayIMUserJoinBLL.Instance.GetUserNeedHandleApply(userid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 创建用户群
        [HttpPost]
        [ActionName("create")]
        public JsonResult CreateGroup(string n, string d, int uid)
        {
            var result = LayimUserBLL.Instance.CreateGroup(n, d, uid);
            var group = result.data as UserGroupCreatedMessage;
            //向客户端推送创建成功的消息
            HubServerHelper.SendMessage(group,uid.ToString(), ChatToClientType.GroupCreatedToClient);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 获取用户申请消息
        [HttpGet]
        [ActionName("msg")]
        public JsonResult GetUserApplyMessage(int userid)
        {
           var result = LayimUserBLL.Instance.GetUserApplyMessage(userid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 处理用户的请求
        [HttpPost]
        [ActionName("handle")]
        public JsonResult HandleApply(int applyid, int userid, short result, string reason="")
        {
            var res = LayIMUserJoinBLL.Instance.HandleApply(applyid, userid, result, reason);
            var msg = res.data as ApplyHandledMessgae;
            //推送消息
            HubServerHelper.SendMessage(msg);
            return Json(res, JsonRequestBehavior.DenyGet);
        }
        #endregion
    }
}