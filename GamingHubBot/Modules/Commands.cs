using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace TutorialBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong motherfucker");
        }

        [Command("help")]
        public async Task Help()
        {
            string message = "";
            message += "**!help**: Displays this informative message.\n";
            message += "**!ping**: Respondes with a pong to confirm online status.\n";
            await ReplyAsync(message);
        }
    }
}