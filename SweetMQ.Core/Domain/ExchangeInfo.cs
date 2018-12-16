using System;
using System.Collections.Generic;
using SweetMQ.Core.Enums;

namespace SweetMQ.Core.Domain
{
    public sealed class ExchangeInfo
    {
        public ExchangeInfo(
            string name,
            ExchangeType type,
            bool durable = true,
            bool autoDelete = false,
            IDictionary<string, object> arguments = null
        )
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentNullException(nameof(name))
                : name;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = arguments;
        }

        public string Name { get; }
        public ExchangeType Type { get; }
        public bool Durable { get; }
        public bool AutoDelete { get; }
        public IDictionary<string, object> Arguments { get; }
    }
}
