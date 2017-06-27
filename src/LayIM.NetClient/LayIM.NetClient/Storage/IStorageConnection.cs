using LayIM.NetClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    /// <summary>
    /// layim数据调用接口
    /// 目前在 Layim.SqlServer SqlServerConnection中实现具体方法
    /// </summary>

    public interface IStorageConnection : IDisposable
    {
        /// <summary>
        /// 读取历史记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="toId"></param>
        /// <param name="type"></param>
        /// <returns></returns>

        IEnumerable<Model.LayimChatMessageViewModel> GetHistoryMessages(Model.LayimHistoryParam param);
        /// <summary>
        /// 获取Layim初始化数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        CommonResult GetInitInfo(long userId);

        /// <summary>
        /// 获取某个群组的用户列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        CommonResult GetGroupMembers(long groupId);

        /// <summary>
        /// 保存融云Token ，当融云设置token永久的时候可以永久保存数据库
        /// 也可用缓存方案替代
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool SaveToken(long userId, string token);

        /// <summary>
        /// 获取用户Token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetToken(long userId);

        /// <summary>
        /// 添加历史记录信息
        /// </summary>
        /// <param name="fromUserId">发送消息人</param>
        /// <param name="toUserId">接收消息人或者群组ID</param>
        /// <param name="msg">消息内容</param>
        /// <param name="createAt">发送时间</param>
        /// <returns>返回结果</returns>
        bool AddChatMsg(Model.LayimChatMessageModel msg);

        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="userId">所属用户ID</param>
        /// <param name="groupName">群名称</param>
        /// <param name="groupDesc">群描述</param>
        /// <param name="groupAvatar">群logo</param>
        /// <returns></returns>
        Task<CommonResult> CreateGroup(long userId, string groupName, string groupDesc, string groupAvatar);

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userName">用户昵称</param>
        /// <param name="userSign">用户签名</param>
        /// <param name="userAvatar">用户头像</param>
        /// <returns></returns>
        Task<CommonResult> CreateUser(string userName, string userSign, string userAvatar);

        /// <summary>
        /// 添加用户申请记录
        /// </summary>
        /// <param name="fromUserId">申请人ID</param>
        /// <param name="toUserId">被申请人ID</param>
        /// <param name="other">备注</param>
        /// <returns></returns>
        Task<CommonResult> CreateUserApply(long fromUserId, long toUserId, string other);

        /// <summary>
        /// 添加用户申请加群记录
        /// </summary>
        /// <param name="fromUserId">申请人ID</param>
        /// <param name="toGroupId">被申请群组ID</param>
        /// <param name="other">备注</param>
        /// <returns></returns>
        Task<CommonResult> CreateGroupApply(long fromUserId, long toGroupId, string other);

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="start">开始序号</param>
        /// <param name="num">条数</param>
        /// <param name="key">模糊搜索关键字</param>
        /// <returns></returns>
        Task<CommonResult> SearchUser(int start, int num, string key);

        /// <summary>
        /// 搜索群组
        /// </summary>
        /// <param name="start">开始序号</param>
        /// <param name="num">条数</param>
        /// <param name="key">模糊搜索关键字</param>
        /// <returns></returns>
        Task<CommonResult> SearchGroup(int start,int num,string key);

        /// <summary>
        /// 获取用户的待审批条数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CommonResult> GetApplyCount(long userId);

        /// <summary>
        /// 获取用户的请求列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CommonResult> GetApplyList(long userId);

        IEnumerable<LayimApplyModel> GetPageApplyList(long userId);

        Task<CommonResult> HandleApply(long id, long userId, int result, long uidGroup = 0);

    }
}
