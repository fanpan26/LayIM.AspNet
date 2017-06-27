using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using LayIM.NetClient;
using LayIM.SqlServer;

[assembly: OwinStartup(typeof(HtmlSample.Startup))]

namespace HtmlSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //使用SQL Server
            GlobalConfiguration.Configuration.UseSqlServer("LayIM_Connection");

            //使用layim api 
            app.UseLayimApi();
        }
    }
}
