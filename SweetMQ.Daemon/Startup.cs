using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SweetMQ.Daemon
{
    internal class Startup
    {
        internal Startup(string[] args)
        {
            var hostBuilder = new HostBuilder();
            hostBuilder
                .ConfigureAppConfiguration((context, builder) => Configure(context, builder, args))
                .ConfigureServices(ConfigureServices)
                .ConfigureLogging(ConfigureLogging);

            HostBuilder = hostBuilder;
        }

        public HostBuilder HostBuilder { get; }

        private static void Configure(HostBuilderContext builderContext, IConfigurationBuilder configHost,
            string[] args)
        {
            configHost.SetBasePath(Directory.GetCurrentDirectory());
            configHost.AddEnvironmentVariables("ASPNETCORE_");
            configHost.AddJsonFile("appsettings.json", true);
            configHost.AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", true);
            configHost.AddCommandLine(args);
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services
                .AddLogging()
                .AddOptions()
                .AddSingleton<IHostedService, TestService>()
                .AddServiceBus();
        }

        private static void ConfigureLogging(HostBuilderContext hostingContext, ILoggingBuilder logging)
        {
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            logging.AddConsole();
        }
    }
}