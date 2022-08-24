using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BalcaoUnicoDataAcess.DBAcess
{
    public class DbSession : IDisposable
    {
        public IDbConnection Connection { get;}
        public DbSession(IConfiguration configuration)
        {
            Connection = new OracleConnection(configuration.GetConnectionString("DefaultConnection"));
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
