using LayIM.Model.Message.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.DataAccessLayer.Interface
{
    public interface IChatMessage
    {
        bool AddMessage(ChatMessage message);
    }
}
