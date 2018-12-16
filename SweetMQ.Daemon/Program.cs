using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SweetMQ.Daemon
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var hostBuilder = new Startup(args).HostBuilder;
            await hostBuilder.RunConsoleAsync();
        }
    }
}