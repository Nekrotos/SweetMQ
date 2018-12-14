using SweetMQ.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace SweetMQ.Daemon
{
    public class UpdateUserHandler : IEventHandler<UpdateUser>
    {
        public Task Execute(UpdateUser message)
        {
            throw new NotImplementedException();
        }
    }
}
