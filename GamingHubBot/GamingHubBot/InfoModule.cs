using System;
using System.Collections.Generic;
using System.Text;

namespace GamingHubBot
{
    using Discord.Commands;
    using System.Threading.Tasks;

    public class InfoModule : ModuleBase<SocketCommandContext>
    {
		// ~say hello world -> hello world
		[Command("say")]
		[Summary("Echoes a message.")]
		public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
			=> ReplyAsync(echo);
	}
}
