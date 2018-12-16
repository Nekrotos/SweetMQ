using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SweetMQ.Core.Interfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetMQ.Core.App
{
    public static class EventHandlersManager
    {

        //private static readonly IDictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static IServiceCollection AddEventHandler<TEventHandler>(
            this IServiceCollection services,
            ConnectionFactory connectionFactory,
            string queueName
        ) where TEventHandler : class // TODO , IEventHandler<IEventBase>
        {
            var chanel = connectionFactory.Connection.CreateModel();

            services.AddScoped<TEventHandler>();
            var consumer = new EventingBasicConsumer(chanel);
            consumer.Received += async (model, args) =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var handler = serviceProvider.GetService<TEventHandler>();

                var type = typeof(TEventHandler).GetInterfaces()
                    .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    .SelectMany(t => t.GetGenericArguments())
                    .ToArray()[0];

                var message = Encoding.UTF8.GetString(args.Body);
                var receiveModel = JsonConvert.DeserializeObject(message, type);

                var methodInfo = handler.GetType().GetMethod("ExecuteAsync");

                await (Task)methodInfo.Invoke(handler, new[] { receiveModel });

                chanel.BasicAck(args.DeliveryTag, false);

                //var result = Instances.TryAdd(typeof(TEventHandler), eventInstance);
                //if (result == false)
                //    throw new Exception(nameof(AddEventHandler));
            };

            chanel.QueueDeclare(queueName, true, false, false);
            chanel.BasicConsume(queueName, false, consumer);

            return services;

        }
    }
}
