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
```
### 项目结构
![](http://img1.gurucv.com/image/2017/2/8/02c84ea7702d42ae957d67e995de4d57.png) 
### 源码下载，请联系：645857874 或发邮件至 645857874@qq.com
#### LayIM官网 http://www.layui.com/doc/layim.html
 
