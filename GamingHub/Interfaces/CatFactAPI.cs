using GamingHubBot.Infrastructure.Gateways;
using System.Threading.Tasks;

namespace GamingHubBot.Application.Interfaces
{
    public interface CatFactAPI
    {
        Task<CatFact> GetCatFact();
    }
}
