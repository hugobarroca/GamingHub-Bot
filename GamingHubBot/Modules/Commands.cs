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


            await ReplyAsync(message[0]);
        }

        [Command("addrole")]
        public async Task Role(params String[] message)
        {
            var User = Context.User as SocketGuildUser;
            var roles = Context.Guild.Roles;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == message[0]);
            List<string> permittedRoles = new List<string>() { "Hunter", "Pirate", "S.W.A.T." };

            Console.WriteLine($"Role was: {message[0]}");

            //foreach (SocketRole role in roles)
            //{
            //    Console.Write($"Role: {role}\n");
            //}

            if (User.Roles.Contains(role))
            {
                await ReplyAsync($"You already have the role of {role}!");
            }
            else
            {
                if (permittedRoles.Contains(role.ToString()))
                {
                    await User.AddRoleAsync(role);
                    await ReplyAsync($"You have been given the role of {role}!");
                }
                else
                {
                    await ReplyAsync("Sneaky bastard aintcha...");
                }
            }

            //if (Context.User is SocketGuildUser socketUser)
            //{
            //    SocketGuild socketGuild = socketUser.Guild;
            //    SocketRole socketRole = socketGuild.GetRole(772788208500211724);
            //    if (socketUser.Roles.Any(r => r.Id == socketRole.Id))
            //    {
            //        await Context.Channel.SendMessageAsync("The user '" + socketUser.Username + "' already has the role '" + socketRole.Name + "'!");
            //    }
            //    else
            //    {
            //        //await socketUser.AddRoleAsync(socketRole);
            //        await Context.Channel.SendMessageAsync("Added Role '" + socketRole.Name + "' to '" + socketUser.Username + "'!");
            //    }
            //}
        }
    }
}