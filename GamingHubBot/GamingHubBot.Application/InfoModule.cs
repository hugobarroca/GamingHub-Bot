namespace GamingHubBot.Application
{
    using Discord.Interactions;
    using Discord.WebSocket;
    using GamingHubBot.Infrastructure.Gateways;
    using global::GamingHubBot.Data;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;

    public class InfoModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IDataAccess _dataAccess;
        private readonly IAnimeApi _animeApi;
        private readonly ILogger<InfoModule> _logger;

        public InfoModule(ILogger<InfoModule> logger, IDataAccess dataAccess, IAnimeApi animeApi)
        {
            _logger = logger;
            _dataAccess = dataAccess;
            _animeApi = animeApi;
        }

        private readonly ulong _gameMasterId = 207178008706940928;
        List<string> permittedRoles = new List<string>() { "Bishop", "Freeloaders", "Ghost", "Hunter", "Impostor", "Pirate", "Summoner", "S.W.A.T.", "Tiefling", "Waifu" };

        [SlashCommand("echo", "Echo an input")]
        public async Task Echo(string input)
        {
            var user = Context.User as SocketGuildUser;
            _logger.LogInformation($"User \"{user.Username}\" echoed a message.");
            await RespondAsync(input);
        }

        [SlashCommand("ping", "Sends a ping to the bot to verify connection status.")]
        public async Task Ping()
        {
            var user = Context.User as SocketGuildUser;
            _logger.LogInformation($"User \"{user.Username}\" requested a ping.");
            await RespondAsync("Bot is currently active!", ephemeral: true);
        }

        [SlashCommand("weeb", "Returns a random anime quote.")]
        public async Task AnimeQuote()
        {
            var user = Context.User as SocketGuildUser;
            _logger.LogInformation($"User \"{user}\" requested an anime quote!");

            var animeQuote = await _animeApi.GetRandomAnimeQuote();

            await RespondAsync($"\"{animeQuote.Quote}\"\n-{animeQuote.Character}, from {animeQuote.Anime}");
        }

    }
}

