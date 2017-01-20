using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayIM.Model.Result;
using Macrosage.ElasticSearch.Models;
using Macrosage.ElasticSearch.Core;
using LayIM.Utils.Single;
using LayIM.Model.Message;
using LayIM.Model.Enum;
using LayIM.Utils.Extension;

namespace LayIM.BLL.Messages
{
    public class ElasticMessage : IBaseMessage
    {
        public static ElasticMessage Instance
        {
            get
            {
                return SingleHelper<ElasticMessage>.Instance;
            }
        }
        private Elastic<ChatInfo> es
        {
            get
            {
                var _es = new Elastic<ChatInfo>();
                _es.SetIndexInfo("layim", "chatinfo");

                return _es;
            }
        }
        public SendMessageResult Send(ClientBaseMessage message)
        {
            ChatInfo chatInfo;
            var mine = message.mine;
            chatInfo = new ChatInfo
            {
                addtime = message.addtime,
                avatar = mine.avatar,
                content = mine.content,
                nickname = mine.username,
                qq = mine.id,
                timespan = message.addtime.ToTimestamp(),
                roomid = message.roomid,
                isfile = mine.content.IndexOf("file[") > -1,
                isimg = mine.content.IndexOf("img[") > -1
            };
            #region 这段代码暂时保留，但可以不用这么写，因为用不到to的内容
            /*
            switch (message.type) {
                case ChatToClientType.ClientToClient:
                    message = (ClientToClientMessage)message;
                   
                    chatInfo = new ChatInfo
                    {
                        addtime = message.addtime,
                        avatar = mine.avatar,
                        content = mine.content,
                        nickname = mine.username,
                        qq = mine.id,
                        timespan = 0,
                        roomid = message.roomid,
                        isfile = mine.content.IndexOf("file[") > -1,
                        isimg = mine.content.IndexOf("img[") > -1
                    };
                    break;
                case ChatToClientType.ClientToGroup:
                    message = (ClientToGroupMessage)message;
                    chatInfo = new ChatInfo
                    {
                        addtime = message.addtime,
                        avatar = mine.avatar,
                        content = mine.content,
                        nickname = mine.username,
                        qq = mine.id,
                        timespan = 0,
                        roomid = message.roomid,
                        isfile = mine.content.IndexOf("file[") > -1,
                        isimg = mine.content.IndexOf("img[") > -1
                    };
                    break;
                default:
                    chatInfo= new ChatInfo();
                    break;
            }
            */
            #endregion

           bool result= es.Index(chatInfo);
            return new SendMessageResult(result);
        }
    }
}
