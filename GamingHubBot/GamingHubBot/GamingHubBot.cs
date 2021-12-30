using Discord;
using Discord.WebSocket;
using GamingHubBot.Application.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private IOptions<GeneralSettings> _options;

        public GamingHubBot(ILogger<GamingHubBot> logger, DiscordSocketClient client, IServiceProvider services, ICommandHandler commandHandler, IOptions<GeneralSettings> options)
        {
            _logger = logger;
            _commandHandler = commandHandler;
            _client = client;
            _options = options;
        }


        public async void Start()
        {
            await _commandHandler.InstallCommandsAsync();

            _client.Log += Log;

            await Log(new LogMessage(LogSeverity.Info, "", $"Looking for token at {Path.GetFullPath("token.txt")}"));

            var token = _options.Value.Token;

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
            if (msg.Severity == LogSeverity.Debug) 
                _logger.LogDebug(msg.Message);

            return Task.CompletedTask;
        }
    }
}
