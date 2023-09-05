using Epidi.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.ConnectionProvider
{
    public class ConnectionProvider : IConnectionProvider
    {
        readonly DatabaseType _databaseType;

        public ConnectionProvider()
        {
            _databaseType = DatabaseType.SQLServer;
        }
        public IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection dbConnection = null;
            switch (_databaseType)
            {
                case DatabaseType.SQLServer:
                    dbConnection = new SqlConnection(connectionString);
                    break;
            }
            return dbConnection;
        }
    }
}
