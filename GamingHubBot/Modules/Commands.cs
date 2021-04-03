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
            message += "**!addrole** *{role}*: Adds specified role to the user. Use quotations marks for roles with spaces.\n";
            message += "**!removerole** *{role}*: Removes specified role from the user. Use quotations marks for roles with spaces.\n";
            await ReplyAsync(message);
        }

        [Command("echo")]
        public async Task Echo(params String[] message)
        {


            await ReplyAsync(message[0]);
        }

        [Command("addrole")]
        public async Task AddRole(params String[] message)
        {
            var user = Context.User as SocketGuildUser;
            var roles = Context.Guild.Roles;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == message[0].ToLower());
            List<string> permittedRoles = new List<string>() { "Hunter", "Pirate", "S.W.A.T.", "Summoner", "Tiefling", "Waifu", "Freeloaders", "Impostor", "Bishop" };

            if (role == null) {
                await ReplyAsync("Could not find that role. Check for typing errors you doofus!");
                return;
            }

            if (user.Roles.Contains(role))
            {
                await ReplyAsync($"You already have the role of {role}!");
            }
            else
            {
                if (permittedRoles.Contains(role.ToString()))
                {
                    await user.AddRoleAsync(role);
                    await ReplyAsync($"You have been given the role of {role}!");
                }
                else
                {
                    if (role.ToString() == "Game Master")
                    {
                        await ReplyAsync("Sneaky bastard aintcha...");
                    }
                    else {
                        await ReplyAsync("The role you tried to add is not a part of the permitted roles list!");
                    }
                }
            }
        }

        [Command("removerole")]
        public async Task RemoveRole(params String[] message)
        {
            var user = Context.User as SocketGuildUser;
            var roles = Context.Guild.Roles;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == message[0].ToLower());
            List<string> permittedRoles = new List<string>() { "Hunter", "Pirate", "S.W.A.T.", "Summoner", "Tiefling", "Waifu", "Freeloaders", "Impostor", "Bishop" };

            if (role == null)
            {
                await ReplyAsync("Could not find that role. Check for typing errors you doofus!");
                return;
            }

            if (!user.Roles.Contains(role))
            {
                await ReplyAsync($"You do not have the role of {role}!");
            }
            else
            {
                if (permittedRoles.Contains(role.ToString()))
                {
                    await user.RemoveRoleAsync(role);
                    await ReplyAsync($"You have been removed from the role of {role}!");
                }
                else
                {
                    await ReplyAsync("Why would you try and do that? o.O");
                }
            }
        }
    }
}