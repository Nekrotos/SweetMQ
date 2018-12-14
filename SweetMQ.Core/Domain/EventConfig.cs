using System;
using System.Collections.Generic;
using System.Linq;

namespace SweetMQ.Core.Domain
{
    public class EventConfig
    {
        public EventConfig(
            ExchangeInfo exchange,
            IReadOnlyCollection<RouteKey> routing
        )
        {
            Exchange = exchange;
            Routing = routing == null || !routing.Any()
                ? routing
                : throw new ArgumentNullException(nameof(routing));
        }

        public EventConfig(
            ExchangeInfo exchange,
            IReadOnlyCollection<QueueInfo> queues
        )
        {
            Exchange = exchange;
            Queues = queues == null || !queues.Any()
                ? queues
                : throw new ArgumentNullException(nameof(queues));
        }

        public ExchangeInfo Exchange { get; }
        public IEnumerable<RouteKey> Routing { get; }
        public IEnumerable<QueueInfo> Queues { get; }
    }
}
