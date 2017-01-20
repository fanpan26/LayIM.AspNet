using LayIM_RongCloud_Chat.App_Start;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LayIM_RongCloud_Chat.Startup))]
namespace LayIM_RongCloud_Chat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //注册日志
            LoggerConfig.RegisterLogger();
            //注册服务
            GlobalContainerConfig.RegisterService();
        }
    }
}
