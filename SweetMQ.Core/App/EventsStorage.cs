using SweetMQ.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SweetMQ.Core.App
{
    public sealed class EventsStorage
    {
        private static readonly IDictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static void AddEvent<T>(EventInstance<T> eventInstance) where T : class, IEventBase
        {
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
