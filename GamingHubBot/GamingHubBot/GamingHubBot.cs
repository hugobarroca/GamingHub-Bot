using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GamingHubBot
{
    public class GamingHubBot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private Dictionary<string, string> _roleByEmoji;

        public async void Start() 
        {
            _roleByEmoji = new Dictionary<string, string>();
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            PopulateRoles();

            var commandHandler = new CommandHandler(_client, _commands, _services);

            await commandHandler.InstallCommandsAsync();


            _client.Log += Log;

            var token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            HookReactionAdded(_client);
            HookReactionRemoved(_client);

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    

        public void Stop()
        {

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

            Console.WriteLine("Reaction name: " + reactionName);

            if (_roleByEmoji.ContainsKey(reactionName))
            {
                role = _roleByEmoji[reactionName];
                Console.WriteLine("Role name: " + role);
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

            Console.WriteLine("Reaction name: " + reactionName);

            if (_roleByEmoji.ContainsKey(reactionName))
            {
                role = _roleByEmoji[reactionName];
                Console.WriteLine("Role name: " + role);
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
