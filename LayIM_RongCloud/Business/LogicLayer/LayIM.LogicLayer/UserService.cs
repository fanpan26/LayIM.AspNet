using LayIM.Container;
using LayIM.DataAccessLayer.Interface;
using LayIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.LogicLayer
{
    public class UserService
    {
        private IUser _user => LayIMDataAccessLayerContainer.GlobalContainer.Resolve<IUser>();


        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResultModel GetUserBase(int userId)
        {
            var result = _user.GetUserBase(userId);
            if (result != null && result.mine != null && result.mine.id > 0)
            {
                return new JsonResultModel
                {
                    code = JsonResultType.Success,
                    data = result
                };
            }
            return new JsonResultModel { code = JsonResultType.Failed, msg = "无此用户" };
        }
    }
}
