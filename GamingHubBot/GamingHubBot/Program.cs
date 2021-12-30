using ApiCalls;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GamingHubBot.Application.Configuration;
using GamingHubBot.Data;
using GamingHubBot.Infrastructure.Gateways;
using GamingHubBot.Infrastructure.Repositories.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GamingHubBot
{
    class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            var config = builder.Build();

            var socketConfig = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.GuildMessageReactions | GatewayIntents.GuildMessages,
            };

            var host = Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IGamingHubBot, GamingHubBot>()
                .AddSingleton<ICommandHandler, CommandHandler>()
                .AddSingleton<DiscordSocketClient>(x => new DiscordSocketClient(socketConfig))
                .AddSingleton<CommandService>()
                .AddSingleton<IDataAccess, SqlDataAccess>()
                .AddSingleton<IAnimeApi, AnimeApi>()
                .Configure<GeneralSettings>(config)
                .BuildServiceProvider();
            })
            .Build();

            var bot = ActivatorUtilities.CreateInstance<GamingHubBot>(host.Services);
            bot.Start();
            await Task.Delay(-1);
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
        }

    }
}
