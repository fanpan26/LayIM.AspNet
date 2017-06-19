using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayIM.NetClient.Model;

/*
 * Layim业务逻辑基于LayimStorage的实现
 */
namespace LayIM.NetClient
{
    internal class LayimUserClient : ILayimUserClient
    {

        private LayimStorage _storage;
        private LayimRequest _request;
        private long _id;

        public LayimUserClient(LayimStorage storage, LayimRequest request) : this(storage, 0)
        {
            _request = request;
        }
        public LayimUserClient(LayimStorage storage,long id)
        {
            _storage = storage;
            _id = id;
        }
        /// <summary>
        /// 获取初始化信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object GetInitInfo()
        {
            using (var connection = _storage.GetConnection())
            {
                return connection.GetInitInfo(_id);
            }
        }

        public object GetGroupMembers()
        {
            using (var connection = _storage.GetConnection())
            {
                return connection.GetGroupMembers(_id);
            }
        }

        public async Task<bool> AddMsg()
        {
            var dictParameters = await _request.GetFormKeyValuesAsync("uid", "toid", "type", "msg");

            long fUid = dictParameters["uid"].ToInt64();
            long tUid = dictParameters["toid"].ToInt64();

            string type = dictParameters["type"];
            string msg = dictParameters["msg"];

            using (var connection = _storage.GetConnection())
            {

                return connection.AddChatMsg(new Model.LayimChatMessageModel
                {
                    CreateAt = DateTime.Now,
                    FromUserId = fUid,
                    Message = msg,
                    ToUserId = tUid,
                    Type = type
                });
            }
        }

        public async Task<CommonResult> CreateGroup()
        {
            var dictParameters = await _request.GetFormKeyValuesAsync("uid", "name", "desc", "avatar");

            var uid = dictParameters["uid"].ToInt64();
            var name = dictParameters["name"];
            var desc = dictParameters["desc"];
            var avatar = dictParameters["avatar"];

            if (uid == 0) {
                return new CommonResult { code = 1, msg = "缺少uid参数" };
            }
            if (string.IsNullOrEmpty(name) || name.Length > 20)
            {
                return new CommonResult { code = 1, msg = "群组名称限制在1-20个字符" };
            }

            using (var connection = _storage.GetConnection())
            {
                return await connection.CreateGroup(uid, name, desc, avatar);
            }
        }

        public async Task<CommonResult> CreateUser()
        {
            //string userName, string userSign, string userAvatar
            var dictParameters = await _request.GetFormKeyValuesAsync("name", "avatar", "sign");

            var name = dictParameters["name"];
            var sign = dictParameters["sign"];
            var avatar = dictParameters["avatar"];

            if (string.IsNullOrEmpty(name) || name.Length > 10)
            {
                return new CommonResult { code = 1, msg = "用户名限制在1-10个字符" };
            }

            using (var connection = _storage.GetConnection())
            {
                return await connection.CreateUser(name, sign, avatar);
            }
        }

        public Task<CommonResult> CreateUserApply()
        {
            return CreateApply(true);
        }

        public Task<CommonResult> CreateGroupApply()
        {
            return CreateApply(false);
        }

        private async Task<CommonResult> CreateApply(bool isUser=true)
        {
            var dictParameters = await _request.GetFormKeyValuesAsync("uid", "tid", "other");

            var userId = dictParameters["uid"].ToInt64();
            var toUserId = dictParameters["tid"].ToInt64();
            var other = dictParameters["other"];

            if (userId <= 0 || toUserId <= 0)
            {
                return new CommonResult { code = 1, msg = "缺少参数:[uid,tid]" };
            }

            using (var connection = _storage.GetConnection())
            {
                if (isUser)
                {
                    return await connection.CreateUserApply(userId, toUserId, other);
                }
                else
                {
                    return await connection.CreateGroupApply(userId, toUserId, other);
                }
            }
        }

        public async Task<CommonResult> SearchUser()
        {
            var start = _request.GetQuery("start").ToInt32();
            var num = _request.GetQuery("num").ToInt32();
            string key = _request.GetQuery("key");

            using (var connection = _storage.GetConnection())
            {
                return await connection.SearchUser(start, num, key);
            }
        }

        public async Task<CommonResult> SearchGroup()
        {
            var start = _request.GetQuery("start").ToInt32();
            var num = _request.GetQuery("num").ToInt32();
            string key = _request.GetQuery("key");

            using (var connection = _storage.GetConnection())
            {
                return await connection.SearchGroup(start, num, key);
            }
        }

        public async Task<CommonResult> GetApplyCount()
        {
            long userId = _request.GetQuery("uid").ToInt64();
            if (userId == 0)
            {
                return new CommonResult { code = 0, data = new { apply = 0 } };
            }
            using (var connection = _storage.GetConnection())
            {
                return await connection.GetApplyCount(userId);
            }
        }

        public async Task<CommonResult> GetApplyList()
        {
            long userId = _request.GetQuery("uid").ToInt64();
            if (userId == 0)
            {
                return new CommonResult { code = 0, data = new List<string>() };
            }
            using (var connection = _storage.GetConnection())
            {
                return await connection.GetApplyList(userId);
            }
        }

        public async Task<CommonResult> HandleApply()
        {
            var dictParameters = await _request.GetFormKeyValuesAsync("id", "uid", "result", "fgid");

            var id = dictParameters["id"].ToInt64();
            var userId = dictParameters["uid"].ToInt64();
            var result = dictParameters["result"].ToInt32();
            var uidGroup = dictParameters["fgid"].ToInt64();

            if (id == 0 || userId == 0) {
                return new CommonResult { code = 1, msg = "申请不存在" };
            }

            using (var connection = _storage.GetConnection()) {
                return await connection.HandleApply(id, userId, result, uidGroup);
            }
        }
    }
}
