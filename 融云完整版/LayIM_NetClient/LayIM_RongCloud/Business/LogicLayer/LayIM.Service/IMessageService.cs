using LayIM.Model.Message.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Service
{
    public interface IMessageService
    {
        bool AddMsg(int userId, int to, string type, string content);
        bool AddMsg(ChatMessage message);
    }
}
