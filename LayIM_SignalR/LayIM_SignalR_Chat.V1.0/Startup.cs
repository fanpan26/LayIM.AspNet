using LayIM.ChatServer.UserProvider;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LayIM_SignalR_Chat.V1._0.Startup))]
namespace LayIM_SignalR_Chat.V1._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ///注册自定义用户ID方法
            var userIdProvider = new LayIMUserFactory();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => userIdProvider);
            //ConfigureAuth(app);
            app.Map("/layim", map =>
            {
                var hubConfiguration = new HubConfiguration()
                {
                    EnableJSONP = true, EnableJavaScriptProxies=true
                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
