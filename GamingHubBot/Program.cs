using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using GamingHubBot.Models;
using GamingHubBot.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GamingHubBot
{
    class Program
    {

        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private Dictionary<string, string> _roleByEmoji;
        static HttpClient _apiClient = new HttpClient();
        private DiscordSocketClient _client;
        private CommandService _commands;
        private LoggingService _logger;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {

            _roleByEmoji = new Dictionary<string, string>();
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _logger = new LoggingService(_client, _commands);
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            PopulateRoles();
            string token = File.ReadAllText("token.txt");

            _client.Log += _client_Log;

            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();


            HookReactionAdded(_client);
            HookReactionRemoved(_client);

            await Task.Delay(-1);

            _apiClient.BaseAddress = new Uri("https://cat-fact.herokuapp.com/facts");
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        static public async Task<CatFact> GetCatFactAsync()
        {
            CatFact fact = null;
            HttpResponseMessage response = await _apiClient.GetAsync("https://catfact.ninja/fact");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var pirate = JsonConvert.DeserializeObject<Pirate>(jsonString);
            }
            return fact;
        }

        static public async Task<string> GetPirateTranslationAsync(string text)
        {
            string translation = "";

            string request = "https://api.funtranslations.com/translate/pirate.json?text=" + text;

            HttpResponseMessage response = await _apiClient.GetAsync(request);


            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                fact = JsonConvert.DeserializeObject<>(jsonString);
            }

            return translation;
        }


        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        private void PopulateRoles()
        {
            _roleByEmoji.Add("🔪", "Impostor");
            _roleByEmoji.Add("♟️", "Bishop");
            _roleByEmoji.Add("🦅", "S.W.A.T.");
            _roleByEmoji.Add("🧝", "Tiefling");
            _roleByEmoji.Add("🤸‍♀️", "Waifu");
            _roleByEmoji.Add("🧂", "Summoner");
            _roleByEmoji.Add("🐉", "Hunter");
            _roleByEmoji.Add("🏴‍☠️", "Pirate");
            _roleByEmoji.Add("🤑", "Freeloaders");
        }

        public void HookReactionAdded(BaseSocketClient client) => client.ReactionAdded += HandleReactionAddedAsync;

        public void HookReactionRemoved(BaseSocketClient client) => client.ReactionRemoved += HandleReactionRemovedAsync;

        public async Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            ulong guildId = 312101041380524032;
            var message = await cachedMessage.GetOrDownloadAsync();



            if (message != null && reaction.User.IsSpecified && message.Id == 661371005993746512)
            {
                string reactionName = reaction.Emote.Name;
                string role;

                _logger.writeFile("Reaction name: " + reactionName);

                if (_roleByEmoji.ContainsKey(reactionName))
                {
                    role = _roleByEmoji[reactionName];
                    _logger.writeFile("Role name: " + role);
                    var roleObject = _client.GetGuild(guildId).Roles.FirstOrDefault(x => x.Name.ToLower() == role.ToLower());
                    await _client.GetGuild(guildId).GetUser(reaction.UserId).AddRoleAsync(roleObject);
                    Console.WriteLine(reaction.User.ToString() + " added itself to the role of \"" + role + "\".");
                }
                else
                {
                    Console.WriteLine($"Key {reactionName} was not found");
                    return;
                }
            }
        }

        public async Task HandleReactionRemovedAsync(Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            ulong guildId = 312101041380524032;
            var message = await cachedMessage.GetOrDownloadAsync();



            if (message != null && reaction.User.IsSpecified && message.Id == 661371005993746512)
            {
                string reactionName = reaction.Emote.Name;
                string role;

                _logger.writeFile("Reaction name: " + reactionName);

                if (_roleByEmoji.ContainsKey(reactionName))
                {
                    role = _roleByEmoji[reactionName];
                    _logger.writeFile("Role name: " + role);
                    var roleObject = _client.GetGuild(guildId).Roles.FirstOrDefault(x => x.Name.ToLower() == role.ToLower());
                    await _client.GetGuild(guildId).GetUser(reaction.UserId).RemoveRoleAsync(roleObject);
                    Console.WriteLine(reaction.User.ToString() + " removed itself from the role of \"" + role + "\".");
                }
                else
                {
                    Console.WriteLine($"Key {reactionName} was not found");
                    return;
                }
            }
        }

    }
}
