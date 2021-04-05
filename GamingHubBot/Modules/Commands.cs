using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GamingHubBot.Models;

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

            if (role == null)
            {
                Console.WriteLine($"User \"{user}\" tried to add itself to the role of \"{role}\", but such role could not be found.");
                await ReplyAsync("Could not find that role. Check for typing errors you doofus!");
                return;
            }

            if (user.Roles.Contains(role))
            {
                Console.WriteLine($"User \"{user}\" tried to add itself to the role of \"{role}\", but he already belonged to that role.");
                await ReplyAsync($"You already have the role of {role}!");
            }
            else
            {
                if (permittedRoles.Contains(role.ToString()))
                {
                    Console.WriteLine($"User \"{user}\" added itself to the role of \"{role}\".");
                    await user.AddRoleAsync(role);
                    await ReplyAsync($"You have been given the role of {role}!");
                }
                else
                {
                    if (role.ToString() == "Game Master")
                    {
                        Console.WriteLine($"User \"{user}\" tried to add itself to the role of \"{role}\", which is not a permitted role.");
                        await ReplyAsync("Sneaky bastard aintcha...");
                    }
                    else
                    {
                        Console.WriteLine($"User \"{user}\" tried to add itself to the role of \"{role}\", which is not a permitted role.");
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
                Console.WriteLine($"User \"{user}\" tried to remove itself from the role of \"{role}\", but such role could not be found.");
                await ReplyAsync("Could not find that role. Check for typing errors you doofus!");
                return;
            }

            if (!user.Roles.Contains(role))
            {
                Console.WriteLine($"User \"{user}\" tried to remove itself from the role of \"{role}\", but he did not belong to that role.");
                await ReplyAsync($"You do not have the role of {role}!");
            }
            else
            {
                if (permittedRoles.Contains(role.ToString()))
                {
                    Console.WriteLine($"User \"{user}\" removed itself from the role of \"{role}\".");
                    await user.RemoveRoleAsync(role);
                    await ReplyAsync($"You have been removed from the role of {role}!");
                }
                else
                {
                    Console.WriteLine($"User \"{user}\" tried to remove itself from the role of \"{role}\", but that role is not in the permitted roles list.");
                    await ReplyAsync("Why would you try and do that? o.O");
                }
            }
        }

        [Command("catfact")]
        public async Task dailyCatFact()
        {
            var user = Context.User as SocketGuildUser;
            Console.WriteLine($"User \"{user}\" requested a cat fact!");
            CatFact catfact = await Program.GetCatFactAsync();
            await ReplyAsync(catfact.fact);
        }
    }
}