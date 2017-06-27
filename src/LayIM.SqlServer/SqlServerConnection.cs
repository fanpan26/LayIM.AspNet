using Dapper;
using LayIM.NetClient;
using LayIM.SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayIM.NetClient.Model;

namespace LayIM.SqlServer
{
    /// <summary>
    /// 此类为 LayIM  逻辑的真正实现。所有的上层调用都会调用到这里
    /// </summary>
    public  class SqlServerConnection : LayimStorageConnection
    {
        private SqlServerStorage _storage;

        public SqlServerConnection(SqlServerStorage storage)
        {
            _storage = storage;
        }

        #region 返回添加成功后的主键方法
        /// <summary>
        /// 返回添加后的主键
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns>返回添加成功后的主键</returns>
        private Task<long> ExecuteReturnIdAsync(string sql, object param)
        {
            bool last = sql.LastIndexOf(';') > -1;

            sql += $"{(last ? "" : ";")} SELECT SCOPE_IDENTITY()";
            return  _storage.UseConnectionAsync(async connection =>
            {
                long Id = await connection.ExecuteScalarAsync<long>(sql, param);
                return Id;
            });
        }
        #endregion

        #region 返回用户基本信息
        /// <summary>
        /// 获取用户基本信息 好友列表等
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override CommonResult GetInitInfo(long userId)
        {
            return _storage.UseConnection<CommonResult>(connection =>
            {

                string sql = @"SELECT pk_id AS id,name AS username,[sign],avatar FROM dbo.[user] WHERE pk_id=@uid
SELECT pk_id AS id,name AS groupname FROM dbo.friend_group WHERE [user_id]=@uid
SELECT a.pk_id AS gid,c.pk_id AS id,c.name as username,c.[sign],c.avatar FROM dbo.friend_group A INNER JOIN dbo.friend_group_detail B ON a.pk_id=B.group_id LEFT JOIN dbo.[user] C ON B.[user_id]=c.pk_id WHERE a.[user_id]=@uid
  SELECT b.pk_id AS id,name AS groupname,avatar FROM dbo.big_group_detail A LEFT JOIN dbo.big_group B ON A.group_id=b.pk_id WHERE [user_id]=@uid";

                SqlMapper.GridReader reader = connection.QueryMultiple(sql, new { uid = userId });

                var result = new BaseListResult();
                result.mine = reader.ReadFirstOrDefault<UserModel>();
                //处理friend逻辑 start
                var friend = reader.Read<FriendGroupModel>();
                var groupUsers = reader.Read<GroupUserModel>();

                friend.ToList().ForEach(f =>
                {
                    //每一组的人分配给各个组
                    f.list = groupUsers?.Where(x => x.gid == f.id);
                });
                result.friend = friend;
                //处理friend逻辑 end
                //读取用户所在群
                result.group = reader.Read<BigGroupModel>();


                return new CommonResult { code = result.mine == null ? 1 : 0, msg = result.mine == null ? "用户不存在" : "", data = result };
            });
        }
        #endregion

        #region 获取群成员

        public override CommonResult GetGroupMembers(long groupId)
        {
            return _storage.UseConnection<CommonResult>(connection =>
            {
                GroupMemberResult result = new GroupMemberResult();
                string sql = $@"SELECT pk_id AS id,name AS username,[sign],avatar FROM dbo.[user] WHERE pk_id=(SELECT owner_id FROM dbo.big_group WHERE pk_id={groupId})
SELECT b.pk_id AS id,name AS username,B.[sign],B.avatar FROM dbo.big_group_detail A LEFT JOIN dbo.[user] B ON A.[user_id]=b.pk_id WHERE group_id={groupId}";
                SqlMapper.GridReader reader = connection.QueryMultiple(sql);

                result.owner = reader.Read<UserModel>().FirstOrDefault();
                result.list = reader.Read<UserModel>();

                return new CommonResult { code = 0, data = result, msg = "ok" };
            });
        }
        #endregion

        #region 获取历史消息记录
        public override IEnumerable<LayimChatMessageViewModel> GetHistoryMessages(LayimHistoryParam param)
        {
            return _storage.UseConnection<IEnumerable<LayimChatMessageViewModel>>(connection =>
            {
                string timestampCondition = param.MsgTimestamp > 0 ? "AND A.[timestamp]<" + param.MsgTimestamp : "";

                var sql = $@"SELECT TOP {param.Page}  A.[user_id] AS [uid] ,
        A.msg AS content ,
        A.create_at AS addtime ,
        A.[timestamp] ,
        B.name ,
        B.avatar
FROM    dbo.chat_msg A
        LEFT JOIN dbo.[user] B ON A.[user_id] = B.pk_id WHERE A.room_id='{CreateRoom(param.UserId, param.ToId, param.Type)}' {timestampCondition} ORDER BY A.[timestamp] desc";

                var res = connection.Query<LayimChatMessageViewModel>(sql).ToList();
                res.ForEach(x => x.self = x.uid == param.UserId);
                return res.OrderBy(x => x.timestamp);
            });
        }
        #endregion

        #region 添加聊天记录
        /// <summary>
        /// 添加聊天消息记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override bool AddChatMsg(LayimChatMessageModel msg)
        {
            if (msg==null||msg.FromUserId == 0 || msg.ToUserId == 0) { return false; }

            return _storage.UseConnection<bool>(connection =>
            {
                string sql = @"INSERT INTO dbo.chat_msg( pk_id , user_id ,room_id ,msg ,create_at,timestamp ) VALUES  ( @msgid , @uid ,  @roomid ,@msg , @create,@timestamp)";

                var result = connection.Execute(sql,
                    new
                    {
                        msgid = Guid.NewGuid().ToString(),
                        uid = msg.FromUserId,
                        roomid = CreateRoom(msg.FromUserId, msg.ToUserId, msg.Type),
                        msg = msg.Message,
                        create = msg.CreateAt,
                        timestamp = msg.TimeStamp
                    });
                return result > 0;
            });
        }
        #endregion
         
        #region 创建群组
        public override async Task<CommonResult> CreateGroup(long userId, string groupName, string groupDesc, string groupAvatar)
        {
            return await _storage.UseConnectionAsync(async connection =>
            {
                var sql = "INSERT INTO dbo.big_group( name ,avatar ,[description] ,owner_id ) VALUES(@name, @avatar, @desc, @uid);";

                long groupId = await ExecuteReturnIdAsync(sql, new { name = groupName, avatar = groupAvatar, desc = groupDesc, uid = userId });

                if (groupId > 0)
                {
                    var userSql = "INSERT INTO dbo.big_group_detail(group_id, [user_id]) VALUES(@gid, @uid)";

                    var result = await connection.ExecuteAsync(userSql, new { uid = userId, gid = groupId });
                    return CreateResult(result > 0);
                }
                return CreateResult(false);
            });
        }
        #endregion

        #region 创建用户

        public override async Task<CommonResult> CreateUser(string userName, string userSign, string userAvatar)
        {
            return await _storage.UseConnectionAsync(async connection =>
            {
                var sql = "INSERT INTO dbo.[user]( name, [sign], avatar) VALUES  (@name,@sign,@avatar);";
                long userId = await ExecuteReturnIdAsync(sql, new { name = userName, avatar = userAvatar, sign = userSign });

                string friendGroupSql = "INSERT INTO dbo.friend_group([user_id], name )VALUES(@uid, '我的好友')";

                var result = await connection.ExecuteAsync(friendGroupSql, new { uid = userId });

                return CreateResult(result > 0);
            });
        }
        #endregion

        #region 创建加好友申请

        public override async Task<CommonResult> CreateUserApply(long fromUserId, long toUserId, string other)
        {
            const int typeUserApply = 0;
            //判断是否已经是好友
            bool isFriend = await IsFriend(fromUserId, toUserId);
            if (isFriend)
            {
                return new CommonResult { code = 1, msg = "已经是好友了" };
            }

            int result = await CreateApply(fromUserId, toUserId, typeUserApply, 0, other);
            return CreateResult(result > 0);
        }

        public override async Task<CommonResult> GetApplyCount(long userId)
        {
            return await _storage.UseConnectionAsync<CommonResult>(async connection =>
            {
                var sql = "SELECT apply_count FROM dbo.apply_summary WHERE [user_id]=@uid";
                int count = await connection.ExecuteScalarAsync<int>(sql, new { uid = userId });
                return new CommonResult { code = 0, data = new { apply = count } };
            });
        }
        #endregion

        #region 是否已经是好友
        private async Task<bool> IsFriend(long userId, long friendId)
        {
            return await _storage.UseConnectionAsync<bool>(async connection =>
            {
                var sql = "SELECT COUNT([user_id]) FROM dbo.friend_group_detail WHERE group_id IN (SELECT pk_id FROM dbo.friend_group WHERE [USER_ID]=@uid) AND [user_id]=@fid";
                ushort count = await connection.ExecuteScalarAsync<ushort>(sql, new { uid = userId, fid = friendId });
                return count > 0;
            });
        }
        #endregion

        #region 是否在群组内
        private async Task<bool> IsInGroup(long userId, long groupId)
        {
            return await _storage.UseConnectionAsync<bool>(async connection =>
            {
                var sql = "SELECT count(*) FROM dbo.big_group_detail WHERE [user_id]=@uid AND group_id=@gid";
                ushort count = await connection.ExecuteScalarAsync<ushort>(sql, new { uid = userId, gid = groupId });
                return count > 0;
            });
        }
        #endregion

        #region 创建加群申请
        public override async Task<CommonResult> CreateGroupApply(long fromUserId, long toGroupId, string other)
        {
            const int typeGroupApply = 1;

            bool isInGroup = await IsInGroup(fromUserId, toGroupId);

            if (isInGroup) {
                return new CommonResult { code = 1, msg = "已经是群成员了" };
            }

            var ownerId = await GetUserIdByGroupId(toGroupId);

            int result = await CreateApply(fromUserId, ownerId, typeGroupApply, toGroupId, other);
            return CreateResult(result > 0);
        }
        #endregion

        #region 通过或者拒绝操作
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="result">1同意 2 拒绝</param>
        /// <returns></returns>
        public override async Task<CommonResult> HandleApply(long id, long userId, int result, long uidGroup = 0)
        {
            return await _storage.UseConnectionAsync<CommonResult>(async connection =>
            {
                var sql = @"DECLARE @type INT ,
    @gid BIGINT ,
    @uid BIGINT;
SELECT  @type = [type] ,
        @gid = group_id ,
        @uid = apply_uid
FROM    dbo.[apply]
WHERE   pk_id = @id
        AND apply_tid = @tid;

IF @type = 0
    OR @type = 1
    BEGIN
        UPDATE  dbo.[apply]
        SET     result = @result
        WHERE   pk_id = @id;
        SELECT  @type as type ,
                @gid as groupId,
                @uid as userId;
    END;
ELSE
    BEGIN
        SELECT  -1 as  type ,
                0 as groupId,
                0 as userId;
    END;";
                var appInfo = (await connection.QueryAsync<ApplyInfo>(sql, new { id = id, tid = userId, result = result })).FirstOrDefault();

                if (appInfo.Type == -1)
                {
                    return new CommonResult { code = 1, msg = "申请不存在" };
                }
                else
                {
                    if (result == 1)
                    {
                        HandleApplyUser(appInfo, userId, uidGroup);
                    }
                }
                return new CommonResult { code = 0 };
            });
        }

        private void HandleApplyUser(ApplyInfo info, long userId,long uidGroup = 0)
        {
            if (info.Type == 0)
            {
                //好友申请
                AddFriend(userId, info.UserId, uidGroup);
            }
            if (info.Type == 1)
            {
                //群组申请
                AddGroupUser(info.GroupId, info.UserId);
            }
        }

        private void AddFriend(long uid, long friendUid, long uidGroup)
        {
            var sqlUserId = "INSERT INTO dbo.friend_group_detail( group_id, [user_id] ) VALUES (@uidGroup,@friendUid);";
            var sqlFriendUserId = "INSERT INTO dbo.friend_group_detail( group_id, [user_id] ) VALUES ((SELECT TOP 1 pk_id from dbo.friend_group WHERE [user_id] =@friendUid),@uid)";

            _storage.UseConnection(connection =>
            {
                connection.Execute(sqlUserId + sqlFriendUserId, new { uidGroup = uidGroup, friendUid = friendUid, uid = uid });
            });
        }
        private void AddGroupUser(long groupId, long userId)
        {
            _storage.UseConnection(connection =>
            {
                var sql = "INSERT INTO dbo.big_group_detail (group_id, user_id) VALUES (@gid,@uid)";

                connection.Execute(sql, new { gid = groupId, uid = userId });
            });
        }
        #endregion

        #region 获取申请详情列表
        private string GetApplyListSQL()
        {
            
            #region sql
                var sql = @"SELECT TOP 100
        X.pk_id AS id ,
        X.apply_uid AS [uid] ,
        X.[type] AS [type] ,
        X.group_id AS gid ,
        X.other AS other ,
        X.create_at AS addtime ,
        name AS name ,
        X.avatar AS avatar ,
        X.[sign] as [sign],
        result AS result,[self],X.groupname
FROM    ( SELECT    A.pk_id ,
                    A.apply_uid ,
                    A.[type] ,
                    A.group_id ,
                    A.other ,
                    A.create_at ,
                    B.name ,
                    B.avatar ,
                    B.[sign],
                    A.result,
                    1 as [self],c.name AS groupname
          FROM      dbo.[apply] A
                    LEFT JOIN dbo.[user] B ON A.apply_uid = B.pk_id
                    LEFT JOIN dbo.big_group C ON A.group_id=C.pk_id
          WHERE    apply_tid = @uid
          UNION ALL
          SELECT    A.pk_id ,
                    A.apply_tid as apply_uid,
                    A.[type] ,
                    A.group_id ,
                    A.other ,
                    A.create_at ,
                    B.name ,
                    B.avatar ,
                    B.[sign],
                    A.result,
                    0 as [self],
                    c.name AS groupname
          FROM      dbo.[apply] A
                    LEFT JOIN dbo.[user] B ON A.apply_tid = B.pk_id
                    LEFT JOIN dbo.big_group C ON A.group_id=c.pk_id
          WHERE     result > 0
                    AND A.apply_uid = @uid
        ) X
ORDER BY X.create_at DESC;";
            #endregion
            return sql;
        }
        #endregion

        #region 更新已读
        private bool UpdateRead(long userId)
        {
            return _storage.UseConnection(connection =>
            {
                string sql = "DELETE FROM dbo.apply_summary WHERE [user_id] =@uid;UPDATE dbo.[apply] SET isread=1 WHERE apply_tid=@uid";
                return connection.Execute(sql, new { uid = userId }) > 0;
            });
        }
        #endregion

        #region 获取申请详情列表
        public override async Task<CommonResult> GetApplyList(long userId)
        {
            return await _storage.UseConnectionAsync(async connection =>
            {
                var sql = GetApplyListSQL();
                var applylist = await connection.QueryAsync<LayimApplyModel>(sql, new { uid = userId });
                return new CommonResult { code = 0, data = applylist };
            });
        }

        public override IEnumerable<LayimApplyModel> GetPageApplyList(long userId)
        {
            UpdateRead(userId);
            return _storage.UseConnection(connection =>
            {
                var sql = GetApplyListSQL();
                return connection.Query<LayimApplyModel>(sql, new { uid = userId });
            });
        }
        #endregion
         
        #region 获取群创建人
        private async Task<long> GetUserIdByGroupId(long groupId)
        {
            return await _storage.UseConnectionAsync<long>(async connection =>
            {
                var sql = "SELECT owner_id FROM dbo.big_group WHERE pk_id=@gid";
                return await connection.ExecuteScalarAsync<long>(sql, new { gid = groupId });
            });
        }
        #endregion 

        public override async Task<CommonResult> SearchUser(int start, int num, string key)
        {
            string searchSQL = string.IsNullOrEmpty(key) ? "" : $"WHERE name LIKE @key";
            var sql = $@"SELECT X.pk_id AS id,name AS username,[sign],X.avatar
FROM(SELECT    ROW_NUMBER() OVER(ORDER BY create_at ASC) AS rowId,
                    *
          FROM      dbo.[user] {searchSQL}
        ) X
WHERE   X.rowId BETWEEN @start AND @end; ";

            return await _storage.UseConnectionAsync(async connection =>
            {
                var list = await connection.QueryAsync<UserModel>(sql, new { start = start, end = start + num, key = $"%{key}%" });
                return new CommonResult { code = 0, data = list };
            });
        }

        public override async Task<CommonResult> SearchGroup(int start, int num, string key)
        {
            string searchSQL = string.IsNullOrEmpty(key) ? "" : $"WHERE name LIKE @key";
            var sql = $@"  
  SELECT    *
  FROM      ( SELECT    ROW_NUMBER() OVER ( ORDER BY create_at ASC ) rowId ,
                        pk_id AS id ,
                        name AS groupname ,
                        avatar
              FROM      dbo.big_group {searchSQL}
            ) X
  WHERE     X.rowId BETWEEN @start AND @end;";

            return await _storage.UseConnectionAsync(async connection =>
            {
                var list = await connection.QueryAsync<BigGroupModel>(sql, new { start = start, end = start + num, key = $"%{key}%" });
                return new CommonResult { code = 0, data = list };
            });
        }


        #region 其他私有方法
        private async Task<int> CreateApply(long userId,long toId,int type,long groupId,string other)
        {
            int result = await _storage.UseConnectionAsync(async connection =>
             {
                 var sql = @"INSERT INTO dbo.[apply]( apply_uid ,apply_tid ,[type] ,group_id ,isread , other ,result )
VALUES(@uid, @tid, @type, @gid, 0, @other, 0);
  IF EXISTS ( SELECT [user_id] FROM  dbo.apply_summary WHERE [user_id] = @tid )
    BEGIN UPDATE  dbo.apply_summary SET apply_count = apply_count + 1 ,update_at = GETDATE() WHERE [user_id] = @tid END;
  ELSE BEGIN INSERT INTO dbo.apply_summary ([user_id], apply_count) VALUES (@tid,1) END;";
                 return await connection.ExecuteAsync(sql, new { uid = userId, tid = toId, type = type, gid = groupId, other = other });
             });
            return result;
        }

        private CommonResult CreateResult(bool success)
        {
            return new CommonResult { code = success ? 0 : 1, msg = success ? "success" : "failed" };
        }
        private string CreateRoom(long id1, long id2, string type)
        {
            if (type == Constants.ChatGroup)
            {
                return id2.ToString();
            }

            if (type == Constants.ChatFriend || type == Constants.ChatUserService)
            {
                if (id1 > id2)
                {
                    return string.Format("{0}{1}", id2, id1);
                }
                return string.Format("{0}{1}", id1, id2);
            }

            return "";
        }
        #endregion 
    }
}
