using System;
using SweetMQ.Core.Interfaces;

namespace SweetMQ.Daemon.Events
{
    public sealed class UpdateUser : IEventBase
    {
        public UpdateUser(Guid userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }

        public Guid UserId { get; }
        public string UserName { get; }
    }
}