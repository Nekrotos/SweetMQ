using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SweetMQ.Core;
using SweetMQ.Core.App;
using SweetMQ.Core.Domain;
using SweetMQ.Core.Enums;

namespace SweetMQ.Daemon
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var exchangeInfo = new ExchangeInfo("exchange name", ExchangeType.Direct);
            IReadOnlyCollection<RouteKey> routing = new List<RouteKey>
            {
                new RouteKey("all admin user moderator", new List<QueueInfo>
                {
                    new QueueInfo("queue1"),
                    new QueueInfo("queue2")
                }),
                new RouteKey("dante nooby", new List<QueueInfo>
                {
                    new QueueInfo("queue3"),
                    new QueueInfo("queue4")
                })
            };
            var eventConfig = new EventConfig(exchangeInfo, routing);

            EventsManagers.AddEvent<UpdateUser>(eventConfig, new ConnectionFactory());
            
            var updateUser = new UpdateUser(Guid.NewGuid(), "new user name");

            await EventsManagers.SendEvent(updateUser, "all admin user moderator");

            Console.WriteLine("Hello World!");
        }
    }
}