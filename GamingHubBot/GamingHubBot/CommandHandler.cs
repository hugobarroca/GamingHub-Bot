using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using GamingHubBot.Application;
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
        private readonly InteractionService _interactionService;
        private readonly IServiceProvider _services;
        private Dictionary<string, string> _roleByEmoji;

        public CommandHandler(DiscordSocketClient client, InteractionService interactionService, IServiceProvider services)
        {
            _interactionService = interactionService;
            _client = client;
            _services = services;
            _roleByEmoji = new Dictionary<string, string>();
            PopulateRoleByEmoji();
        }

        public async Task InitializeAsync()
        {
            await _interactionService.AddModuleAsync<InfoModule>(_services);
            await _interactionService.AddModuleAsync<RoleModule>(_services);
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.InteractionCreated += HandleInteraction;

            _interactionService.SlashCommandExecuted += SlashCommandExecuted;
            _interactionService.ContextCommandExecuted += ContextCommandExecuted;
            _interactionService.ComponentCommandExecuted += ComponentCommandExecuted;

            _client.ReactionAdded += HandleReactionAddedAsync;
            _client.ReactionRemoved += HandleReactionRemovedAsync;


        }



        private Task ComponentCommandExecuted(ComponentCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task ContextCommandExecuted(ContextCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                var ctx = new SocketInteractionContext(_client, arg);
                await _interactionService.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (arg.Type == InteractionType.ApplicationCommand)
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
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
