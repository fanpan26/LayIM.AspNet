using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public static class RouteCollectionExtensions
    {
        /// <summary>
        /// 添加执行命令的路由
        /// </summary>
        /// <param name="routes">路由集合</param>
        /// <param name="pathTemplate">路由地址</param>
        /// <param name="command">执行命令参数</param>
        public static void AddCommand(this RouteCollection routes, string pathTemplate, Func<LayimContext, bool> command)
        {
            Error.ThrowIfNull(routes, nameof(routes));
            Error.ThrowIfNull(pathTemplate, nameof(pathTemplate));
            Error.ThrowIfNull(command, nameof(command));

            routes.Add(pathTemplate, new CommandDispatcher(command));
        }

        /// <summary>
        /// 添加查询命令的路由
        /// </summary>
        /// <param name="routes">路由集合</param>
        /// <param name="pathTemplate">路由地址</param>
        /// <param name="execute">查询命令参数</param>
        public static void AddQueryExecute(this RouteCollection routes, string pathTemplate, Func<LayimContext, Task<object>> execute)
        {
            Error.ThrowIfNull(routes, nameof(routes));
            Error.ThrowIfNull(pathTemplate, nameof(pathTemplate));
            Error.ThrowIfNull(execute, nameof(execute));

            routes.Add(pathTemplate, new ExecuteResultDispatcher(execute, "GET"));
        }

        /// <summary>
        /// 添加查询命令的路由
        /// </summary>
        /// <param name="routes">路由集合</param>
        /// <param name="pathTemplate">路由地址</param>
        /// <param name="execute">查询命令参数</param>
        public static void AddCommandExecute(this RouteCollection routes, string pathTemplate, Func<LayimContext, Task<object>> execute)
        {
            Error.ThrowIfNull(routes, nameof(routes));
            Error.ThrowIfNull(pathTemplate, nameof(pathTemplate));
            Error.ThrowIfNull(execute, nameof(execute));

            routes.Add(pathTemplate, new ExecuteResultDispatcher(execute, "POST"));
        }

        /// <summary>
        /// 添加带参数的查询命令路由
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="routes">路由集合</param>
        /// <param name="pathTemplate">路由地址</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="query">查询命令参数</param>
        public static void AddQuery<T>(this RouteCollection routes, string pathTemplate, string parameterName, Func<LayimContext, T, object> query)
        {
            Error.ThrowIfNull(routes, nameof(routes));
            Error.ThrowIfNull(pathTemplate, nameof(pathTemplate));
            Error.ThrowIfNull(query, nameof(query));

            routes.Add(pathTemplate, new SingleParameterQueryDispatcher<T>(parameterName, query));
        }

        /// <summary>
        /// 添加路由页面
        /// </summary>
        /// <param name="routes">路由集合</param>
        /// <param name="pathTemplate">路由地址</param>
        /// <param name="pageFunc">生成页面的方法</param>
        public static void AddRazorPage(this RouteCollection routes, string pathTemplate, Func<Match, RazorPage> pageFunc)
        {
            Error.ThrowIfNull(routes, nameof(routes));
            Error.ThrowIfNull(pathTemplate, nameof(pathTemplate));
            Error.ThrowIfNull(pageFunc, nameof(pageFunc));

            routes.Add(pathTemplate, new RazorPageDispatcher(pageFunc));
        }

        /// <summary>
        /// 添加js文件
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="jsName"></param>
        /// <param name="assembly"></param>
        /// <param name="nameSpace"></param>
        public static void AddJs(this RouteCollection routes, string jsName, Assembly assembly, string nameSpace)
        {
            routes.Add($"/js/{jsName}", new CombinedResourceDispatcher("application/javascript", assembly, nameSpace, $"{jsName}.js"));
        }

        /// <summary>
        /// 添加css文件
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="jsName"></param>
        /// <param name="assembly"></param>
        /// <param name="nameSpace"></param>
        public static void AddCss(this RouteCollection routes, string jsName, Assembly assembly, string nameSpace)
        {
            routes.Add($"/css/{jsName}", new CombinedResourceDispatcher("text/css", assembly, nameSpace, $"{jsName}.css"));
        }

        /// <summary>
        /// 批量添加js路由
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="jsNames"></param>
        /// <param name="assembly"></param>
        /// <param name="nameSpace"></param>
        public static void AddJsFolder(this RouteCollection routes, string[] jsNames, Assembly assembly, string nameSpace)
        {
            foreach (var js in jsNames)
            {
                routes.AddJs(js, assembly, nameSpace);
            }
        }

        /// <summary>
        /// 添加css包
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="cssNames"></param>
        /// <param name="assembly"></param>
        /// <param name="nameSpace"></param>
        public static void AddCssFolder(this RouteCollection routes, string[] cssNames, Assembly assembly, string nameSpace)
        {
            foreach (var css in cssNames)
            {
                routes.AddCss(css, assembly, nameSpace);
            }
        }

    }
}
