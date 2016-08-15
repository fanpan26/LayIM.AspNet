using LayIM.Cache;
using LayIM.Model;
using LayIM.Utils.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var userEntity = new UserEntity { id=1, avatar="headphoto", sign="这个是我的签名"};
            LayIMCache.Instance.SetHash<UserEntity>(userEntity.id.ToString(), userEntity);

            var model = LayIMCache.Instance.GetHash<UserEntity>("1");
            Console.WriteLine(model.ToJson());
            userEntity.username = "修改了用户头像";
            LayIMCache.Instance.SetHash<UserEntity>(userEntity.id.ToString(), userEntity);
            var model1= LayIMCache.Instance.GetHash<UserEntity>("1");
            Console.WriteLine(model1.ToJson());

            LayIMCache.Instance.RemoveHash("1");
            var model2 = LayIMCache.Instance.GetHash<UserEntity>("1");
            Console.WriteLine(model2.ToJson());
            Console.Read();
        }
    }
}
