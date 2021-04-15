using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingHubBot.Logging
{
    public class LoggingService
    {
        string _path;
        public LoggingService(DiscordSocketClient client, CommandService command)
        {
            client.Log += LogAsync;
            command.Log += LogAsync;

            _path = @"C:\\Users\\hugob\\Documents\\log.txt";
        }
        private Task LogAsync(LogMessage message)
        {
            string lines;
            if (message.Exception is CommandException cmdException)
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                lines = $"[Command/{message.Severity}] {cmdException.Command.Aliases.First()} failed to execute in {cmdException.Context.Channel}.";
                lines += cmdException;
            }
            else
            {
                lines = $"[General/{message.Severity}] {message}";
            }
            //Console.WriteLine(lines);
            return Task.CompletedTask;
        }

        public void writeFile(string lines)
        {
            using (StreamWriter sw = File.AppendText(_path))
            {
                sw.WriteLine(lines);
            }
        }
    }
}
