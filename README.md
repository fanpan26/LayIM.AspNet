### 项目简介

###### LayIM.NETClient 是对于当前流行前端WebIM框架LayIM后端的实现。
###### LayIM项目地址：http://layim.layui.com/
-- 此项目仅供学习交流使用，不适合用在生产环境。数据库为SQL Server 2008，开发工具为Visual Studio 2015.

###### 快速开始
---
 第一步：新建MVC项目或者ASP.NET WebForm项目，添加 Owin Startup 类。
```
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
```

---
第二步：运行数据库脚本（当前未自动化，需要手动运行） LayIM.SqlServer/layim.sql

---
第三步：修改配置文件，增加数据库连接字符串和融云的appkey，appsecret.(下文中的key和secret可以直接拿去做测试使用)

```
	<!--融云配置-->
    <add key="RongCloud_AppKey" value="pvxdm17jpv1or"/>
    <add key="RongCloud_AppSecret" value="Co2RwQhzkL6G8i"/>
```

```
    <connectionStrings>
		<add name="LayIM_Connection" connectionString="server=192.168.1.18;user id=sa;password=123123;database=LayIM;Min Pool Size=16;Connect Timeout=500;"/>
    </connectionStrings>
```

--- 
第四步：将下载的layui文件夹放入项目的 Scripts 文件夹下。（参考MVCSamples项目）

--- 
第五步：运行项目。运行成功后由于项目中尚无数据。可以访问 http://localhost:6357/api 中的添加用户接口增加测试用户。
![](http://img1.gurucv.com/image/2017/6/19/0c0b3974be244c41a8f20c14888de28f.png)

--- 
添加完用户之后默认第一个用户ID为 100000. 然后访问 http://localhost:6357/?uid=100000 即可展现LayIM页面。
![](http://img1.gurucv.com/image/2017/6/19/2d8f5b5887eb4161aa13b74290b60976.png)

---
剩下的事情就可以自己根据接口中做各种测试啦。比如，添加好友。
![](http://img1.gurucv.com/image/2017/6/19/a9de2a5ea24a4b098c9496f5c4a447ca.png)

---
项目介绍到此为止。剩下有遇到问题或者bug 的小伙伴，加群了解吧。 群号：303451512 