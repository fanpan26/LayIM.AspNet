using LayIM.NetClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Layim的业务逻辑接口
 */
namespace LayIM.NetClient
{
    internal interface ILayimUserClient
    {
        /// <summary>
        /// 获取layim初始化的数据信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        object GetInitInfo();

        /// <summary>
        /// 获取群员列表
        /// </summary>
        /// <returns></returns>
        object GetGroupMembers();

        /// <summary>
        /// 添加消息记录
        /// </summary>
        /// <returns></returns>
        Task<bool> AddMsg();

        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="userId">所属用户ID</param>
        /// <param name="groupName">群名称</param>
        /// <param name="groupDesc">群描述</param>
        /// <param name="groupAvatar">群logo</param>
        /// <returns></returns>
        Task<CommonResult> CreateGroup();

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userName">用户昵称</param>
        /// <param name="userSign">用户签名</param>
        /// <param name="userAvatar">用户头像</param>
        /// <returns></returns>
        Task<CommonResult> CreateUser();

        /// <summary>
        /// 添加用户申请记录
        /// </summary>
        /// <param name="fromUserId">申请人ID</param>
        /// <param name="toUserId">被申请人ID</param>
        /// <param name="other">备注</param>
        /// <returns></returns>
        Task<CommonResult> CreateUserApply();

        /// <summary>
        /// 添加用户申请加群记录
        /// </summary>
        /// <param name="fromUserId">申请人ID</param>
        /// <param name="toGroupId">被申请群组ID</param>
        /// <param name="other">备注</param>
        /// <returns></returns>
        Task<CommonResult> CreateGroupApply();

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="start">开始序号</param>
        /// <param name="num">条数</param>
        /// <param name="key">模糊搜索关键字</param>
        /// <returns></returns>
        Task<CommonResult> SearchUser();

        /// <summary>
        /// 搜索群组
        /// </summary>
        /// <param name="start">开始序号</param>
        /// <param name="num">条数</param>
        /// <param name="key">模糊搜索关键字</param>
        /// <returns></returns>
        Task<CommonResult> SearchGroup();

        /// <summary>
        /// 获取用户待审批的条数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CommonResult> GetApplyCount();

        /// <summary>
        /// 获取申请列表
        /// </summary>
        /// <returns></returns>
        Task<CommonResult> GetApplyList();

        /// <summary>
        /// 处理申请请求
        /// </summary>
        /// <returns></returns>
        Task<CommonResult> HandleApply();
    }

}
