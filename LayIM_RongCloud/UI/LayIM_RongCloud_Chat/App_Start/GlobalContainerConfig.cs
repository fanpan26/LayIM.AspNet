using LayIM.Container;
using LayIM.Logger;
using LayIM.LogicLayer;
using LayIM.Queue.Config;
using LayIM.Queue.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LayIM_RongCloud_Chat.App_Start
{
    public class GlobalContainerConfig
    {
        /// <summary>
        /// 注册各种服务
        /// </summary>
        public static void RegisterService()
        {
            //注册各种服务
            LayIMGlobalServiceContainer.GlobalContainer
                //队列配置
                .Register<IQueueConfig, LayIMQueueConfig>()
                //队列服务
                .Register<ILayIMQueue, LayIMQueue>();
            //注册数据层服务
            LogicLayerService.RegisterDataService();
        }
    }
}