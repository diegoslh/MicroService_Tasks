using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ISqlServerConnection
    {
        public Task<List<T>> ExecuteQuerySqlServerDb<T>(string query, Dictionary<string, object>? parameters) where T : new();
        public Task<int> ExecuteNonQuerySqlServerDb(string sql, Dictionary<string, object?> parameters);
    }
}
