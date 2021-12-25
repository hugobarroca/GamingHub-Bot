using GamingHubBot.Application.Configuration;
using GamingHubBot.Data;
using GamingHubBot.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamingHubBot.Infrastructure.Repositories.DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private readonly ConnectionStringOptions _options;
        public SqlDataAccess(IOptions<ConnectionStringOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IEnumerable<DataEntity>> GetData()
        {
            var connectionId = _options.DBConnection;
            //using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

            return new List<DataEntity>();
        }
    }
}
