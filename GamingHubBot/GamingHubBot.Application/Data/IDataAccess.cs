using GamingHubBot.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GamingHubBot.Data
{
    public interface IDataAccess
    {
        public Task<IEnumerable<DataEntity>> GetData();
    }
}
