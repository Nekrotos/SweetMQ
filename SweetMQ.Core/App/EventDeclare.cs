using System;
using RabbitMQ.Client;
using SweetMQ.Core.Domain;

namespace SweetMQ.Core.App
{
    internal class EventDeclare
    {
        internal static void QueueDeclare(ref IModel channel, QueueInfo queue)
        {
            channel.QueueDeclare(
                string.IsNullOrWhiteSpace(queue.Name)
                    ? ""
                    : queue.Name,
                queue.Durable,
                queue.Exclusive,
                queue.AutoDelete,
                queue.Arguments
            );
        }

        internal static void ExchangeDeclare(ref IModel channel, ExchangeInfo exchange)
        {
            channel.ExchangeDeclare(
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