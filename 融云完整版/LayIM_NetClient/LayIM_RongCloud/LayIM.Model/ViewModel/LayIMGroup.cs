using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Model.ViewModel
{
    /// <summary>
    /// 查询页面
    /// </summary>
    public class LayIMGroup : LayIMBaseView
    {
        public bool isjoin { get; set; }
        public string gdesc { get; set; }
        public int owner { get; set; }

    }
}
