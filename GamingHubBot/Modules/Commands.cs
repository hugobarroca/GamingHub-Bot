using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        List<string> permittedRoles = new List<string>() { "Hunter", "Pirate", "S.W.A.T.", "Summoner", "Tiefling", "Waifu", "Freeloaders", "Impostor", "Bishop" };

        [Command("ping")]
        public async Task Ping()
        {
            var user = Context.User as SocketGuildUser;
            Console.WriteLine($"User \"{user}\" requested a ping.");
            await ReplyAsync("pong motherfucker");
        }

        [Command("help")]
        public async Task Help()
        {
            var user = Context.User as SocketGuildUser;
            Console.WriteLine($"User \"{user}\" requested help information.");
            string message = "";
            message += "**!help**: Displays this informative message.\n";
            message += "**!ping**: Respondes with a pong to confirm online status.\n";
            message += "**!permittedroles**: Lists all the roles that you can add or remove from yourself with this bot.\n";
            message += "**!addrole** *{role}*: Adds specified role to the user. Use quotations marks for roles with spaces.\n";
            message += "**!removerole** *{role}*: Removes specified role from the user. Use quotations marks for roles with spaces.\n";
            message += "**!catfact**: Gives you a random cat fact.";
            await ReplyAsync(message);
        }

        [Command("addrole")]
        public async Task AddRole(params String[] message)
        {
            var user = Context.User as SocketGuildUser;
            var roles = Context.Guild.Roles;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == message[0].ToLower());

            if (role == null)
            {
                Console.WriteLine($"User \"{user}\" tried to add itself to the role of \"{message[0]}\", but such role could not be found.");
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
        public async Task DailyCatFact()
        {
            var user = Context.User as SocketGuildUser;
            Console.WriteLine($"User \"{user}\" requested a cat fact!");
            CatFact catfact = await Program.GetCatFactAsync();
            await ReplyAsync(catfact.fact);
        }


        [Command("Arrr")]
        public async Task PirateTranslate(params String[] message)
        {
            string initial = "";

            foreach (var word in message)
            {
                initial += word;
            }

            var user = Context.User as SocketGuildUser;
            Console.WriteLine($"User \"{user}\" requested a pirate translation!");
            string final = await Program.GetPirateTranslationAsync(initial);
            await ReplyAsync(final);
        }

        [Command("Arr?")]
        public async Task TimeUntillSoTS2()
        {
            //Time until Thursday, 15 April 2021, 01:00:00
            //Get these values however you like.
            DateTime daysLeft = DateTime.Parse("4/15/2021 01:00:00 AM");
            DateTime startDate = DateTime.Now;

            //Calculate countdown timer.
            TimeSpan t = daysLeft - startDate;
            string countDown = "";

            Console.WriteLine("Someone requested time until SoT S2!");

            if (t.Days > 0)
            {
                countDown = string.Format("{0} Days, {1} Hours, {2} Minutes and {3} Seconds til Season 2 arrives mates! Arrrrr!", t.Days, t.Hours, t.Minutes, t.Seconds);
                await ReplyAsync(countDown);
                return;
            }

            if (t.Hours > 0)
            {
                countDown = string.Format("{0} Hours, {1} Minutes and {2} Seconds til Season 2 arrives mates! Arrrrr!", t.Hours, t.Minutes, t.Seconds);
                await ReplyAsync(countDown);
                return;
            }

            if (t.Minutes > 0)
            {
                countDown = string.Format("{0} Minutes and {1} Seconds til Season 2 arrives mates! Arrrrr!", t.Minutes, t.Seconds);
                await ReplyAsync(countDown);
                return;
            }

            if (t.Seconds > 0)
            {
                countDown = string.Format("{0} Seconds til Season 2 arrives mates! Arrrrr!", t.Seconds);
                await ReplyAsync(countDown);
                return;
            }

            countDown = string.Format("SEASON 2 IS HERE MATES! ARRRRR! LET'S GO GRAB SOME BOOTY, AYEEEEE!");
            await ReplyAsync(countDown);
            return;



        }

        [Command("permittedroles")]
        public async Task PermittedRoles()
        {
            var user = Context.User as SocketGuildUser;
            Console.WriteLine($"User \"{user}\" requested information on permitted roles.");

            string roles = "";
            int i = 0;
            foreach (var role in permittedRoles)
            {
                if (i == 0)
                {
                    roles += $"{role}";
                    i++;
                }
                else
                {
                    roles += $", {role}";
                }
            }
            await ReplyAsync($"**Permitted roles**: {roles}");
        }
    }
}