using Dapper;
using LayIM.DataAccessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.DataAccessLayer.Helper
{

    public class DBHelper
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["LayIM_SQLConnectionString"].ConnectionString;


        #region 获取SQL连接
        private static IDbConnection getConnection()
        {
            try
            {
                IDbConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                return connection;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        #endregion


        #region 执行单条SQL语句
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool Execute(String sql, Object param)
        {
            using (IDbConnection connection = getConnection())
            {
                int result = connection.Execute(sql, param);

                return result > 0;
            }
        }
        #endregion

        #region 执行查询某个字段
        public static T ExecuteScalar<T>(string sql, DynamicParameters parameters)
        {
            using (IDbConnection connection = getConnection())
            {
                return connection.ExecuteScalar<T>(sql, parameters);
            }
        }
        #endregion

        #region 执行多条带事物的SQL语句或者存储过程 （要么都是存储过程，要么都是SQL语句）
        /// <summary>
        ///带事物的执行方法
        /// </summary>
        /// <param name="param">一个sql和参数组合字典</param>
        /// <param name="openTransaction">是否开启事物</param>
        /// <returns>返回是否执行成功</returns>
        public static bool Execute(Dictionary<string, Object> param, bool openTransaction = true, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection connection = getConnection())
            {
                IDbTransaction trans = null;
                if (openTransaction)
                {
                    trans = connection.BeginTransaction();
                }
                try
                {
                    foreach (KeyValuePair<String, Object> kv in param)
                    {
                        /*
                         * 如果分配给命令的连接位于本地挂起事务中，ExecuteNonQuery 要求命令拥有事务。命令的 Transaction 属性尚未初始化
                         * 解决：trans参数要传进去
                         */
                        connection.Execute(kv.Key, kv.Value, trans, null, commandType);
                    }
                    if (trans != null)
                    {
                        trans.Commit();
                    }
                    return true;
                }
                catch (SqlException ex)
                {

                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    return false;
                }
            }
        }
        #endregion

        #region 执行查询
        public static IEnumerable<T> Query<T>(String sql, DynamicParameters parameters, CommandType commandType)
        {
            using (IDbConnection connection = getConnection())
            {
                return connection.Query<T>(sql, parameters, commandType: commandType);
            }
        }

        public static IEnumerable<T> Query<T>(String sql)
        {
            return Query<T>(sql, null, CommandType.Text);
        }
        #endregion

        #region 查询多表
        public static T QueryMultiple<T>(string sql, object param,CommandType commandType = CommandType.Text, IMultipleHandler<T> handler=null)
        {
            using (var connection = getConnection())
            {
                using (var multi = connection.QueryMultiple(sql, param,commandType: commandType))
                {
                    if (handler == null) {
                        return default(T);
                    }
                    return handler.Handle(multi);
                }
            }
        }

        public static T QueryMultiple<T>(string sql, object param, CommandType commandType = CommandType.Text, Func<SqlMapper.GridReader,T> readerCallBack=null)
        {
            using (var connection = getConnection())
            {
                using (var multi = connection.QueryMultiple(sql, param, commandType: commandType))
                {
                    if (readerCallBack == null)
                    {
                        return default(T);
                    }
                    return readerCallBack(multi);
                }
            }
        }
        #endregion
    }
}
