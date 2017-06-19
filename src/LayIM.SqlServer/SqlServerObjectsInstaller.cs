using LayIM.NetClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace LayIM.SqlServer
{
    internal static class SqlServerObjectsInstaller
    {
        public static void Install(DbConnection connection, string schema)
        {
            Error.ThrowIfNull(connection, nameof(connection));

            var script = GetStringResource(
                typeof(SqlServerObjectsInstaller).GetTypeInfo().Assembly,
                "LayIM.SqlServer.Install.sql");


            script = script.Replace("$(LayIMSchema)", !string.IsNullOrWhiteSpace(schema) ? schema : Constants.DefaultSchema);

            connection.Execute(script, commandTimeout: 0);
        }

        private static string GetStringResource(Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException(
                        $"资源文件 Install.sql 不存在.");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
