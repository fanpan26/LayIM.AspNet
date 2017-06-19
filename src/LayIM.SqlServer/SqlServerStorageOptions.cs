using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.SqlServer
{
    public class SqlServerStorageOptions
    {
        private string _schemaName;

        public SqlServerStorageOptions()
        {
            _schemaName = Constants.DefaultSchema;
        }

        public string SchemaName
        {
            get
            {
                return _schemaName;
            }
            set
            {
                _schemaName = value;
            }
        }
    }
}
