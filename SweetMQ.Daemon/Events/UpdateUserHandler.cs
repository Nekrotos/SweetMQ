using System;
using System.Threading.Tasks;
using SweetMQ.Core.Interfaces;

namespace SweetMQ.Daemon.Events
{
    public class UpdateUserHandler : IEventHandler<UpdateUser>
    {
        public Task Execute(UpdateUser message)
        {
            throw new NotImplementedException();
        }
    }
}