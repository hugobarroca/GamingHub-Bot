using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace GamingHubBot
{
    class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var bot = new GamingHubBot();
            bot.Start();
            await Task.Delay(-1);
        }

    }
}
