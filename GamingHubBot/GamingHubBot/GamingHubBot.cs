using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GamingHubBot
{
    public class GamingHubBot : IGamingHubBot
    {
        private ICommandHandler _commandHandler;
        private DiscordSocketClient _client;
        private ILogger<GamingHubBot> _logger;

        public GamingHubBot(ILogger<GamingHubBot> logger, DiscordSocketClient client, IServiceProvider services, ICommandHandler commandHandler)
        {
            _logger = logger;
            _commandHandler = commandHandler;
            _client = client;
        }


        public async void Start()
        {
            await _commandHandler.InstallCommandsAsync();

            _client.Log += Log;

            await Log(new LogMessage(LogSeverity.Info, "", $"Looking for token at {Path.GetFullPath("token.txt")}"));

            if (!File.Exists("token.txt"))
            {
                await Log(new LogMessage(LogSeverity.Critical, "", "Token not found, quitting program."));
                System.Environment.Exit(1);
            }

            var token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            if (msg.Severity == LogSeverity.Critical || msg.Severity == LogSeverity.Error) 
                _logger.LogError(msg.Message);
            if (msg.Severity == LogSeverity.Warning) 
                _logger.LogWarning(msg.Message);
            if (msg.Severity != LogSeverity.Info)
                _logger.LogInformation(msg.Message);

            return Task.CompletedTask;
        }
    }
}
