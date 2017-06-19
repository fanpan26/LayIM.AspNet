using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    using MidFunc = Func<
        Func<IDictionary<string, object>, Task>,
        Func<IDictionary<string, object>, Task>
        >;

    using BuildFunc = Action<
        Func<
            IDictionary<string,object>,
            Func<
                Func<IDictionary<string,object>,Task>,
                Func<IDictionary<string,object>,Task>
                >>>;

   public static class MiddlewareExtensions
    {
        public static BuildFunc UseLayimApi(this BuildFunc builder,LayimStorage storage, LayimOptions options, RouteCollection routes)
        {
            builder(_ => UseLayimApi(storage, options, routes));
            return builder;
        }


        public static MidFunc UseLayimApi(LayimStorage storage,LayimOptions options, RouteCollection routes)
        {
            Error.ThrowIfNull(options, nameof(options));
            Error.ThrowIfNull(routes, nameof(routes));

            return next =>
                   env =>
            {
                var owinContext = new OwinContext(env);

                var context = new OwinLayimContext(storage,options, env);

                var path =  owinContext.Request.Path.Value;

                //匹配路由
                var findResult = routes.FindDispatcher(path);
                //如果没有匹配到，执行下一个
                if (findResult == null) {
                    return next(env);
                }

                //匹配成功之后执行 Dispatch
                context.UriMatch = findResult.Item2;
                //执行具体disptach方法，返回相应结果
                return findResult.Item1.Dispatch(context);
            };
        }

        private static Task Unauthorized(IOwinContext owinContext)
        {
            var isAuthorized = owinContext.Authentication?.User?.Identity?.IsAuthenticated;

            owinContext.Response.StatusCode = isAuthorized == true ? (int)HttpStatusCode.Forbidden : (int)HttpStatusCode.Unauthorized;

            return Task.FromResult(0);

        }
    }
}
