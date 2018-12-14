using System;
using System.Collections.Generic;
using System.Text;
using SweetMQ.Core.Interfaces;

namespace SweetMQ.Core.App
{
    public sealed class EventsStorage
    {
        private static readonly IDictionary<string, EventBase> Instances = new Dictionary<string, EventBase>();


    }
}
