# LayIM.OwinMiddleware 

###### 对接 LayIM 实现的一个.NET后端组件。基于Owin开发,组件与WebUI解耦，无论是WebForm还是MVC都可以在 Owin Startup 中配置使用。


######  LayIM.NetClient 项目简介

###### 组件核心代码，中间件注册扩展方法，路由注册，数据请求处理等公共逻辑。 融云通信服务端接口对接等。

######  LayIM.SqlServer 项目简介

###### 实现了LayIM的通用接口方法，用户好友列表，群组列表，历史纪录保存等。

###### Startup.cs 文件代码如下：

 ```C#
 public void Configuration(IAppBuilder app)
        {
            //使用SQL Server
            GlobalConfiguration.Configuration.UseSqlServer("LayIM_Connection");

            //使用layim api 
            app.UseLayimApi();
        }
 ```
###### 或者在配置文件中配置相应的信息。

```
 <!--融云配置-->
    <add key="RongCloud_AppKey" value="pvxdm17jpv1or"/>
    <add key="RongCloud_AppSecret" value="*******"/>
    
     <connectionStrings>
    <add name="LayIM_Connection" connectionString="****************"/>
  </connectionStrings>
```

###### index.html：
###### https://github.com/fanpan26/LayIM.OwinMiddleware/blob/master/MVCSample/Views/Home/Index.cshtml

###### socket.js (通讯使用融云)
###### https://github.com/fanpan26/LayIM.OwinMiddleware/tree/master/MVCSample/Scripts/im/rc

###### LayIM 官网： http://layim.layui.com

#### 欢迎大家找bug，star ★
