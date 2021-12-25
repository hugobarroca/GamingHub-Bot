using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace GamingHubBot
{
    public interface ICommandHandler
    {
        Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> cachedMessage, Cacheable<IMessageChannel, ulong> originChannel, SocketReaction reaction);
        Task HandleReactionRemovedAsync(Cacheable<IUserMessage, ulong> cachedMessage, Cacheable<IMessageChannel, ulong> originChannel, SocketReaction reaction);
        Task InstallCommandsAsync();
    }
}