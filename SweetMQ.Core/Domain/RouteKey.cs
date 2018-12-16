using System.Collections.Generic;

namespace SweetMQ.Core.Domain
{
    public sealed class RouteKey
    {
        public RouteKey(
            string route,
            IReadOnlyCollection<QueueInfo> queues
        )
        {
            Route = route;
            Queues = queues;
        }

        public string Route { get; }
        public IReadOnlyCollection<QueueInfo> Queues { get; }
    }
}