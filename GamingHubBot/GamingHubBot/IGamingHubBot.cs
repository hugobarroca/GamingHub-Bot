using Microsoft.Extensions.Configuration;

namespace GamingHubBot
{
    public interface IGamingHubBot
    {
        void Start(ConfigurationBuilder conf);
    }
}