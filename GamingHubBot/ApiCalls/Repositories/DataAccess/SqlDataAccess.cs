using Dapper;
using GamingHubBot.Application.Configuration;
using GamingHubBot.Data;
using GamingHubBot.Entities;
using GamingHubBot.Infrastructure.Repositories.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GamingHubBot.Infrastructure.Repositories.DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private readonly GeneralSettings _options;
        private readonly ILogger _logger;

        public SqlDataAccess(IOptions<GeneralSettings> options, ILogger<SqlDataAccess> logger)
        {
            _logger = logger;
            _options = options.Value;
        }

        public string GetConnectionString()
        {
            return _options.DBConnection;
        }

        public async Task<IEnumerable<DataEntity>> GetData()
        {
            try
            {
                var connectionId = _options.DBConnection;
                //using IDbConnection connection = new SqlConnection(_options.DBConnection);
                string sql = "SELECT * FROM Colors";

                using (var conn = new MySqlConnection(_options.DBConnection))
                {
                    var invoices = conn.Query<Color>(sql);
                }
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
                return null;
            }

            return new List<DataEntity>();
        }
    }
}
