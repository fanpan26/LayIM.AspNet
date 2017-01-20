1.项目所有代码与LayIM_SignalR项目文件夹下代码不冲突，也不会互相引用。
2.数据库交互用轻量级ORM组件Dapper实现
3.真正加入消息队列功能，队列采用RabbitMQ，依赖于EasyNetQ
4.重构ElasticSearch代码，依赖于Nest和ElasticSearch.Net
5.即时通讯功能采用第三方RongCloud服务，交互更加稳定。程序设计会以接口形式实现，方便RongCloud和SingalR版本切换（SignalR版本请移步LayIM_SignalR）
6.RongCloud .NET开发包从融云官网下载。做了一些改动 在项目 LayIM.RMLib项目中



2017-1-18
对接融云，可以进行聊天
2017-1-19
增加消息队列，日志组件，自定义Ioc组件
