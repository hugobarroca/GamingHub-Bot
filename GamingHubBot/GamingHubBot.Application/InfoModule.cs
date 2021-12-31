namespace GamingHubBot
{
    using Discord.Commands;
    using Discord.Interactions;
    using Discord.WebSocket;
    using GamingHubBot.Infrastructure.Gateways;
    using global::GamingHubBot.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class InfoModule : InteractionModuleBase<SocketInteractionContext>
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
        public Task SayAsync(string echo)
            => ReplyAsync(echo);

        [SlashCommand("echo", "Echo an input")]
        public async Task Echo(string input)
        {
            await RespondAsync(input);
        }

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

        [SlashCommand("ping", "Sends a ping to the bot to verify connection status.")]
        public async Task Ping()
        {
            var user = Context.User as SocketGuildUser;

            if (user == null)
                return;

            Console.WriteLine($"User \"{user.Username}\" requested a ping.");
            await RespondAsync("Bot is currently active!");
        }

        [SlashCommand("addrole", "Adds the request role to your roles.")]
        public async Task AddRole(string roleName)
        {
            var user = Context.User as SocketGuildUser;

            if (user == null)
                return;

            var roles = Context.Guild.Roles;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == roleName.ToLower());

            if (role == null)
            {
                Console.WriteLine($"User \"{user.Username}\" tried to add itself to the role of \"{roleName}\", but such role could not be found.");
                await RespondAsync("I couldn't find that role. Check for typing errors you doofus!");
                return;
            }

            if (user.Roles.Contains(role))
            {
                Console.WriteLine($"User \"{user.Username}\" tried to add itself to the role of \"{role}\", but he already belonged to that role.");
                await RespondAsync($"You already have the role of {role}!");
            }
            else
            {
                if (permittedRoles.Contains(role.ToString()))
                {
                    Console.WriteLine($"User \"{user.Username}\" added itself to the role of \"{role}\".");
                    await user.AddRoleAsync(role);
                    await RespondAsync($"You have been given the role of {role}!");
                }
                else
                {
                    if (role.ToString() == "Game Master")
                    {
                        Console.WriteLine($"User \"{user.Username}\" tried to add itself to the role of \"{role}\", which is not a permitted role.\n");
                        await RespondAsync("Sneaky bastard aintcha...");
                    }
                    else
                    {
                        Console.WriteLine($"User \"{user.Username}\" tried to add itself to the role of \"{role}\", which is not a permitted role.\n");
                        await RespondAsync("The role you tried to add is not a part of the permitted roles list!");
                    }
                }
            }
        }

        [SlashCommand("removerole", "Adds the request role to your roles.")]
        public async Task RemoveRole(string roleName)
        {
            var user = Context.User as SocketGuildUser;
            var roles = Context.Guild.Roles;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == roleName.ToLower());

            if (role == null)
            {
                Console.WriteLine($"User \"{user.Username}\" tried to remove itself from the role of \"{role}\", but such role could not be found.");
                await RespondAsync("Could not find that role. Check for typing errors you doofus!");
                return;
            }

            if (!user.Roles.Contains(role))
            {
                Console.WriteLine($"User \"{user.Username}\" tried to remove itself from the role of \"{role}\", but he did not belong to that role.");
                await RespondAsync($"You do not have the role of {role}!");
            }
            else
            {
                if (permittedRoles.Contains(role.ToString()))
                {
                    Console.WriteLine($"User \"{user.Username}\" removed itself from the role of \"{role}\".");
                    await user.RemoveRoleAsync(role);
                    await RespondAsync($"You have been removed from the role of {role}!");
                }
                else
                {
                    Console.WriteLine($"User \"{user.Username}\" tried to remove itself from the role of \"{role}\", but that role is not in the permitted roles list.");
                    await RespondAsync("Why would you try and do that? o.O");
                }
            }
        }

        [SlashCommand("permittedroles", "Lists all currently permitted roles.")]
        public async Task PermittedRoles()
        {
            var user = Context.User as SocketGuildUser;
            if (user == null)
                return;

            Console.WriteLine($"User \"{user.Username}\" requested information on permitted roles.");

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
            await RespondAsync($"**Permitted roles**: {roles}");
        }

        [SlashCommand("createrole", "Creates a new role.")]
        public async Task CreateRole(string roleName, int red, int blue, int green)
        {
            var user = Context.User;
            if (user.Id != _gameMasterId)
            {
                await RespondAsync("You don't have permission to run this command as of now.");
            }
            return;
        }

        [SlashCommand("weeb", "Returns a random anime quote.")]
        public async Task AnimeQuote()
        {
            var user = Context.User as SocketGuildUser;
            Console.WriteLine($"User \"{user}\" requested an anime quote!");

            var animeQuote = await _animeApi.GetRandomAnimeQuote();

            await RespondAsync($"\"{animeQuote.Quote}\"\n-{animeQuote.Character}, from {animeQuote.Anime}");
        }

        [SlashCommand("colors", "Returns all available role colors.")]
        public async Task AvailableColors()
        {
            var colors = await _dataAccess.GetColors();

            if (colors == null)
            {
                await RespondAsync("Sorry, I couldn't get the color list at this time. :(");
                return;
            }

            string response = "Available colors are: \n";
            foreach (var color in colors)
            {
                response += color.Name + "\n";
            }
            await RespondAsync(response);
        }
    }
}

