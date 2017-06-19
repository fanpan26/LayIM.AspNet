using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayIM.NetClient.Model;

namespace LayIM.NetClient
{
    /// <summary>
    /// 不做实现
    /// </summary>
    public class LayimStorageConnection : IStorageConnection
    {
        public virtual void Dispose()
        {
        }

        public virtual CommonResult GetGroupMembers(long groupId)
        {
            throw new NotImplementedException();
        }

        public virtual string GetToken(long userId)
        {
            throw new NotImplementedException();
        }

        public virtual bool SaveToken(long userId, string token)
        {
            throw new NotImplementedException();
        }

        public virtual bool AddChatMsg(LayimChatMessageModel msg)
        {
            throw new NotImplementedException();
        }

        public virtual CommonResult GetInitInfo(long userId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<LayimChatMessageViewModel> GetHistoryMessages(LayimHistoryParam param)
        {
            throw new NotImplementedException();
        }

        public virtual Task<CommonResult> CreateGroup(long userId, string groupName, string groupDesc, string groupAvatar)
        {
            throw new NotImplementedException();
        }

        public virtual Task<CommonResult> CreateUser(string userName, string userSign, string userAvatar)
        {
            throw new NotImplementedException();
        }

        public virtual Task<CommonResult> CreateUserApply(long fromUserId, long toUserId, string other)
        {
            throw new NotImplementedException();
        }

        public virtual Task<CommonResult> CreateGroupApply(long fromUserId, long toGroupId, string other)
        {
            throw new NotImplementedException();
        }

        public virtual Task<CommonResult> SearchUser(int start, int num, string key)
        {
            throw new NotImplementedException();
        }

        public virtual Task<CommonResult> SearchGroup(int start, int num, string key)
        {
            throw new NotImplementedException();
        }

        public virtual Task<CommonResult> GetApplyCount(long userId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<CommonResult> GetApplyList(long userId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<LayimApplyModel> GetPageApplyList(long userId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<CommonResult> HandleApply(long id, long userId, int result, long uidGroup = 0)
        {
            throw new NotImplementedException();
        }
    }
}
