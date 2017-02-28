using Dapper;
using LayIM.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.DataAccessLayer.Helper
{
    public class PageHelper
    {
        #region 通用存储过程分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">查询类型</typeparam>
        /// <param name="options">分页查询条件</param>
        /// <returns>返回PageView<T></returns>
        public static PageView<T> GetPageList<T>(PageSearchOptions options)
        {
            const string pageProcedureName = "Proc_GetPageData";
            //构造参数，存储过程固定
            var p = new DynamicParameters();
            p.Add("TableName", options.TableName);
            p.Add("PrimaryKey", options.PrimaryKey);
            p.Add("Fields", options.Fields);
            p.Add("Condition", options.Condition);
            p.Add("CurrentPage", options.PageIndex);
            p.Add("PageSize", options.PageSize);
            p.Add("Sort", options.Sort);
            p.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var pageData = new PageView<T>(options.PageIndex, options.PageSize);
            
            if (HasDangerousContents(options.Condition))
            {
                return pageData;
            }
            pageData.List = DBHelper.Query<T>(pageProcedureName, p, CommandType.StoredProcedure);
            pageData.TotalCount = p.Get<int>("RecordCount");

            return pageData;
        }
        #endregion

        private static bool HasDangerousContents(string contents)
        {
            bool bReturnValue = false;
            if (!string.IsNullOrEmpty(contents))
            {
                //convert to lower
                string sLowerStr = contents.ToLower();
                //RegularExpressions
                string sRxStr = @"(select\s)|(insert\s)|
(delete\s)|(update\s[\s\S].*\sset)|(create\s)|(\stable)|(<[iframe|/iframe|script|/script])|
(')|(\sexec)|(\sdeclare)|(\struncate)|(\smaster)|(\sbackup)|(\smid)|(\scount)";
                //Match
                bool bIsMatch = false;
                System.Text.RegularExpressions.Regex sRx = new
                System.Text.RegularExpressions.Regex(sRxStr);
                bIsMatch = sRx.IsMatch(sLowerStr, 0);
                if (bIsMatch)
                {
                    bReturnValue = true;
                }
            }
            return bReturnValue;
        }
    }
}
