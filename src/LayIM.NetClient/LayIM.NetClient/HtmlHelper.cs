using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
   public class HtmlHelper
    {
        private readonly RazorPage _page;

        public HtmlHelper(RazorPage page)
        {
            _page = page;
        }

        /// <summary>
        /// 加载部分视图
        /// </summary>
        /// <param name="partialPage"></param>
        /// <returns></returns>
        public string RenderPartial(RazorPage partialPage)
        {
            partialPage.Assign(_page);
            return partialPage.ToString();
        }
    }
}
