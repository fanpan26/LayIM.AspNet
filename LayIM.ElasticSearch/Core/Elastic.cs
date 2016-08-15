using Macrosage.ElasticSearch.Model;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Mappings;
using System.Reflection;
using Macrosage.ElasticSearch.Log;

namespace Macrosage.ElasticSearch.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Elastic<T> where T : BaseEntity, new()
    {
        #region 单例
        private static Elastic<T> _instance;
        private static object _lock = new object();
        public static Elastic<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {

                        _instance = _instance ?? new Elastic<T>();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region 成员变量

        private ElasticConnection Client;
        private JsonNetSerializer serializer;
        private string _hostName;
        private int _port;
        public string HostName { get { return _hostName; } }
        public int Port { get { return _port; } }

        private IndexInfo _indexInfo;
        public IndexInfo IndexInfo { get { return _indexInfo; } }
        #endregion

        #region 构造函数

        public Elastic(string hostName, int port)
        {
            this._hostName = hostName ?? ElasticConfig.HostName;
            this._port = port == 0 ? ElasticConfig.Port : port;
            Client = new ElasticConnection(_hostName, _port, ElasticConfig.TimeOutSeconds);
            serializer = new JsonNetSerializer();
        }
        public Elastic() : this(null, 0)
        {

        }
        #endregion

        #region 帮助方法
        /// <summary>
        /// 初始化的时候要执行这个方法
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="indexType"></param>
        public void SetIndexInfo(string indexName, string indexType)
        {
            if (indexName == null || indexType == null) { throw new ArgumentNullException(); }
            this._indexInfo = new IndexInfo { IndexName = indexName.ToLowerInvariant(), IndexType = indexType.ToLowerInvariant() };
        }

        #region 索引一条记录
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonDocument"></param>
        /// <returns></returns>
        public bool Index(string id, string jsonDocument)
        {
            if (_indexInfo == null) { throw new ArgumentNullException(); }
            string cmd = new IndexCommand(_indexInfo.IndexName, _indexInfo.IndexType, id);
            OperationResult result = Client.Put(cmd, jsonDocument);
            var indexResult = serializer.ToIndexResult(result.Result);
            return indexResult.created;
        }

        public bool Index(T entity)
        {
            return Index(entity.id, serializer.Serialize(entity));
        }
        #endregion

        #region 根据 Id获取某条记录

        public T QuerySingle(string id)
        {
            try
            {
                string getCommand = new GetCommand(_indexInfo.IndexName, _indexInfo.IndexType, id);
                OperationResult result = Client.Get(getCommand);
                var getResult = serializer.ToGetResult<T>(result.Result);
                if (getResult.found)
                {
                    return getResult.Document;
                }
                return null;
            }
            catch (OperationException ex) {
                return null;
            }
        }
        #endregion

        #region 根据具体条件查询（单一条件）
        public BaseQueryEntity<T> QueryInTerm(string field, string value)
        {
            string cmd = CreateSearchCommand();
            string query = new QueryBuilder<T>()
                .Query(q =>
                            q.Filtered(f =>
                                            f.Filter(f1 =>
                                                            f1.Term(t =>
                                                                        t.Field(field).Value(value))))).Build();
            OperationResult result = Client.Post(cmd, query);
            var data = serializer.ToSearchResult<T>(result.Result);
            return GetResults(data);
        }
        /// <summary>
        /// 根据单一条件查询
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">内容</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页多少条</param>
        /// <returns></returns>
        public BaseQueryEntity<T> QueryInTerm(string field, string value, string orderField = "", string orderType = "asc", int pageIndex = 1, int pageSize = 20)
        {
            string cmd = CreateSearchCommand();
            string query = new QueryBuilder<T>()
                .Query(q =>
                            q.Filtered(f =>
                                            f.Filter(f1 =>
                                                            f1.Term(t =>
                                                                        t.Field(field).Value(value)))))
                                                                        .From((pageIndex - 1) * pageSize)
                                                                        .Size(pageSize)
                                                                        .Sort(x => x.Field(orderField, orderType == "asc" ? SortDirection.asc : SortDirection.desc)).Build();

            OperationResult result = Client.Post(cmd, query);
            var data = serializer.ToSearchResult<T>(result.Result);
            return GetResults(data);
        }
        #endregion

        #region 根据query条件查询
        public BaseQueryEntity<T> QueryBayConditions(string query)
        {
            try
            {
                string cmd = CreateSearchCommand();//构造查询命令
                OperationResult result = Client.Post(cmd, query);//Post query参数查询
                var data = serializer.ToSearchResult<T>(result.Result); //返回结果转换
                return GetResults(data);
            }
            catch (Exception ex)
            {
                ESLog.WriteLogException(MethodBase.GetCurrentMethod(), "查询条件：" + query);
                ESLog.WriteLogException(MethodBase.GetCurrentMethod(), ex);
                return new BaseQueryEntity<T>();
            }
        }
        #endregion

        #region 根据条件删除数据
        /// <summary>
        /// 根据单一条件查询删除
        /// </summary>
        /// <param name="query">例如(字符串) {"artid":1000000000}</param>
        /// <returns></returns>
        public bool DeleteByQuery(string query)
        {
            try
            {
                var cmd = CreateDeleteByQueryCommand();
                var fullQuery = "{ \"query\": { \"term\": " + query + " }}";
                var result = QueryBayConditions(fullQuery);
                if (result.hits > 0)
                {
                    DeleteBulk(result.list);
                }
                return true;
            }
            catch (Exception ex)
            {
                ESLog.WriteLogException(MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        #endregion

        #region 删除索引
        /*
            功能：将整个索引删除，类似清除数据库中所有表
            调用：谨慎调用
            正确结果：{"acknowledged":true}
            */
        public bool DeleteIndex()
        {
            var cmd = "/" + _indexInfo.IndexName;
            var result = Client.Delete(cmd);
            return true;
        }

        public string DeleteIndex(string indexName)
        {
            try
            {
                var cmd = "/" + indexName;
                var result = Client.Delete(cmd);
                return result;
            }
            catch (Exception ex) {
                ESLog.WriteLogException(MethodBase.GetCurrentMethod(), ex);
                return ex.Message;
            }
        }
        #endregion

        #region 模糊查询

        public BaseQueryEntity<T> QueryInFields(string value,string orderField="",string orderType="asc", int pageIndex=1, int pageSize=20)
        {
            string cmd = CreateSearchCommand();
            string query = new QueryBuilder<T>()
                .Query(q => q.Match(t => t.Field("tagname").Query(value)))
                                                                  .From((pageIndex - 1) * pageSize)
                                                                  .Size(pageSize)
                                                                  .Sort(x => x.Field(orderField, orderType == "asc" ? SortDirection.asc : SortDirection.desc)).Build();
            //string query = "{\"query\": {\"match\": {\"tagname\": \"" + value + "\"}}}";
            OperationResult result = Client.Post(cmd, query);
            var data = serializer.ToSearchResult<T>(result.Result);
            return GetResults(data);
            #region 其他注释
            ////排序
            //// .Sort(c => c.Field("age", SortDirection.desc))
            ////添加高亮
            //.Highlight((Highlight<person> h) => h
            //    .PreTags("<b>")
            //    .PostTags("</b>")
            //    .Fields(
            //           (HighlightField<person> f) => f.FieldName("intro").Order(HighlightOrder.score),
            //           (HighlightField<person> f) => f.FieldName("_all")
            //           )
            //   )
            //  .Build();
            #endregion
        }

        #endregion

        #region bulk操作
        /// <summary>
        /// 批量创建或者更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool IndexBulk(IEnumerable<T> list)
        {
            try
            {
                string cmd = CreateBulkCommand();
                BulkBuilder builder = new BulkBuilder(serializer);
                StringBuilder str = new StringBuilder();
                OperationResult result;
                foreach (var item in list)
                {
                    str.Append(builder.Index(item, _indexInfo.IndexName, _indexInfo.IndexType, item.id));
                }
                //最后一组
                if (str.Length > 0) {
                    result = Client.Post(cmd, str.ToString()); 
                }
               
                return true;
            }
            catch (Exception ex)
            {
                ESLog.WriteLogException(MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }

        #region 批量删除
        public bool DeleteBulk(IEnumerable<T> list)
        {
            try
            {
                string cmd = CreateBulkCommand();
                BulkBuilder builder = new BulkBuilder(serializer);
                StringBuilder str = new StringBuilder();
                OperationResult result;
                foreach (var item in list)
                {
                    str.Append(builder.Delete(item.id, _indexInfo.IndexName, _indexInfo.IndexType));
                }
                //最后一组
                if (str.Length > 0)
                {
                    result = Client.Post(cmd, str.ToString());
                }

                return true;
            }
            catch (Exception ex)
            {
                ESLog.WriteLogException(MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        #endregion

        #region 批量添加或者更新
        public bool IndexBulk(IEnumerable<T> list, bool mulityThread)
        {
            if (!mulityThread)
            {
                return IndexBulk(list);
            }
            else
            {
                var lists = new List<List<T>>();
                var count = list.Count();
                var pageSize = 2000;//2000条加一次
                var last = count % pageSize;//剩余条数
                var pageCount = count % pageSize == 0 ? count / pageSize : count / pageSize + 1;
                for (var i = 0; i < pageCount; i++)
                {
                    if (i == pageCount - 1 && last > 0)
                    {
                        lists.Add(list.Skip(i * pageSize).Take(last).ToList());
                    }
                    else
                    {
                        lists.Add(list.Skip(i * pageSize).Take(pageSize).ToList());
                    }
                }
                Parallel.ForEach(lists, item => {
                    IndexBulk(item);
                });
                return true;
            }
        }
        #endregion

        #region 批量创建
        /// <summary>
        /// 批量创建 1000-5000个比较好
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string CreateBulk(IEnumerable<T> list)
        {
            string cmd = CreateBulkCommand();
            BulkBuilder builder = new BulkBuilder(serializer);
            StringBuilder str = new StringBuilder();
            foreach (var item in list)
            {

                str.Append(builder.Create(item, _indexInfo.IndexName, _indexInfo.IndexType, item.id));
            }
            OperationResult result = Client.Post(cmd, str.ToString());
            return result;
        }
        #endregion
        #endregion

        #region 私有方法，处理查询后的结果
        private BaseQueryEntity<T> GetResults(SearchResult<T> result)
        {
            BaseQueryEntity<T> baseEntity = new BaseQueryEntity<T>();
            baseEntity.took = result.took;
            baseEntity.hits = result.hits.total;
            baseEntity.list = HitsToList(result.hits);
            return baseEntity;
        }

        public virtual IEnumerable<T> HitsToList(SearchResult<T>.SearchHits hits)
        {
            var result = new List<T>();

            var hitsList = hits.hits.ToList();
            hitsList.ForEach(x => {
                result.Add(x._source);
            });
            return result;
        }
        #endregion

        #region 私有方法，创建命令
        private SearchCommand CreateSearchCommand()
        {
            return new SearchCommand(_indexInfo.IndexName, _indexInfo.IndexType);
        }
        private DeleteByQueryCommand CreateDeleteByQueryCommand()
        {
            return new DeleteByQueryCommand(_indexInfo.IndexName,_indexInfo.IndexType);
        }
        private BulkCommand CreateBulkCommand()
        {
            return new BulkCommand(_indexInfo.IndexName, _indexInfo.IndexType);
        }
        private PutMappingCommand CreatePutMappingCommand()
        {
            return new PutMappingCommand(_indexInfo.IndexName, _indexInfo.IndexType);
        }
        #endregion

        #endregion

        #region 系统表初始化 date字符串类型不解析等

        #region 表设置date字段

        /*
        功能：将表date字段设置为string类型，不被解析，防止其他类型parse出现异常，例如2015-02-01，'some name' 前者能转换为日期，后者不能（都为字符串）
        调用：只能在没有创建index的时候调用，否则会出现 index_already_exists_exception
        异常： {"error":{"root_cause":[{"type":"index_already_exists_exception","reason":"already exists","index":"ms_mobile"}],
                "type":"index_already_exists_exception","reason":"already exists","index":"ms_mobile"},"status":400}
                无法连接到远程服务器 
        正确返回结果：{"acknowledged":true}                  
            */
        public bool SetDateDetection(bool dateDetection = true)
        {
            var cmd = "/" + _indexInfo.IndexName;
            var param = "{\"mappings\":{\"" + _indexInfo.IndexType + "\":{\"date_detection\":" + dateDetection.ToString().ToLowerInvariant() + "}}}";
            try
            {
                OperationResult result = Client.Put(cmd, param);
                return true;
            }
            catch (Exception ex)
            {
                ESLog.WriteLogException(MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        #endregion

        #endregion

        #region  测试代码
        /*
        #region QueryTest
        public string GetQueryTest()
        {
            //单纯 term方法
            //string queryTerm = new QueryBuilder<T>()
            //   .Query(q =>
            //               q.Filtered(f =>
            //                               f.Filter(f1 =>
            //                                               f1.Term(t =>
            //                                                           t.Field(field).Value(value))))).Build();
            //单纯terms方法
            //string queryTerms = new QueryBuilder<T>()
            //    .Query(q =>
            //               q.Filtered(f =>
            //                               f.Filter(f1 => 
            //                                              f1.Terms(ts => 
            //                                                             ts.Field("title").Values(new string[] { "v1", "v2" }))))).Build();
            //string queryMoreTerm = new QueryBuilder<T>()
            //    .Query(q => q.Filtered(f => f.Filter(b => b.Bool(m1 => m1.Must(m => m.Term(t1 => t1.Field("title").Value("title1")).Term(t2 => t2.Field("title2").Value("title2"))))))).BuildBeautified();

            string queryRange = new QueryBuilder<T>()
               .Query(q => q.Filtered(f => f.Filter(f1 => f1.Range(r => r.Field("age").Gt("1").Lt("10"))))).BuildBeautified();
            return queryRange;
        }
        #endregion

        /*
        //分词映射
        private static string BuildCompanyMapping()
        {
            return new MapBuilder<person>()
                .RootObject(typeName: "person",
                            map: r => r
                    .All(a => a.Enabled(false))
                    .Dynamic(false)
                    .Properties(pr => pr
                        .String(person => person.name, f => f.Analyzer(DefaultAnalyzers.standard).Boost(2))
                        .String(person => person.intro, f => f.Analyzer("ik"))
                        )
              )
              .BuildBeautified();
        }

        //将语句用ik分词，返回分词结果的集合
        private List<string> GetIKTokenFromStr string key)
        {
            string s = "/db_test/_analyze?analyzer=ik";
            var result = Client.Post(s, "{"+key+"}");
            var serializer = new JsonNetSerializer();
            var list = serializer.Deserialize(result, typeof(ik)) as ik;            
            return list.tokens.Select(c=>c.token).ToList();
        }

        */
        #endregion
    }

    /// <summary>
    /// 批量操作类型
    /// </summary>
    public enum BulkType
    {
        Create = 0,
        Index = 1,
        Delete = 2
    }
}
