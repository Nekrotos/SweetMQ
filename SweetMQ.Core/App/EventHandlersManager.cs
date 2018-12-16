using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SweetMQ.Core.Domain;

namespace SweetMQ.Core.App
{
    public static class EventHandlersManager
    {

        private static readonly IDictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static IServiceCollection AddEventHandler<TEventHandler>(
            this IServiceCollection services,
            ConnectionFactory connectionFactory
        ) where TEventHandler : class // TODO , IEventHandler<IEventBase>
        {
            var eventInstance = new EventHandlerInstance<TEventHandler>(new ConnectionFactory());
            var result = Instances.TryAdd(typeof(TEventHandler), eventInstance);
            if (result == false)
                throw new Exception(nameof(AddEventHandler));

            return services;
        }
    }
}