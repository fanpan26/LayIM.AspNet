using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
   /*
    * LayIM 路由
    * 所有关于 LayIM 的请求都会从此路由中寻找相应的请求地址
    */
    public static class LayimRoutes
    {
        //对外共开
        public static RouteCollection Routes { get; }

        private static readonly string[] Javascripts = { "apply", "chatlog","jquery" };
        private static readonly string[] StyleSheets = { "apply", "chatlog","layui" };


        static LayimRoutes()
        {
            Routes = new RouteCollection();

            #region 资源文件
            Routes.AddJsFolder(Javascripts, GetExecutingAssembly(), GetContentFolderNamespace("js"));
            Routes.AddCssFolder(StyleSheets, GetExecutingAssembly(), GetContentFolderNamespace("css"));
            #endregion

            //获取用户好友列表
            Routes.AddQuery<long>("/init", "id", (context, uid) =>
             {
                 var client = new LayimUserClient(context.Storage, uid);
                 return client.GetInitInfo();
             });

            //根据群id 获取组员列表
            Routes.AddQuery<long>("/members", "id", (context, gid) =>
             {
                 var client = new LayimUserClient(context.Storage, gid);
                 return client.GetGroupMembers();
             });

            //获取融云token方法
            Routes.AddQuery<string>("/token", "id", (context, uid) =>
            {

                var cloud = RongCloudContainer.CreateInstance(context.Options?.RongCloudSetting);
                var token = cloud.GetToken(uid);
                return token;
            });

            //添加历史记录
            Routes.AddCommand("/chat", context =>
            {
                var client = new LayimUserClient(context.Storage, context.Request);
                return client.AddMsg().Result;
            });
            //添加群组
            Routes.AddCommandExecute("/group/add", async context =>
             {
                 var client = new LayimUserClient(context.Storage, context.Request);
                 return await client.CreateGroup();
             });
            //添加用户
            Routes.AddCommandExecute("/user/add", async context =>
             {
                 var client = new LayimUserClient(context.Storage, context.Request);
                 return await client.CreateUser();
             });
            //申请好友
            Routes.AddCommandExecute("/friend/apply", async context =>
             {
                 var client = new LayimUserClient(context.Storage, context.Request);
                 return await client.CreateUserApply();
             });
            //申请加群
            Routes.AddCommandExecute("/group/apply", async context =>
             {
                 var client = new LayimUserClient(context.Storage, context.Request);
                 return await client.CreateGroupApply();
             });
            Routes.AddCommandExecute("/apply/handle", async context =>
            {
                var client = new LayimUserClient(context.Storage, context.Request);
                return await client.HandleApply();
            });
            //查找用户
            Routes.AddQueryExecute("/user/search",async context =>
            {
                var client = new LayimUserClient(context.Storage, context.Request);
                return await client.SearchUser();
            });
            //查找群组
            Routes.AddQueryExecute("/group/search",async context =>
            {
                var client = new LayimUserClient(context.Storage, context.Request);
                return await client.SearchGroup();
            });
            //获取申请条数
            Routes.AddQueryExecute("/apply/count", async context =>
             {
                 var client = new LayimUserClient(context.Storage, context.Request);
                 return await client.GetApplyCount();
             });
            //获取申请的列表
            Routes.AddQueryExecute("/apply/list", async context =>
            {
                var client = new LayimUserClient(context.Storage, context.Request);
                return await client.GetApplyList();
            });
            //聊天历史记录页面
            Routes.AddRazorPage("/chatlog", x => new ChatLogPage());
            //历史记录局部视图
            Routes.AddRazorPage("/history", x => new HistoryMessagePage());
            //申请列表页面
            Routes.AddRazorPage("/msgbox", x => new MsgBoxPage());
        }


        internal static string GetContentFolderNamespace(string contentFolder)
        {
            var res = $"{typeof(LayimRoutes).Namespace}.Content.{contentFolder}";
            return res;
        }

        internal static string GetContentResourceName(string contentFolder, string resourceName)
        {
            return $"{GetContentFolderNamespace(contentFolder)}.{resourceName}";
        }



        private static Assembly GetExecutingAssembly()
        {
            return typeof(LayimRoutes).GetTypeInfo().Assembly;
        }
    }
}
