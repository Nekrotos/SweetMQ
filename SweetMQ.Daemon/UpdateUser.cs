using System;
using System.Collections.Generic;
using SweetMQ.Core.Domain;
using SweetMQ.Core.Enums;
using SweetMQ.Core.Interfaces;

namespace SweetMQ.Daemon
{
    public sealed class UpdateUser : EventBase
    {
        private static readonly ExchangeInfo ExchangeInfo = new ExchangeInfo("dsadsa", ExchangeType.Direct);

        private static readonly IReadOnlyCollection<RouteKey> Routing = new List<RouteKey>
        {
            new RouteKey("dsadas", new List<QueueInfo>
            {
                new QueueInfo("dddas"),
                new QueueInfo("33243243")
            })
        };

        private static readonly EventConfig EventConfig = new EventConfig(ExchangeInfo, Routing);

        public UpdateUser(
            Guid userId,
            string userName
        ) : base(EventConfig)
        {
            UserId = userId;
            UserName = userName;
        }

        public Guid UserId { get; }
        public string UserName { get; }
    }
}