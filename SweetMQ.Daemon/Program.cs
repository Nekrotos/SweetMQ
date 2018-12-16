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
                })
            };
            var eventConfig = new EventConfig(exchangeInfo, routing);

            var eventInstance = new EventInstance<UpdateUser>(eventConfig, new ConnectionFactory());
            var updateUser = new UpdateUser(Guid.NewGuid(), "new user name");

            await eventInstance.SendAsync(updateUser, "dw");

            Console.WriteLine("Hello World!");
        }
    }
}