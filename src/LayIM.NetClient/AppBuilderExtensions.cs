using Microsoft.Owin.Infrastructure;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    using BuildFunc = Action<
          Func<
            IDictionary<string, object>,
            Func<
              Func<IDictionary<string, object>, Task>,
              Func<IDictionary<string, object>, Task>
          >>>;

    public static class AppBuilderExtensions
    {
        /// <summary>
        /// 根目录默认设置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pathMatch"></param>
        /// <returns></returns>
        public static IAppBuilder UseLayimApi(this IAppBuilder builder)
        {
            return builder.UseLayimApi("/layim");
        }
        /// <summary>
        /// 设置路径
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pathMatch"></param>
        /// <returns></returns>
        public static IAppBuilder UseLayimApi(this IAppBuilder builder, string pathMatch)
        {
            return builder.UseLayimApi(pathMatch, new LayimOptions());
        }

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pathMatch"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IAppBuilder UseLayimApi(this IAppBuilder builder, string pathMatch, LayimOptions options)
        {
            return builder.UseLayimApi(pathMatch, options, LayimStorage.Current);
        }

        /// <summary>
        /// 自定义其他设置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pathMatch"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IAppBuilder UseLayimApi(this IAppBuilder builder, string pathMatch, LayimOptions options,LayimStorage storage)
        {
            Error.ThrowIfNull(pathMatch, nameof(pathMatch));
            Error.ThrowIfNull(options, nameof(options));

            SignatureConversions.AddConversions(builder);

            builder.Map(pathMatch, appBuilder => appBuilder
                .UseOwin()
                .UseLayimApi(storage,options, LayimRoutes.Routes));
            return builder;
        }

        private static BuildFunc UseOwin(this IAppBuilder builder)
        {
            return middleware => builder.Use(middleware(builder.Properties));
        }
    }
}
