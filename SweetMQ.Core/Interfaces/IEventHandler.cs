using System.Threading.Tasks;

namespace SweetMQ.Core.Interfaces
{
    public interface IEventHandler<in TEvent> where TEvent : IEventBase
    {
        Task ExecuteAsync(TEvent message);
    }
}