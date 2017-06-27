using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    internal class RazorPageDispatcher : ILayimDispatcher
    {
        private readonly Func<Match, RazorPage> _pageFunc;

        public RazorPageDispatcher(Func<Match, RazorPage> pageFunc)
        {
            _pageFunc = pageFunc;
        }

        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Dispatch(LayimContext context)
        {
           
            context.Response.ContentType = "text/html";
            //根据路径匹配到相应的页面
            var page = _pageFunc(context.UriMatch);

            page.Assign(context);
            //输出该页面的值
            return context.Response.WriteAsync(page.ToString());
        }
    }
}
