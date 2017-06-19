using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    /// <summary>
    /// 路由集合
    /// </summary>
   public class RouteCollection
    {
        private readonly List<Tuple<string, ILayimDispatcher>> _dispatchers = new List<Tuple<string, ILayimDispatcher>>();

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="pathTemplate">路由地址</param>
        /// <param name="dispatcher">处理方法</param>
        public void Add(string pathTemplate, ILayimDispatcher dispatcher)
        {
            Error.ThrowIfNull(pathTemplate, nameof(pathTemplate));
            Error.ThrowIfNull(dispatcher, nameof(dispatcher));

            _dispatchers.Add(new Tuple<string, ILayimDispatcher>(pathTemplate, dispatcher));
        }

        /// <summary>
        /// 根据Path寻找对应的Dispatcher
        /// 通过正则表达式来找到匹配的结果
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public Tuple<ILayimDispatcher, Match> FindDispatcher(string path)
        {
            if (path.Length == 0) path = "/";

            foreach (var dispatcher in _dispatchers)
            {
                var pattern = dispatcher.Item1;

                if (!pattern.StartsWith("^", StringComparison.OrdinalIgnoreCase)) {
                    pattern = $"^{pattern}";
                }

                if (!pattern.EndsWith("$", StringComparison.OrdinalIgnoreCase)) {
                    pattern += "$";
                }

                var match = Regex.Match(path, pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (match.Success) {
                    return new Tuple<ILayimDispatcher, Match>(dispatcher.Item2, match);
                }
            }
            return null;
        }
    }
}
