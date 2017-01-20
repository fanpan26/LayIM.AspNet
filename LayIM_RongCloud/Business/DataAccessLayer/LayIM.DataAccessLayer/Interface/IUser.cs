using LayIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.DataAccessLayer.Interface
{
    public interface IUser
    {
        BaseListResult GetUserBase(int userId);
    }
}
