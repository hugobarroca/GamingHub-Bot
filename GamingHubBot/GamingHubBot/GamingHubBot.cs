﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GamingHubBot
{
    public class GamingHubBot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;


        public async void Start() 
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();


            var commandHandler = new CommandHandler(_client, _commands, _services);

            await commandHandler.InstallCommandsAsync();

            _client.Log += Log;

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
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    

        public void Stop()
        {

        }




}
}
