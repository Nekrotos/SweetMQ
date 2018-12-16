using Microsoft.Extensions.DependencyInjection;
using SweetMQ.Core;
using SweetMQ.Core.App;
using SweetMQ.Daemon.Events;

namespace SweetMQ.Daemon
{
    public static class Extensions
    {
        public static IServiceCollection AddServiceBus(this IServiceCollection services)
        {
            var connectionFactory = new ConnectionFactory();

            services
                .AddEventHandler<UpdateUserHandler>(connectionFactory);
            
            return services;
        }
    }
}