using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

//using IHost host = Host.CreateDefaultBuilder(args).Build();

namespace GamingHubBot
{
    class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

            var connectionString = config.GetSection("connectionString");

            var bot = new GamingHubBot();
            bot.Start();
            await Task.Delay(-1);
        }

    }
}
