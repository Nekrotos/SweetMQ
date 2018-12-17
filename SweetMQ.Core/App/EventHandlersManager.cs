using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SweetMQ.Core.Domain;
using SweetMQ.Core.Interfaces;

namespace SweetMQ.Core.App
{
    public static class EventHandlersManager
    {
        public static IServiceCollection AddEventHandler<TEventHandler>(
            this IServiceCollection services,
            ConnectionFactory connectionFactory,
            QueueInfo queueInfo
        ) where TEventHandler : class // TODO, IEventHandler<T> where T : class, IEventBase
        {
            var type = typeof(TEventHandler)
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                .SelectMany(t => t.GetGenericArguments())
                .ToArray()[0];

            if (type == null)
                throw new ArgumentException(
                    $"{nameof(TEventHandler)} doesn't implements IEventHandler<IEventBase> interface.");

            var chanel = connectionFactory.Connection.CreateModel();
            EventDeclare.QueueDeclare(ref chanel, queueInfo);

            services.AddScoped<TEventHandler>();
            var consumer = new EventingBasicConsumer(chanel);

            consumer.Received += async (model, args) =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var handler = serviceProvider.GetService<TEventHandler>();

                var message = Encoding.UTF8.GetString(args.Body);
                var receiveModel = JsonConvert.DeserializeObject(message, type);

                var methodInfo = handler.GetType().GetMethod("ExecuteAsync");

                await (Task) methodInfo.Invoke(handler, new[] {receiveModel});

                chanel.BasicAck(args.DeliveryTag, false);
            };

            chanel.BasicConsume(queueInfo.Name, false, consumer);
            return services;
        }
    }
}