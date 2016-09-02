using LayIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayIM.DAL;
using LayIM.Utils.Extension;
using System.Data;
using LayIM.Utils.Single;
using LayIM.Utils.JsonResult;
using LayIM.Utils.Random;
using Macrosage.ElasticSearch.Core;
using Macrosage.ElasticSearch.Models;
using LayIM.Utils.Validate;
using LayIM.BLL.Group;
using LayIM.Model.Message;
using LayIM.Model.Enum;
using LayIM.Utils.Consts;
using Macrosage.ElasticSearch.Model;
using LayIM.Cache;

namespace LayIM.BLL
{
   public class LayimUserBLL
    {
        public static LayimUserBLL Instance
        {
            get
            {
                return SingleHelper<LayimUserBLL>.Instance;
            }
        }

        private LayimUserDAL _dal = new LayimUserDAL();

        

        #region 获取用户登录聊天室后的基本信息

        private  BaseListResult ToBaseListResult(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count ==0) {
                    return new BaseListResult();
                }
                //当前用户的信息
                var rowMine = ds.Tables[0].Rows[0];
                //用户组信息
                var rowFriendDetails = ds.Tables[2].Rows.Cast<DataRow>().Select(x => new GroupUserEntity
                {
                    id = x["uid"].ToInt(),
                    avatar = x["avatar"].ToString(),
                    groupid = x["gid"].ToInt(),
                    remarkname = x["remarkname"].ToString(),
                    username = x["nickname"].ToString(),
                    sign = x["sign"].ToString(),
                    //status之前的字段是为空的，现在我们把他的在线状态加上，IsOnline方法接收一个userid参数，从Redis缓存中读取该用户是否在线并返回
                    status = LayIMCache.Instance.IsOnline(x["uid"].ToInt()) ? "online" : "hide"
                }).OrderByDescending(x => x.status);//这里要根据用户是否在线这个字段排序，保证在线用户都在好友列表最上边
                //用户组信息，执行分组
                var friend = ds.Tables[1].Rows.Cast<DataRow>().Select(x => new FriendGroupEntity
                {
                    id = x["id"].ToInt(),
                    groupname = x["name"].ToString(),
                    online = 0,
                    list = rowFriendDetails.Where(f => f.groupid == x["id"].ToInt())
                });
                //群组信息
                var group = ds.Tables[3].Rows.Cast<DataRow>().Select(x => new GroupEntity
                {
                    id = x["id"].ToInt(),
                    groupname = x["name"].ToString(),
                    avatar = x["avatar"].ToString(),
                    groupdesc = x["groupdesc"].ToString()
                });

                BaseListResult result = new BaseListResult
                {
                    mine = new UserEntity
                    {
                        id = rowMine["id"].ToInt(),
                        avatar = rowMine["avatar"].ToString(),
                        sign = rowMine["sign"].ToString(),
                        username = rowMine["nickname"].ToString(),
                        status = "online"
                    },
                    friend = friend,
                    group = group
                };
                return result;
            }
            return null;
        }
        public JsonResultModel GetChatRoomBaseInfo(int userid)
        {
            if (userid == 0) { throw new ArgumentException("userid can't be zero"); }
            var ds = _dal.GetChatRoomBaseInfo(userid);
            var result = ToBaseListResult(ds);
            return JsonResultHelper.CreateJson(result, result != null);
        }
        #endregion

        #region 获取群组人员信息
        public JsonResultModel GetGroupMembers(int groupid)
        {
            var ds = _dal.GetGroupMembers(groupid);
            if (ds != null)
            {
                var rowOwner = ds.Tables[0].Rows[0];
                MembersListResult result = new MembersListResult
                {
                    owner = new UserEntity
                    {
                        id = rowOwner["userid"].ToInt(),
                        avatar = rowOwner["avatar"].ToString(),
                        username = rowOwner["username"].ToString(),
                        sign = rowOwner["sign"].ToString(),
                    },
                    list = ds.Tables[1].Rows.Cast<DataRow>().Select(x => new GroupUserEntity
                    {
                        id = x["userid"].ToInt(),
                        avatar = x["avatar"].ToString(),
                        groupid = groupid,
                        remarkname = x["remarkname"].ToString(),
                        sign = x["sign"].ToString(),
                        username = x["username"].ToString()
                    })
                };
                return JsonResultHelper.CreateJson(result);
            }
            return JsonResultHelper.CreateJson(null, false);
        }
        #endregion

        #region 用户登录或者注册流程
        /// <summary>
        /// 用户登陆或者注册，返回用户id如果为 0 说明密码错误
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public JsonResultModel UserLoginOrRegister(string loginName, string loginPwd,out int userid)
        {
            userid = 0;
            var wrongNameOrPwdFlag = -1;
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPwd))
            {
                return JsonResultHelper.CreateJson(new { userid = wrongNameOrPwdFlag });
            }
            string nickName = RandomHelper.getRandomName();
            var ds = _dal.UserLoginOrRegister(loginName, loginPwd, nickName);
            if (ds.Tables.Count > 1) {
                //这部分是注册成功后的信息，需要添加到查询表中
                Task.Run(() =>
                {
                    IndexUserInfo(ds.Tables[1]);
                });
               
            }
            userid = ds.Tables[0].Rows[0][0].ToInt();
            return JsonResultHelper.CreateJson(new { userid = userid });
        }
        #endregion

        #region 用户创建群
        public JsonResultModel CreateGroup(string groupName, string groupDesc, int userid)
        {
            var dt = _dal.CreateGroup(groupName, groupDesc, userid);
            if (dt != null && dt.Rows.Count == 1)
            {
                //同步ES库

                var group = ElasticGroup.Instance.IndexGroup(dt);
                var data = GetMessage(group);
                return JsonResultHelper.CreateJson(data, true);
            }
            else
            {
                return JsonResultHelper.CreateJson(null, false);
            }
        }

        private UserGroupCreatedMessage GetMessage(LayImGroup group)
        {
            return new UserGroupCreatedMessage
            {
                id = group.id.ToInt(),
                avatar = group.avatar,
                groupdesc = group.groupdesc,
                groupname = group.groupname,
                memebers = group.allcount,
                type = LayIMConst.LayIMGroupType
            };
        }
        #endregion

        #region ES部分，此部分包含查询逻辑，搜索部分都是从ES中搜索，其他都是从SQL中出数据
        private static Elastic<LayImUser> es
        {
            get
            {
                var _es = new Elastic<LayImUser>();
                _es.SetIndexInfo("layim", "layim_user");
                return _es;
            }
        }
        public bool IndexUserInfo(DataTable userDt)
        {
            if (userDt.Rows.Count > 0)
            {
                var row = userDt.Rows[0];
                var addtime = DateTime.Parse(row["addtime"].ToString());
                var user = new LayImUser()
                {
                    addtime = addtime,
                    avatar = row["avatar"].ToString(),
                    id = row["id"].ToString(),
                    im = row["im"].ToInt(),
                    nickname = row["nickname"].ToString(),
                    sign = row["signstr"].ToString(),
                    timestamp = addtime.ToTimestamp()
                };
                return IndexUserInfo(user);
            }
            return false;
        }
        public bool IndexUserInfo(LayImUser user) {
           bool result = es.Index(user);
            return result;
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="keyword">可以是IM号，或者昵称，或者男女，或者是否在线（目前仅支持IM号和昵称搜索）</param>
        /// <returns></returns>
        public JsonResultModel SearchLayImUsers(string keyword, int pageIndex = 1, int pageSize = 50)
        {

            var result = SearchUser(keyword, pageIndex, pageSize);

            return JsonResultHelper.CreateJson(result);
        }

        private BaseQueryEntity<LayImUser> SearchUser(string keyword, int pageIndex = 1, int pageSize = 50)
        {
            var hasvalue = ValidateHelper.HasValue(keyword);
            var from = (pageIndex - 1) * pageSize;
            //全部的时候按照省份排序
            string queryAll = "{\"query\":{\"match_all\":{}},\"from\":" + from + ",\"size\":" + pageSize + ",\"sort\":{\"province\":{\"order\":\"asc\"}}}";
            //按照关键字搜索的时候，默认排序，会把最接近在在最上边
            int im = hasvalue ? keyword.ToInt() : 0;
            //这里增加im是否为int类型判断，如果是int类型，那么可能是查询用户的IM号码，否则就是关键字查询
            string term = im == 0 ? "{\"im\":0}" : "{\"im\":" + keyword + "}";
            string queryWithKeyWord = "{\"query\":{\"filtered\":{\"filter\":{\"or\":[{\"term\":" + term + "},{\"query\":{\"match_phrase\":{\"nickname\":{\"query\":\"" + keyword + "\",\"slop\":0}}}}]}}},\"from\":" + from + ",\"size\":" + pageSize + ",\"sort\":{}}";
            string queryFinal = hasvalue ? queryWithKeyWord : queryAll;
            var result = es.QueryBayConditions(queryFinal);
            return result;
        }


        #endregion

        #region 获取用户有关的消息
        public JsonResultModel GetUserApplyMessage(int userid)
        {
            var dt = _dal.GetUserApplyMessage(userid);
            var list = dt.Rows.Cast<DataRow>().Select(x => new ApplyMessage
            {
                applyid = x["applyid"].ToInt(),
                addtime = x["addtime"].ToDateTime().ToString("yyyy/MM/dd HH:mm"),
                msg = x["msg"].ToString(),
                applyname = x["applyname"].ToString(),
                applyavatar = x["applyavatar"].ToString(),
                applyim = x["applyim"].ToInt(),
                targetid = x["targetid"].ToInt(),
                userid = x["userid"].ToInt(),
                result = x["result"].ToInt(),
                applytype = x["applytype"].ToInt(),
                isself = userid == x["userid"].ToInt()
            });
            return JsonResultHelper.CreateJson(list, true);
        }
        #endregion

        #region 获取某个用户的好友列表
        /// <summary>
        /// 获取某个用户的好友列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns>返回格式如下 ""或者 "10001,10002,10003"</returns>
        public string GetUserFriends(int userid)
        {
            //先读取缓存
            var friends = LayIMCache.Instance.GetUserFriendList(userid);
            //如果缓存中没有
            if (friends == "")
            {
                //从数据库读取，在保存到缓存中
                friends = _dal.GetUserFriends(userid);
                LayIMCache.Instance.SetUserFriendList(userid, friends);
            }
            return friends;
        }
        #endregion

        #region 读取历史纪录 根据条件 开始时间，结束时间  聊天关键字  组

        private ElasticChat eschat
        {
            get
            {
                var _es = new ElasticChat();
                _es.SetIndexInfo("layim", "chatinfo");

                return _es;
            }
        }

        public JsonResultModel SearchHistoryMsg(string groupId, DateTime? starttime = null, DateTime? endtime = null, string keyword = null, bool isfile = false, bool isimg = false, int pageIndex = 1, int pageSize = 20)
        {
            string st = starttime == null ? "" : starttime.Value.ToString("yyyy-MM-dd");
            string et = endtime == null ? "" : endtime.Value.ToString("yyyy-MM-dd");
            int from = (pageIndex - 1) * pageSize;
            //某个聊天组查询
            string queryGroup = "{\"query\": {\"match\": { \"roomid\": \"FRIEND_14895_14894\" }}}";
            //关键字查询
            string queryKeyWord = "{ \"query\": {\"match_phrase\": {\"content\": {\"query\": \"" + keyword + "\",\"slop\": 0} } }}";
            //是否图片 查询
            string queryImg = "{ \"term\": {\"isimg\": true }}";
            //是否包含文件查询
            string queryFile = "{ \"term\": {\"isfile\": true }}";
            //大于小于某个时间段查询
            string queryTimeRange = "{\"range\": {\"addtime\": { \"gt\": \"" + st + "\",\"lt\": \"" + et + "\" }} }";
            //大于某个时间
            string queryTimeRangeGt = "{\"range\": {\"addtime\": { \"gt\": \"" + st + "\"}} }";
            //小于某个时间
            string queryTimeRangeLt = "{\"range\": {\"addtime\": { \"lt\": \"" + et + "\" }} }";
            string queryAnd = queryGroup;
            if (starttime != null && endtime != null)
            {
                queryAnd += "," + queryTimeRange;
            }
            if (starttime != null)
            {
                queryAnd += "," + queryTimeRangeGt;
            }
            if (endtime != null)
            {
                queryAnd += "," + queryTimeRangeLt;
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                queryAnd += "," + queryKeyWord;
            }
            if (isfile)
            {
                queryAnd += "," + queryFile;
            }
            if (isimg)
            {
                queryAnd += "," + queryImg;
            }
            //最终查询语句
            string query = "  {\"query\": {\"filtered\": {\"filter\": {\"and\": [" + queryAnd + "] }}},\"from\": " + from + ",\"size\": " + pageSize + ",\"sort\": {\"addtime\": { \"order\": \"asc\"}},\"highlight\": {\"fields\": { \"content\": {}} }}";


            var result = eschat.QueryBayConditions(query);
            return JsonResultHelper.CreateJson(result, true);
        }
        #endregion

        #region 读取用户所在的群
        public string[] GetUserAllGroups(string userId)
        {
            var result = _dal.GetUserAllGroups(userId);
            return result;
        }
        #endregion
    }
}
