using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace LayIM.DAL.DB
{
    public class BaseDB
    {
        private static readonly string SqlConnectionString = ConfigurationManager.ConnectionStrings["SQL_CONNECTION_LAYIM"].ToString();

        #region ExecuteNonQuery
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuerySQL(string sql, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryStoreProcedure(string spName, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(CommandType.StoredProcedure, spName, parameters);
        }
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static int ExecuteNonQuery(CommandType commandType, string sql, params SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteNonQuery(SqlConnectionString, commandType, sql, parameters);
        }
        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// 查询Sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDateTableSQL(string sql, params SqlParameter[] parameters)
        {
            return ExecuteDateTable(CommandType.Text, sql, parameters);
        }
        /// <summary>
        /// 查询存储过程
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTableStoreProcedure(string spName, params SqlParameter[] parameters)
        {
            return ExecuteDateTable(CommandType.StoredProcedure, spName, parameters);
        }

        /// <summary>
        /// 查询存储过程
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSetStoreProcedure(string spName, params SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteDataset(SqlConnectionString, CommandType.StoredProcedure, spName, parameters);
        }

        /// <summary>
        /// SQL语句返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSetSQL(string sql, params SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteDataset(SqlConnectionString, CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 查询DataTable
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDateTable(CommandType commandType,string sql,params SqlParameter[] parameters)
        {
            var ds = SqlHelper.ExecuteDataset(SqlConnectionString, commandType, sql, parameters);
            if (ds.Tables.Count > 0) {
                return ds.Tables[0];
            }
            throw new ArgumentNullException("query result has no tables");
        }
        #endregion 

        public static SqlParameter MakeParameter(string parameter,SqlDbType dbType, object value)
        {
            if (parameter.IndexOf("@")!=0) {
                parameter = string.Concat("@", parameter);
            }
            var p =  new SqlParameter(parameter, dbType, 0);
            p.Value = value;
            return p;
        }

        public static SqlParameter MakeParameterInt(string parameter, int value)
        {
            return MakeParameter(parameter, SqlDbType.Int, value);
        }
        public static SqlParameter MakeParameterBit(string parameter, bool value)
        {
            return MakeParameter(parameter, SqlDbType.Bit, value);
        }
        public static SqlParameter MakeParameterVarChar(string parameter, string value)
        {
            return MakeParameter(parameter, SqlDbType.VarChar, value);
        }
        public static SqlParameter MakeParameterDecimal(string parameter, float value)
        {
            return MakeParameter(parameter, SqlDbType.Decimal, value);
        }

        public static SqlParameter MakeParameterTinyInt(string parameter, short value)
        {
            return MakeParameter(parameter, SqlDbType.TinyInt, value);
        }



    }
}