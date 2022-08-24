using BalcaoUnicoDataAcess.DBAcess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalcaoUnicoDataAcess.DbAcess
{
    public class IRepository<T> where T: class
    {
        private DbSession _db;

        public IRepository(DbSession dbSession)
        {
            _db = dbSession;
        }

        public async Task<List<T>> Get(string sqlQuery, object sqlParams)
        {
            using(IDbConnection dbCon = _db.Connection)
            {
                List<T> queryResult = (await dbCon.QueryAsync<T>(sql: sqlQuery, param: sqlParams)).ToList();
                return queryResult;
            }
        }

        public async Task<int> Execute(string sqlQuery, T insertParams)
        {
            using (IDbConnection dbCon = _db.Connection)
            {
                int queryResult = await dbCon.ExecuteAsync(sql: sqlQuery, param: insertParams);
                return queryResult;
            }
        }
    }
}
