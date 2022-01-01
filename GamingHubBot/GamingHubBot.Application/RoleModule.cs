using Discord.Interactions;
using Discord.WebSocket;
using GamingHubBot.Application.Entities;
using GamingHubBot.Infrastructure.Gateways;
using global::GamingHubBot.Data;
using Microsoft.Extensions.Logging;

namespace GamingHubBot.Application
{
    public class RoleModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IDataAccess _dataAccess;
        private readonly IAnimeApi _animeApi;
        private readonly ILogger<InfoModule> _logger;

        public RoleModule(ILogger<InfoModule> logger, IDataAccess dataAccess, IAnimeApi animeApi)
        {
            _logger = logger;
            _dataAccess = dataAccess;
            _animeApi = animeApi;
        }

        private readonly ulong _gameMasterId = 207178008706940928;
        List<string> permittedRoles = new List<string>() { "Bishop", "Freeloaders", "Ghost", "Hunter", "Impostor", "Pirate", "Summoner", "S.W.A.T.", "Tiefling", "Waifu" };


        [SlashCommand("addrole", "Adds the requested role to your roles.")]
        public async Task AddRole(SocketRole role)
        {
            var user = Context.User as SocketGuildUser;

            if (user.Roles.Contains(role))
            {
                _logger.LogInformation($"User \"{user.Username}\" tried to add itself to the role of \"{role.Name}\", but he already belonged to that role.");
                await RespondAsync($"You already have the role of {role.Name}!", ephemeral: true);
            }
            else
            {
                if (permittedRoles.Contains(role.Name))
                {
                    _logger.LogInformation($"User \"{user.Username}\" added itself to the role of \"{role.Name}\".");
                    await user.AddRoleAsync(role);
                    await RespondAsync($"You have been given the role of {role.Name}!", ephemeral: true);
                }
                else
                {
                    if (role.Name == "Game Master")
                    {
                        _logger.LogInformation($"User \"{user.Username}\" tried to add itself to the role of \"{role.Name}\", which is not a permitted role.\n");
                        await RespondAsync("Sneaky bastard aintcha...");
                    }
                    else
                    {
                        _logger.LogInformation($"User \"{user.Username}\" tried to add itself to the role of \"{role.Name}\", which is not a permitted role.\n");
                        await RespondAsync("The role you tried to add is not a part of the permitted roles list!", ephemeral: true);
                    }
                }
            }
        }

        [SlashCommand("removerole", "Adds the request role to your roles.")]
        public async Task RemoveRole(SocketRole role)
        {
            var user = Context.User as SocketGuildUser;
            var roles = Context.Guild.Roles;

            if (!user.Roles.Contains(role))
            {
                _logger.LogInformation($"User \"{user.Username}\" tried to remove itself from the role of \"{role}\", but he did not belong to that role.");
                await RespondAsync($"You do not have the role of {role.Name}!", ephemeral: true);
            }
            else
            {
                if (permittedRoles.Contains(role.Name))
                {
                    _logger.LogInformation($"User \"{user.Username}\" removed itself from the role of \"{role.Name}\".");
                    await user.RemoveRoleAsync(role);
                    await RespondAsync($"You have been removed from the role of {role.Name}!", ephemeral: true);
                }
                else
                {
                    _logger.LogInformation($"User \"{user.Username}\" tried to remove itself from the role of \"{role}\", but that role is not in the permitted roles list.");
                    await RespondAsync("Why would you try and do that? o.O", ephemeral: true);
                }
            }
        }

        [SlashCommand("permittedroles", "Lists all currently permitted roles.")]
        public async Task PermittedRoles()
        {
            var user = Context.User as SocketGuildUser;
            if (user == null)
                return;

            _logger.LogInformation($"User \"{user.Username}\" requested information on permitted roles.");

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
            await RespondAsync($"**Permitted roles**: {roles}", ephemeral: true);
        }

        [SlashCommand("createrole", "Creates a new role.")]
        public async Task CreateRole(string roleName, int red, int blue, int green)
        {
            var user = Context.User;
            if (user.Id != _gameMasterId)
            {
                await RespondAsync("You don't have permission to run this command as of now.", ephemeral: true);
            }
            return;
        }

        [SlashCommand("synchronizeroles", "Synchronizes current server roles with the database.")]
        public async Task SynchronizeRoles() 
        {
            var dbRoles = new List<Role>();
            var roles = Context.Guild.Roles;

            foreach (var role in roles) 
            {
                dbRoles.Add(new Role { Id = role.Id, Name = role.Name, Permitted = false, ColorId = null});
            }

            await DeferAsync();
            await _dataAccess.SynchronizeRolesAsync(dbRoles);
            
            await FollowupAsync("Roles were synchronized successfully!");
        }

        [SlashCommand("colors", "Returns all available role colors.")]
        public async Task AvailableColors()
        {
            var colors = await _dataAccess.GetColorsAsync();

            if (colors == null)
            {
                await RespondAsync("Sorry, I couldn't get the color list at this time. :(", ephemeral: true);
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
