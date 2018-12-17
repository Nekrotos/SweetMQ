using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SweetMQ.Core;
using SweetMQ.Core.App;
using SweetMQ.Core.Domain;
using SweetMQ.Core.Enums;
using SweetMQ.Daemon.Events;

namespace SweetMQ.Daemon
{
    public class TestService : IHostedService, IDisposable
    {
        private readonly ILogger<TestService> _logger;

        public TestService(ILogger<TestService> logger)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing daemon....");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started daemon.");

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
            EventsManager.AddEvent<UpdateUser>(eventConfig, new ConnectionFactory());
            var updateUser = new UpdateUser(Guid.NewGuid(), "new user name");
            await EventsManager.SendEvent(updateUser, "all admin user moderator");
            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping daemon.");
            return Task.CompletedTask;
        }
    }
}