using GamingHubBot.Data;
using GamingHubBot.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GamingHubBot.Infrastructure.Repositories.DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        IConfiguration _config;
        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        //public async Task<IEnumerable<SqlDataAccess>> GetAll()
        public void GetAll()
        {

        }

        public async Task<IEnumerable<DataEntity>> GetData()
        {
            var connectionId = "Default";
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

            return new List<DataEntity>();
        }
    }
}
