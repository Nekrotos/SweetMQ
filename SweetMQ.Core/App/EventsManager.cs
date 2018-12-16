using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SweetMQ.Core.Domain;
using SweetMQ.Core.Interfaces;

namespace SweetMQ.Core.App
{
    public sealed class EventsManager
    {
        private static readonly IDictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static void AddEvent<T>(EventConfig eventConfig, ConnectionFactory connectionFactory)
            where T : class, IEventBase
        {
            var eventInstance = new EventInstance<T>(eventConfig, new ConnectionFactory());
            var result = Instances.TryAdd(typeof(T), eventInstance);
            if (result == false)
                throw new Exception(nameof(AddEvent));
        }

        public static async Task SendEvent<T>(T message, string routingKey) where T : class, IEventBase
        {
            if (!Instances.Keys.Contains(typeof(T)))
                throw new ArgumentException($"The {typeof(T).Name} is already registered");

            var @event = (EventInstance<T>) Instances.SingleOrDefault(pair => pair.Key == typeof(T)).Value;
            await @event.SendAsync(message, routingKey);
        }
    }
}