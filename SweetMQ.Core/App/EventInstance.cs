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

        public Type EventType { get; }

        public EventInstance(EventConfig eventConfig, ConnectionFactory connectionFactory)
        {
            EventType = typeof(T);
            _channel = connectionFactory.Connection.CreateModel();

            ExchangeDeclare(eventConfig.Exchange);
            _exchange = eventConfig.Exchange.Name;

            if (eventConfig.Queues != null)
                foreach (var queue in eventConfig.Queues)
                {
                    QueueDeclare(queue);
                    _channel.QueueBind(queue.Name, "", queue.Name);
                }
            else
                foreach (var route in eventConfig.Routing)
                foreach (var queue in route.Queues)
                {
                    QueueDeclare(queue);
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

        private void QueueDeclare(QueueInfo queue)
        {
            _channel.QueueDeclare(
                string.IsNullOrWhiteSpace(queue.Name)
                    ? ""
                    : queue.Name,
                queue.Durable,
                queue.Exclusive,
                queue.AutoDelete,
                queue.Arguments
            );
        }

        private void ExchangeDeclare(ExchangeInfo exchange)
        {
            _channel.ExchangeDeclare(
                string.IsNullOrWhiteSpace(exchange.Name)
                    ? throw new ArgumentNullException(nameof(exchange.Name))
                    : exchange.Name,
                exchange.Type.ToString().ToLower(),
                exchange.Durable,
                exchange.AutoDelete,
                exchange.Arguments
            );
        }
    }
}