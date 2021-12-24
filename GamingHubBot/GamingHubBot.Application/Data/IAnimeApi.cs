using GamingHubBot.Application.Entities;

namespace GamingHubBot.Infrastructure.Gateways
{
    public interface IAnimeApi
    {
        public Task<AnimeQuote> GetRandomAnimeQuote();
    }
}
