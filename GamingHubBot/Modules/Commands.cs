using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace GamingHubBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong motherfucker");
        }

        [Command("help")]
        public async Task Help()
        {
            string message = "";
            message += "**!help**: Displays this informative message.\n";
            message += "**!ping**: Respondes with a pong to confirm online status.\n";
            await ReplyAsync(message);
        }

        [Command("echo")]
        public async Task Echo(params String[] message)
        {
            var User = Context.User as SocketGuildUser;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Game Master");

            if (!User.Roles.Contains(role))
            {
                Console.WriteLine("The person IS the game master.");
                await ReplyAsync("Nice!");
            }

            if (Context.User is SocketGuildUser socketUser)
            {
                SocketGuild socketGuild = socketUser.Guild;
                SocketRole socketRole = socketGuild.GetRole(772788208500211724);
                if (socketUser.Roles.Any(r => r.Id == socketRole.Id))
                {
                    await Context.Channel.SendMessageAsync("The user '" + socketUser.Username + "' already has the role '" + socketRole.Name + "'!");
                }
                else
                {
                    //await socketUser.AddRoleAsync(socketRole);
                    await Context.Channel.SendMessageAsync("Added Role '" + socketRole.Name + "' to '" + socketUser.Username + "'!");
                }
            }
            await ReplyAsync(message[0]);
        }

        [Command("role")]
        public async Task Role(params String[] message)
        {
            await ReplyAsync($"Your role: {message[0]}");
        }
    }
}