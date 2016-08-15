using LayIM.Model;
using LayIM.Utils.Extension;
using LayIM.Utils.JsonResult;
using LayIM.Utils.Single;
using LayIM.Utils.Validate;
using Macrosage.ElasticSearch.Core;
using Macrosage.ElasticSearch.Model;
using Macrosage.ElasticSearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.BLL.Group
{
    public class ElasticGroup
    {
        public static ElasticGroup Instance
        {
            get { return SingleHelper<ElasticGroup>.Instance; }
        }

        private Elastic<LayImGroup> es
        {
            get {
               var _es = new Elastic<LayImGroup>();
                _es.SetIndexInfo("layim", "layim_group");
                return _es;
;            }
        }

        /// <summary>
        /// 创建群之后或者修改群信息之后要更新
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool IndexGroup(LayImGroup group)
        {
            return es.Index(group);
        }

        public LayImGroup IndexGroup(DataTable dt)
        {
            if (dt == null) { return null; }
            var group = dt.Rows.Cast<DataRow>().Select(x => new LayImGroup
            {
                id = x["id"].ToString(),
                addtime = x["addtime"].ToDateTime(true),
                allcount = x["allcount"].ToInt(),
                avatar = x["avatar"].ToString(),
                groupdesc = x["groupdesc"].ToString(),
                groupname = x["groupname"].ToString(),
                im = x["im"].ToInt(),
                limitcount = x["limitcount"].ToInt()
            }).FirstOrDefault();

            if (!string.IsNullOrEmpty(group.id))
            {
                IndexGroup(group);
                return group;
            }
            return null;
        }

        public JsonResultModel SearchLayimGroup(string keyword, int pageIndex = 1, int pageSize = 50)
        {

            var result = SearchGroup(keyword, pageIndex, pageSize);

            return JsonResultHelper.CreateJson(result);
        }
        private BaseQueryEntity<LayImGroup> SearchGroup(string keyword, int pageIndex = 1, int pageSize = 50)
        {
            var hasvalue = ValidateHelper.HasValue(keyword);
            var from = (pageIndex - 1) * pageSize;
            //全部的时候按照省份排序
            string queryAll = "{\"query\":{\"match_all\":{}},\"from\":" + from + ",\"size\":" + pageSize + ",\"sort\":{\"allcount\":{\"order\":\"desc\"}}}";
            //按照关键字搜索的时候，默认排序，会把最接近在在最上边
            int im = hasvalue ? keyword.ToInt() : 0;
            //这里增加im是否为int类型判断，如果是int类型，那么可能是查询用户的IM号码，否则就是关键字查询
            string term = im == 0 ? "{\"im\":0}" : "{\"im\":" + keyword + "}";
            string queryWithKeyWord = "{\"query\":{\"filtered\":{\"filter\":{\"or\":[{\"term\":" + term + "},{\"query\":{\"match_phrase\":{\"groupname\":{\"query\":\"" + keyword + "\",\"slop\":0}}}}]}}},\"from\":" + from + ",\"size\":" + pageSize + ",\"sort\":{}}}";
            string queryFinal = hasvalue ? queryWithKeyWord : queryAll;
            var result = es.QueryBayConditions(queryFinal);
            return result;
        }
    }
}
