##LayIM3.0 .NET 【融云】版本的实现
### 通讯业务更加稳定
### 加入消息队列服务，搜索服务
### 项目配置更加灵活，可灵活切换数据源（目前支持SQL Server、 Elasticsearch），其他数据源可自行开发，注册配置即可随意更换。
```c#
   //注册各种服务
            LayIMGlobalServiceContainer.GlobalContainer
                //队列配置
                .Register<IQueueConfig, LayIMQueueConfig>()
                //队列服务
                .Register<ILayIMQueue, LayIMQueue>()
                //消息处理方式 MessageService 直接加入数据库，MessageQueueService 加入队列，交给队列处理
                .Register<IMessageService, MessageQueueService>();
            //注册数据层服务
            LogicLayerService.RegisterDataService();
            LayIMDataAccessLayerContainer.GlobalContainer.Register<IUser, User>()//注册用户类
                                                         .Register<IChatMessage,Message>()//注册消息类
                                                         .Register<IMultipleHandler<BaseListResult>, UserBaseListHandler>();//注册基本数据处理类
```
### 项目结构
![](http://img1.gurucv.com/image/2017/2/8/02c84ea7702d42ae957d67e995de4d57.png) 
### （SignalR版本已开源，可自由下载）此版本源码下载，请联系：645857874 或发邮件至 645857874@qq.com
#### LayIM官网 http://www.layui.com/doc/layim.html （感谢贤心大神提供的优秀web前端产品）
####### 更新 ：2017-2-8
 
