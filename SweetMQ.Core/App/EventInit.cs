using RabbitMQ.Client;
using System;
using SweetMQ.Core.Domain;

namespace SweetMQ.Core
{
    public class EventInit
    {
        private readonly IModel _channel;

        public EventInit(EventConfig eventConfig)
        {
            _channel = new ConnectionFactory().Connection.CreateModel();

            ExchangeDeclare(eventConfig.Exchange);

            if (eventConfig.Queues != null)
                foreach (var queue in eventConfig.Queues)
                {
                    QueueDeclare(queue);
                    _channel.QueueBind(queue.Name, "", queue.Name);
                }
            else
                foreach (var route in eventConfig.Routing)
                {
                    foreach (var queue in route.Queues)
                    {
                        QueueDeclare(queue);
                        _channel.QueueBind(queue.Name, eventConfig.Exchange.Name, route.Route);
                    }
                }
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
