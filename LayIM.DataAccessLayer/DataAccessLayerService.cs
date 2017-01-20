using LayIM.Container;
using LayIM.DataAccessLayer.Classes;
using LayIM.DataAccessLayer.Interface;
using LayIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.DataAccessLayer
{
    public class DataAccessLayerService
    {
        /// <summary>
        /// 服务需要在BLL中注册
        /// </summary>
        public static void RegisterService()
        {
            LayIMDataAccessLayerContainer.GlobalContainer.Register<IUser, User>()
                                                         .Register<IMultipleHandler<BaseListResult>, UserBaseListHandler>();
                
        }
    }
}
