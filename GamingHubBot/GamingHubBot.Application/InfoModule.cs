﻿namespace GamingHubBot
{
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using GamingHubBot.Infrastructure.Gateways;
    using global::GamingHubBot.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDataAccess _dataAccess;
        private readonly IAnimeApi _animeApi;
        public InfoModule(IDataAccess dataAccess, IAnimeApi animeApi)
        {
            _dataAccess = dataAccess;
            _animeApi = animeApi;
        }

        private readonly ulong _gameMasterId = 207178008706940928;
        List<string> permittedRoles = new List<string>() { "Bishop", "Freeloaders", "Ghost", "Hunter", "Impostor", "Pirate", "Summoner", "S.W.A.T.", "Tiefling", "Waifu" };

        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
            => ReplyAsync(echo);

        [Command("help")]
        public async Task Help()
        {
            var user = Context.User as SocketGuildUser;
            Console.WriteLine($"User \"{user}\" requested help information.");
            string message = "";
            message += "**!help**: Displays this informative message.\n";
            message += "**!ping**: Respondes with a pong to confirm online status.\n";
            message += "**!createrole**: Creates a new role from scratch.\n";
            message += "**!permittedroles**: Lists all the roles that you can add or remove from yourself with this bot.\n";
            message += "**!addrole** *{role}*: Adds specified role to the user. Use quotations marks for roles with spaces.\n";
            message += "**!removerole** *{role}*: Removes specified role from the user. Use quotations marks for roles with spaces.\n";
            message += "**!weeb**: Gives you a random anime quote.\n";
            await ReplyAsync(message);
        }

        [Command("ping")]
        public async Task Ping()
        {
            var user = Context.User as SocketGuildUser;
            Console.WriteLine($"User \"{user}\" requested a ping.");
            await ReplyAsync("pong motherfucker");
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
                        Console.WriteLine($"User \"{user}\" tried to add itself to the role of \"{role}\", which is not a permitted role.\n");
                        await ReplyAsync("Sneaky bastard aintcha...");
                    }
                    else
                    {
                        Console.WriteLine($"User \"{user}\" tried to add itself to the role of \"{role}\", which is not a permitted role.\n");
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

        [Command("createrole")]
        public async Task CreateRole(params String[] message)
        {
            var user = Context.User;
            if (user.Id != _gameMasterId)
            {
                await ReplyAsync("You don't have permission to run this command as of now.");
            }

            var roleName = message[0];

            bool success = int.TryParse(message[1], out var red);
            success = int.TryParse(message[2], out var green) && success;
            success = int.TryParse(message[3], out var blue) && success;

            if (!success)
            {
                await ReplyAsync("The color values were not all valid integers.");
                return;
            }

            //var permissions = new GuildPermissions();
            var permissions = Context.Guild.GetUser(user.Id).GuildPermissions;
            var roleColor = new Color(red, green, blue);
            try
            {
                await Context.Guild.CreateRoleAsync(roleName, permissions, roleColor, false, true);
            }
            catch (Exception ex) 
            {
                await ReplyAsync($"An exception has occurred. I am terribly sorry.");
                Console.WriteLine(ex);
            }

           var emoji = message[4];
            

            await ReplyAsync($"Role {roleName} was created successfully.");
            Console.WriteLine($"User {user.Username} with user ID {user.Id} created role {roleName}!");
        }

        [Command("weeb")]
        public async Task AnimeQuote()
        {
            var user = Context.User as SocketGuildUser;
            Console.WriteLine($"User \"{user}\" requested an anime quote!");

            var animeQuote = await _animeApi.GetRandomAnimeQuote();

            await ReplyAsync($"\"{animeQuote.Quote}\"\n-{animeQuote.Character}, from {animeQuote.Anime}");
        }

        [Command("testDBconnection")]
        public async Task Test() 
        {
            var user = Context.User;
            if (user.Id != _gameMasterId)
            {
                await ReplyAsync("You don't have permission to run this command as of now.");
            }
            var moreData = _dataAccess.GetData();
            var data = _dataAccess.GetConnectionString();
            if (string.IsNullOrWhiteSpace(data))
            {
                await ReplyAsync($"Connection String was not found.");
                return;
            }
            await ReplyAsync($"ConnectionStringFound: {data}");
        }
    }
}

