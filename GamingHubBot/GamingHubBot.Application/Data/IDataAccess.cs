using GamingHubBot.Application.Entities;


namespace GamingHubBot.Data
{
    public interface IDataAccess
    {
        public Task SynchronizeRolesAsync(IEnumerable<Role> rolesToInsert, IEnumerable<Role> rolesToRemove);
        public Task<IEnumerable<Role>> GetRolesAsync();
        public Task<List<Color>> GetColorsAsync();
    }
}
