using LayIM.Utils.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.ChatServer.Core
{
    public class GroupHelper
    {
        public static IGroup CreateGroup()
        {
            return SingleHelper<ChatGroup>.Instance;
        }
    }
}
