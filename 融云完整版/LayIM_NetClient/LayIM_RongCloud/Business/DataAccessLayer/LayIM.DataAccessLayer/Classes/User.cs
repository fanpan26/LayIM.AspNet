using Dapper;
using LayIM.DataAccessLayer.Helper;
using LayIM.DataAccessLayer.Interface;
using LayIM.Lib;
using LayIM.Model;
using LayIM.Model.Message.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayIM.Model.ViewModel;
using LayIM.Model.Enum;

namespace LayIM.DataAccessLayer.Classes
{
    public class User : IUser
    {
        private IMultipleHandler<BaseListResult> _handler;
        public User(IMultipleHandler<BaseListResult> handler)
        {
            _handler = handler;
        }


        /// <summary>
        /// 获取基本用户数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BaseListResult GetUserBase(int userId)
        {
            const string spName = "[Proc_LayIM_GetUserInitInfo_RC]";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@userid", userId, DbType.Int32);
            if (_handler == null)
            {
                throw new ArgumentOutOfRangeException("IMultipleHandler<BaseListResult> 服务未注册");
            }
            return DBHelper.QueryMultiple(spName, parameter, CommandType.StoredProcedure, _handler);

            #region 使用Func回调返回
            //return DBHelper.QueryMultiple(spName, parameter, CommandType.StoredProcedure, reader =>
            //{
            //    var result = new BaseListResult();
            //    //用户本人数据
            //    result.mine = reader.ReadFirstOrDefault<UserEntity>();
            //    //处理friend逻辑 start
            //    var friend = reader.Read<FriendGroupEntity>();
            //    var groupUsers = reader.Read<GroupUserEntity>();
            //    friend.ToList().ForEach(f =>
            //    {
            //        //每一组的人分配给各个组
            //        f.list = groupUsers?.Where(x => x.groupid == f.id);
            //    });
            //    result.friend = friend;
            //    //处理friend逻辑 end
            //    //读取用户所在群
            //    result.group = reader.Read<GroupEntity>();
            //    return result;
            //});
            #endregion
        }

        /// <summary>
        /// 用户登录逻辑
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public JsonResultModel LoginOrRegister(string loginName, string passWord)
        {
            passWord = passWord.ToMD5();

            string sql = "select id as UserId,loginname as LoginName,loginpwd as PassWord from layim_user where loginname=@loginname";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@loginname", loginName);

            var user = DBHelper.Query<LoginUser>(sql, parameters, CommandType.Text).FirstOrDefault();
            //该用户名存在
            if (user?.UserId > 0)
            {
                bool valid = user.PassWord == passWord;
                return new JsonResultModel
                {
                    code = valid ? JsonResultType.Success : JsonResultType.Failed,
                    data = user?.UserId ,
                    msg = valid ? "验证成功" : "密码有误"
                };
            }
            else {
                //不存在，就注册该用户
                sql = "INSERT INTO dbo.layim_user (loginname,loginpwd,nickname,headphoto) VALUES (@loginname,@pwd,@nick,@head);select @@identity";
                parameters.Add("@pwd", passWord);
                parameters.Add("@nick", "注册用户" + DateTime.Now.ToString("HHmmss"));
                parameters.Add("@head", "/images/default.jpg");
                int newUserId = DBHelper.ExecuteScalar<int>(sql, parameters);
                if (newUserId > 0) {
                    //插入一个默认分组
                    Task.Run(() =>
                    {
                        AddDefaultFriendGroup(newUserId);
                    });
                   
                }
                return new JsonResultModel
                {
                    code = newUserId > 0 ? JsonResultType.Success : JsonResultType.Failed,
                    data = newUserId,
                    msg = newUserId > 0 ? "注册成功" : "注册失败"
                };
            }
        }

        private bool AddDefaultFriendGroup(int userId)
        {
            string sql = "INSERT INTO dbo.layim_friend_group (name,belonguid,addtime,sort,issystem) VALUES  ('我的好友'  ," + userId + " ,GETDATE() ,0  ,0 )";
            return DBHelper.Execute(sql, null);
        }

        #region 搜索
        public PageView<LayIMGroup> SearchGroup(string name = null, int pageindex = 1, int pagesize = 20)
        {
            bool hasCondition = string.IsNullOrEmpty(name);
            return PageHelper.GetPageList<LayIMGroup>(new PageSearchOptions
            {
                PageIndex = pageindex,
                PageSize = pagesize,
                Condition = hasCondition ? "" : " name like '%" + name + "%'",
                Fields = "id,name,headphoto AS photo,groupdesc AS gdesc,groupowner as owner,addtime",
                PrimaryKey = "id",
                Sort = "addtime desc",
                TableName = "layim_group"
            });
        }

        public PageView<LayIMUser> SearchUser(string name = null, int pageindex = 1, int pagesize = 20)
        {
            bool hasCondition = !string.IsNullOrEmpty(name);
            return PageHelper.GetPageList<LayIMUser>(new PageSearchOptions
            {
                PageIndex = pageindex,
                PageSize = pagesize,
                Condition = hasCondition ? " nickname like '%" + name + "%'" : "",
                Fields = "id,nickname as name,headphoto as photo,sign,addtime",
                PrimaryKey = "id",
                Sort = "addtime desc",
                TableName = "layim_user"
            });
        }
        #endregion

        #region 添加申请

        public JsonResultModel ApplyToFriend(int applyUserId, int targetId, string other = null)
        {
            if (applyUserId == targetId)
            {
                return new JsonResultModel
                {
                    code = JsonResultType.Failed,
                    msg = "不能加自己为好友"
                };
            }
            //判断是否为好友
            bool isfriend = IsFriendRelation(applyUserId, targetId);
            if (isfriend)
            {
                return new JsonResultModel
                {
                    code = JsonResultType.Failed,
                    msg = "对方已经是你的好友"
                };
            }
            bool result = AddApply(new NotifyMessage
            {
                AddTime = DateTime.Now.ToTimeStamp(),
                FromUserId = applyUserId,
                GroupId = 0,
                Other = other,
                ToUserId = targetId,
                Type = NotifyType.ApplyToFriend
            });
            return new JsonResultModel
            {
                code = result ? JsonResultType.Success : JsonResultType.Failed
            };
        }

        public JsonResultModel ApplyToGroup(int applyUserId, int targetId, int gid, string other = null)
        {
            //判断是否为好友
            bool isgroup = IsInGroup(applyUserId, targetId);
            if (isgroup)
            {
                return new JsonResultModel
                {
                    code = JsonResultType.Failed,
                    msg = "你已经是群成员了"
                };
            }
            bool result = AddApply(new NotifyMessage
            {
                AddTime = DateTime.Now.ToTimeStamp(),
                FromUserId = applyUserId,
                GroupId = gid,
                Other = other,
                ToUserId = targetId,
                Type = NotifyType.ApplyToGroup
            });
            return new JsonResultModel
            {
                code = result ? JsonResultType.Success : JsonResultType.Failed
            };
        }

        /// <summary>
        /// 私有方法，通知
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool AddApply(NotifyMessage message)
        {
            //1申请为好友  2 申请加群  3用户拒绝成为好友 4 用户同意成为好友 5管理员同意加群 6 管理员拒绝加群
            string sql = @"INSERT INTO dbo.layim_apply_new(fromuid, touid, type, other, addtime, groupid, isread, operate, updatetime) 
                           VALUES(@fuid,@touid,@type,@other,@addtime,@groupid,@isread,@operate,@updatetime)";
            var parameters = new
            {
                fuid = message.FromUserId,
                touid = message.ToUserId,
                other = message.Other,
                addtime = message.AddTime,
                groupid = message.GroupId,
                isread = message.IsRead,
                operate = message.OperateResult,
                updatetime = message.UpdateTime,
                type = (int)message.Type
            };
            return DBHelper.Execute(sql, parameters);
        }
        //const int ApplyTypeFriend = 0;
        //const int ApplyTypeGroup = 1;
        //public JsonResultModel ApplyToFriend(int applyUserId, int targetId, string other = null)
        //{
        //    if (applyUserId == targetId)
        //    {
        //        return new JsonResultModel
        //        {
        //            code = JsonResultType.Failed,
        //            msg = "不能加自己为好友"
        //        };
        //    }
        //    //判断是否为好友
        //    bool isfriend = IsFriendRelation(applyUserId, targetId);
        //    if (isfriend)
        //    {
        //        return new JsonResultModel
        //        {
        //            code = JsonResultType.Failed,
        //            msg = "对方已经是你的好友"
        //        };
        //    }
        //    //添加申请
        //    //添加申请
        //    return AddApply(ApplyTypeFriend, applyUserId, targetId, other);
        //}

        //public JsonResultModel ApplyToGroup(int applyUserId, int targetId,string other = null)
        //{
        //    //判断是否为好友
        //    bool isgroup = IsInGroup(applyUserId, targetId);
        //    if (isgroup)
        //    {
        //        return new JsonResultModel
        //        {
        //            code = JsonResultType.Failed,
        //            msg = "你已经是群成员了"
        //        };
        //    }
        //    //添加申请
        //    return AddApply(ApplyTypeGroup, applyUserId, targetId, other);
        //}

        #endregion

        #region 添加申请-私有
        private JsonResultModel AddApply(int type, int applyUserId, int targetId, string other = null)
        {
            string sql = "INSERT INTO dbo.layim_apply (userid,applytype,targetid,applytime,other,result) VALUES(@uid, " + type + ", @targetid, GETDATE(), @other, 0)";
            bool result = DBHelper.Execute(sql, new
            {
                uid = applyUserId,
                targetid = targetId,
                other = other
            });
            return new JsonResultModel { code = result ? JsonResultType.Success : JsonResultType.Failed };
        }
      
        /// <summary>
        /// 判断是否为好友关系
        /// </summary>
        /// <param name="applyUserId"></param>
        /// <param name="targetUserId"></param>
        /// <returns></returns>
        private bool IsFriendRelation(int applyUserId, int targetUserId)
        {
            string sql = "SELECT COUNT(1) FROM dbo.layim_friend_group A LEFT JOIN dbo.layim_friend_group_detail B ON A.id=  B.gid WHERE A.belonguid=@buid AND [uid]=@uid";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@buid", applyUserId);//申请人
            parameters.Add("@uid", targetUserId);  //被申请人 判断是否已经是好友
            int fcount = DBHelper.ExecuteScalar<int>(sql, parameters);
            return fcount > 0;
        }

        /// <summary>
        /// 判断是否为群里的组员
        /// </summary>
        /// <param name="applyUserId"></param>
        /// <param name="targetGroupId"></param>
        /// <returns></returns>
        private bool IsInGroup(int applyUserId, int targetGroupId)
        {
            string sql = " SELECT COUNT(1) FROM dbo.layim_group A LEFT JOIN dbo.layim_group_detail B ON A.id=B.gid WHERE [uid]=@uid AND a.id=@gid";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@uid", applyUserId);//申请人
            parameters.Add("@gid", targetGroupId);  //被申请人 判断是否已经是好友
            int fcount = DBHelper.ExecuteScalar<int>(sql, parameters);
            return fcount > 0;
        }
        #endregion

        #region 获取申请列表
        public IEnumerable<NotifyMessage> GetApplyList(int userId)
        {
            string sql = @"SELECT A.id as Id, A.fromuid as FromUserId,U.nickname as UserName,U.headphoto as Avatar,A.[type] as Type,A.other as Other,A.operate as OperateResult,A.addtime as AddTime,A.groupid as GroupId,G.name as GroupName FROM dbo.layim_apply_new A LEFT JOIN dbo.layim_user U ON A.fromuid=U.id LEFT JOIN dbo.layim_group G ON A.groupid=G.id WHERE A.touid=@touid";
            DynamicParameters param = new DynamicParameters();
            param.Add("@touid", userId);
            return DBHelper.Query<NotifyMessage>(sql, param);
        }

        /// <summary>
        /// 是否同意成为好友
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <param name="isAgree"></param>
        /// <returns></returns>
        public JsonResultModel AgreeOrDenyFriend(int userId, int friendId, int groupId = 0)
        {
            bool isAgree = groupId > 0;
            string sql = "UPDATE dbo.layim_apply_new SET operate=@operate,updatetime=@updatetime WHERE fromuid=@touid AND touid=@fuid;";
            var parameters = new DynamicParameters();
            if (isAgree)
            {
                sql += " if not exists (select id from dbo.layim_friend_group_detail where gid=@gid and uid=@touid) begin INSERT INTO dbo.layim_friend_group_detail (gid,uid,addtime,gname) values(@gid,@touid,GETDATE(),'') end";
                parameters.Add("@gid", groupId);
            }
          
            parameters.Add("@operate", isAgree ? 1 : 2);
            parameters.Add("@fuid", userId);
            parameters.Add("@touid", friendId);
            parameters.Add("@updatetime", DateTime.Now.ToTimeStamp());
            bool updateResult = DBHelper.Execute(sql, parameters);

            if (updateResult)
            {
                //给对方发送通知
                bool result = AddApply(new NotifyMessage
                {
                    AddTime = DateTime.Now.ToTimeStamp(),
                    FromUserId = userId,
                    ToUserId = friendId,
                    OperateResult = 0,
                    UpdateTime = 0,
                    Type = isAgree ? NotifyType.AgreeToFriend : NotifyType.DenyToFriend
                });
                return new JsonResultModel { code = result ? JsonResultType.Success : JsonResultType.Failed, msg = result ? "" : "添加好友失败" };
            }
            else
            {
                return new JsonResultModel { code = JsonResultType.Failed, msg = "添加好友失败" };
            }
        }
        #endregion
    }
}
