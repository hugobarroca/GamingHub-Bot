using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GamingHubBot
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private Dictionary<string, string> _roleByEmoji;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _commands = commands;
            _client = client;
            _services = services;
            _roleByEmoji = new Dictionary<string, string>();
            PopulateRoleByEmoji();
        }

        public async Task InstallCommandsAsync()
        {
            //Event handlers
            _client.MessageReceived += HandleCommandAsync;
            _client.ReactionAdded += HandleReactionAddedAsync;
            _client.ReactionRemoved += HandleReactionRemovedAsync;


            await _commands.AddModuleAsync<InfoModule>(_services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }

        public async Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> cachedMessage, Cacheable<IMessageChannel, ulong> originChannel, SocketReaction reaction)
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

        private void PopulateRoleByEmoji()
        {
            _roleByEmoji.Add("🔪", "Impostor");
            _roleByEmoji.Add("♟️", "Bishop");
            _roleByEmoji.Add("🦅", "S.W.A.T.");
            _roleByEmoji.Add("🧝", "Tiefling");
            _roleByEmoji.Add("🤸‍♀️", "Waifu");
            _roleByEmoji.Add("🧂", "Summoner");
            _roleByEmoji.Add("🐉", "Hunter");
            _roleByEmoji.Add("🏴‍☠️", "Pirate");
            _roleByEmoji.Add("👻", "Ghost");
            _roleByEmoji.Add("🤑", "Freeloaders");
        }

        public async Task HandleReactionRemovedAsync(Cacheable<IUserMessage, ulong> cachedMessage,
        Cacheable<IMessageChannel, ulong> originChannel, SocketReaction reaction)
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
