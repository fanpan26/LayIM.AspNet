using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Model.ViewModel
{
    public class LayIMBaseView
    {
        public int id { get; set; }
        public string name { get; set; }
        public string photo { get; set; }

        public DateTime addtime { get; set;}
        public string addtimestr { get { return addtime.ToString("yyyy/MM/dd"); } }
    }

    /// <summary>
    /// 查询页面
    /// </summary>
    public class LayIMUser : LayIMBaseView
    {
        public string sign { get; set; }
        public bool isfriend { get; set; }
    }
}
