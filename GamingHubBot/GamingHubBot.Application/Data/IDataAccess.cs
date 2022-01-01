using GamingHubBot.Application.Entities;


namespace GamingHubBot.Data
{
    public interface IDataAccess
    {
        public Task SynchronizeRolesAsync(IEnumerable<Role> roles);
        public Task<IEnumerable<Role>> GetRolesAsync();
        public Task<List<Color>> GetColorsAsync();
    }
}
