using System.Collections.Generic;

namespace SweetMQ.Core.Domain
{
    public sealed class QueueInfo
    {
        public QueueInfo(
            string name = "",
            bool durable = true,
            bool exclusive = false,
            bool autoDelete = false,
            IDictionary<string, object> arguments = null
        )
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? ""
                : name;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            Arguments = arguments;
        }

        public string Name { get; }
        public bool Durable { get; }
        public bool Exclusive { get; }
        public bool AutoDelete { get; }
        public IDictionary<string, object> Arguments { get; }
    }
}