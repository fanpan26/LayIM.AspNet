using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.IMServer.RMLib.Tool
{
    internal enum RMMessageType
    {
        /// <summary>
        /// 直播消息 A:LiveMessage
        /// </summary>
        typeActivityLive = 0,
        /// <summary>
        /// 弹幕消息 A:ScreenText
        /// </summary>
        typeScreenText = 1,
        /// <summary>
        /// 补充消息 A:OtherMessage
        /// </summary>
        typeActivityOther = 2,
        /// <summary>
        /// 命令消息 A:CMDMessage
        /// </summary>
        typeActivityCommand = 3,
        /// <summary>
        /// 喜欢消息 A:LikeMessage
        /// </summary>
        typeActivityLike = 4
    }
}
