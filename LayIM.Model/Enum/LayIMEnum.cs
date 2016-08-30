using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Model.Enum
{
    #region 聊天类型
    /// <summary>
    /// 聊天消息类型
    /// </summary>
    public enum ChatType
    {
        /// <summary>
        /// 客户端对客户端，即单聊
        /// </summary>
        ClientToClient = 0,
        /// <summary>
        /// 客户端对群组，即群聊
        /// </summary>
        ClientToGroup = 1
    }

    public enum ChatToClientType
    {
        /// <summary>
        /// 系统级别的消息，一般不会做业务处理
        /// </summary>
        System = 0,
        /// <summary>
        /// 单聊消息
        /// </summary>
        ClientToClient = 1,
        /// <summary>
        /// 群聊消息
        /// </summary>
        ClientToGroup = 2,
        /// <summary>
        /// 系统公告
        /// </summary>
        GroupToClient = 3,
        /// <summary>
        /// 创建群成功之后
        /// </summary>
        GroupCreatedToClient = 4,
        /// <summary>
        /// 用户处理消息之后给对方发送消息
        /// </summary>
        ApplyHandledToClient = 5,
        /// <summary>
        /// 申请发送之后发送给对方的消息
        /// </summary>
        ApplySendedToClient = 6,
        /// <summary>
        /// 用户上下线发送给好友的消息
        /// </summary>
        UserOnOffLineToClient = 7
    }
    #endregion

    #region  消息类型
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 普通消息
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 系统消息
        /// </summary>
        System = 1
    }

    #endregion

    #region 群组角色
    public enum GroupRoleType
    {   
        /// <summary>
        /// 普通群员
        /// </summary>
        Member = 0,
        /// <summary>
        /// 群主，拥有最高权限
        /// </summary>
        Owner = 1,
        /// <summary>
        /// 群管理员
        /// </summary>
        Manager = 2,
        /// <summary>
        /// 游客（不能发言）
        /// </summary>
        Visitor = 3
    }
    #endregion

    #region 好友添加设置类型
    public enum FriendAddSettingType
    {
        /// <summary>
        /// 任何人都可以添加，不用通过审核，直接添加成功
        /// </summary>
        AnyOne = 0,
        /// <summary>
        /// 需要附属消息验证
        /// </summary>
        NeedSomeMsg = 1,
        /// <summary>
        /// 需要回答问题，并且由本人验证
        /// </summary>
        NeedQuestionAnswer = 2,
        /// <summary>
        /// 正确回答问题，自动添加
        /// </summary>
        NeedCorrectAnswer = 3
    }
    #endregion

    #region 防打扰设置类型
    public enum DisturbSettingType
    {
        /// <summary>
        /// 任何人（临时会话）
        /// </summary>
        AnyOne = 0,
        /// <summary>
        /// 好友关系或者群组成员
        /// </summary>
        FriendAndGroupMember = 1,
        /// <summary>
        /// 仅限好友，群组成员也不能打扰私聊
        /// </summary>
        OnlyFriend = 2
    }
    #endregion

    #region 申请类型
    public enum ApplyType
    {
        /// <summary>
        /// 用户加好友申请
        /// </summary>
        Friend = 0,
        /// <summary>
        /// 用户请求加群申请
        /// </summary>
        UserJoinGroup = 1,
        /// <summary>
        /// 群主邀请某些人加入群
        /// </summary>
        GroupInviteUser = 2
    }
    #endregion

    #region 消息业务处理类型
    public enum ChatMessageSaveType
    {
        /// <summary>
        /// SQL数据库保存
        /// </summary>
        SQL = 0,
        /// <summary>
        /// 队列保存
        /// </summary>
        Queue = 1,
        /// <summary>
        /// 搜索引擎
        /// </summary>
        SearchEngine = 2,
        /// <summary>
        /// 用户自定义，可能以上都包含或者包含几部分
        /// </summary>
        Main = 3
    }
    #endregion
}
