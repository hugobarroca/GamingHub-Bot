using GamingHubBot.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GamingHubBot.Data
{
    public interface IDataAccess
    {
        public string GetConnectionString();
        public Task<IEnumerable<DataEntity>> GetData();
    }
}
