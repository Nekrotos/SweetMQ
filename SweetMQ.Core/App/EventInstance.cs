using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SweetMQ.Core.Domain;
using SweetMQ.Core.Interfaces;

namespace SweetMQ.Core.App
{
    public class EventInstance<T> where T : class, IEventBase
    {
        private readonly IModel _channel;
        private readonly string _exchange;

        public EventInstance(EventConfig eventConfig, ConnectionFactory connectionFactory)
        {
            _channel = connectionFactory.Connection.CreateModel();

            EventDeclare.ExchangeDeclare(ref _channel, eventConfig.Exchange);
            _exchange = eventConfig.Exchange.Name;

            if (eventConfig.Queues != null)
                foreach (var queue in eventConfig.Queues)
                {
                    EventDeclare.QueueDeclare(ref _channel, queue);
                    _channel.QueueBind(queue.Name, "", queue.Name);
                }
            else
                foreach (var route in eventConfig.Routing)
                foreach (var queue in route.Queues)
                {
                    EventDeclare.QueueDeclare(ref _channel, queue);
                    _channel.QueueBind(queue.Name, eventConfig.Exchange.Name, route.Route);
                }
        }

        public async Task SendAsync(T message, string routingKey, IBasicProperties basicProperties = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrWhiteSpace(routingKey)) routingKey = "";

            await Task.Run(() =>
            {
                var body = JsonConvert.SerializeObject(message);

                if (basicProperties == null)
                    basicProperties = _channel.CreateBasicProperties();
                basicProperties.Persistent = true;

                _channel.BasicPublish(_exchange, routingKey, basicProperties, Encoding.UTF8.GetBytes(body));
            });
        }
    }
}