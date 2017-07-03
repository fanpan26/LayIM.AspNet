using LayIM.NetClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.SqlServer
{
    public class SqlServerStorage : LayimStorage
    {

        private readonly string _connectionString;
        private readonly SqlServerStorageOptions _options;

        public SqlServerStorageOptions Options => _options;
        public SqlServerStorage(string nameOrConnectionString) : 
            this(nameOrConnectionString, new SqlServerStorageOptions())
        {

        }

        public SqlServerStorage(string nameOrConnectionString, SqlServerStorageOptions options)
        {
            Error.ThrowIfNull(nameOrConnectionString, nameof(nameOrConnectionString));
            Error.ThrowIfNull(options, nameof(options));

            _connectionString = GetConnectionString(nameOrConnectionString);
            _options = options;

            //初始化数据库内容
            //using (var connection = CreateAndOpenConnection())
            //{
            //    SqlServerObjectsInstaller.Install(connection, "");
            //}
        }

        public override IStorageConnection GetConnection()
        {
            return new SqlServerConnection(this);
        }


        #region connection

        /// <summary>
        /// 无返回值的操作
        /// </summary>
        /// <param name="action"></param>
        internal void UseConnection(Action<DbConnection> action)
        {
            UseConnection(connection =>
            {
                action(connection);
                return true;
            });
        }

        /// <summary>
        /// 返回值为T的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        internal T UseConnection<T>(Func<DbConnection, T> func)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return func(connection);
            }
        }

        internal Task<T> UseConnectionAsync<T>(Func<DbConnection, Task<T>> func)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return func(connection);
            }
        }

        /// <summary>
        /// 是否存在连接
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal bool IsExistingConnection(IDbConnection connection)
        {
            return connection != null;// && ReferenceEquals(connection, _existingConnection);
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="connection"></param>
        internal void ReleaseConnection(IDbConnection connection)
        {
            if (connection != null && !IsExistingConnection(connection))
            {
                connection.Dispose();
            }
        }

        /// <summary>
        /// 是否是连接字符串
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        /// <returns></returns>
        private bool IsConnectionString(string nameOrConnectionString)
        {
            return nameOrConnectionString.Contains(";");
        }

        /// <summary>
        /// 是否在配置文件中配置
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        private bool IsConnectionStringInConfiguration(string connectionStringName)
        {
            var connectionStringSetting = ConfigurationManager.ConnectionStrings[connectionStringName];
            return connectionStringSetting != null;
        }

        private string GetConnectionString(string nameOrConnectionString)
        {
            if (IsConnectionString(nameOrConnectionString)) {
                return nameOrConnectionString;
            }

            if (IsConnectionStringInConfiguration(nameOrConnectionString)) {
                return ConfigurationManager.ConnectionStrings[nameOrConnectionString].ConnectionString;
            }
            throw new ArgumentException($"配置文件中不存在[{nameOrConnectionString}]");
        }
        #endregion
    }
}
