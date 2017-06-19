using LayIM.NetClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.SqlServer
{
    public static class SqlServerStorageExtensions
    {
        /// <summary>
        /// 配置使用SqlServer
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="nameOrConnectionString"></param>
        /// <returns></returns>
        public static IGlobalConfiguration<SqlServerStorage> UseSqlServer(this IGlobalConfiguration configuration, string nameOrConnectionString)
        {
            Error.ThrowIfNull(configuration, nameof(configuration));
            Error.ThrowIfNull(nameOrConnectionString, nameof(nameOrConnectionString));

            var storage = new SqlServerStorage(nameOrConnectionString);
            return configuration.UseStorage(storage);
        }

        /// <summary>
        /// 配置使用SqlServer
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="nameOrConnectionString"></param>
        /// <returns></returns>
        public static IGlobalConfiguration<SqlServerStorage> UseSqlServer(
            this IGlobalConfiguration configuration,
            string nameOrConnectionString,
            SqlServerStorageOptions options)
        {
            Error.ThrowIfNull(configuration, nameof(configuration));
            Error.ThrowIfNull(nameOrConnectionString, nameof(nameOrConnectionString));
            Error.ThrowIfNull(options, nameof(options));

            var storage = new SqlServerStorage(nameOrConnectionString, options);
            return configuration.UseStorage(storage);
        }
    }
}
