using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Runtime.DotNetCore;

namespace GamingHubBot
{
    class Program
    {

        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var exitCode = HostFactory.Run(x =>
            {
                if (
                  RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                  RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    )
                {
                    x.UseEnvironmentBuilder(
                      target => new DotNetCoreEnvironmentBuilder(target)
                    );
                }
                x.Service<GamingHubBot>(s =>
                {
                    s.ConstructUsing(gaminghubbot => new GamingHubBot());
                    s.WhenStarted(gaminghubbot => gaminghubbot.Start());
                    s.WhenStopped(gaminghubbot => gaminghubbot.Stop());
                });

                x.RunAsNetworkService();

                x.SetServiceName("GamingHubBot");
                x.SetDisplayName("Gaming Hub Bot");
                x.SetDescription("This is the gaming hub bot service, which services all discord requests.");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }

    }
}
