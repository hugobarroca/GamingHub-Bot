using ApiCalls;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using GamingHubBot.Application.Configuration;
using GamingHubBot.Data;
using GamingHubBot.Infrastructure.Gateways;
using GamingHubBot.Infrastructure.Repositories.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
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

            using var services = ConfigureServices(config);

            var client = services.GetRequiredService<DiscordSocketClient>();
            var commands = services.GetRequiredService<InteractionService>();

            client.Log += LogAsync;
            commands.Log += LogAsync;

            client.Ready += async () =>
            {
                if (IsDebug())
                    await commands.RegisterCommandsToGuildAsync(config.GetValue<ulong>("testGuild"), true);
                else
                    await commands.RegisterCommandsGloballyAsync(true);
            };

            await services.GetRequiredService<CommandHandler>().InitializeAsync();

            await client.LoginAsync(TokenType.Bot, config["token"]);
            await client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }


        static ServiceProvider ConfigureServices(IConfiguration configuration)
        {
            return new ServiceCollection()
                .AddSingleton(configuration)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<CommandHandler>()
                .AddSingleton<ICommandHandler, CommandHandler>()
                .AddSingleton<InteractionService>()
                .AddSingleton<IDataAccess, SqlDataAccess>()
                .AddSingleton<IAnimeApi, AnimeApi>()
                .AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Configure<GeneralSettings>(configuration)
                .BuildServiceProvider();
        }

        static Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }


        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
        }


        static bool IsDebug()
        {
            return true;
        }

    }
}
