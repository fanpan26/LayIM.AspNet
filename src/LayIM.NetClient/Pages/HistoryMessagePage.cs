using LayIM.NetClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public partial class HistoryMessagePage
    {
        private LayimHistoryParam _param;

        public HistoryMessagePage():this(null) { }
        public HistoryMessagePage(RazorPage parent)
        {
            if (parent != null) {
                Assign(parent);
            }
        }
    }
}
