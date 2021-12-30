using Dapper;
using GamingHubBot.Application.Configuration;
using GamingHubBot.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Color = GamingHubBot.Application.Entities.Color;
using ColorModel = GamingHubBot.Infrastructure.Repositories.Models.Color;

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

        public async Task<List<Color>> GetColors()
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve colors from database...");

                var connectionId = _options.DBConnection;

                string sql = "SELECT * FROM Colors";

                using (var conn = new MySqlConnection(_options.DBConnection))
                {
                    IEnumerable<ColorModel> colorModels = conn.Query<ColorModel>(sql);
                    var colors = new List<Color>();

                    foreach (var color in colorModels)
                    {
                        colors.Add(new Color
                        {
                            Id = color.Id,
                            Name = color.Name,
                            Red = color.Red,
                            Blue = color.Blue,
                            Green = color.Green,
                        });
                    }
                    _logger.LogInformation("Colors retrieved from the database successfuly!");

                    return colors;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }
    }
}
