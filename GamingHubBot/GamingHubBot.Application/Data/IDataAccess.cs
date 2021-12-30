using GamingHubBot.Application.Entities;


namespace GamingHubBot.Data
{
    public interface IDataAccess
    {
        public Task<List<Color>> GetColors();
    }
}
