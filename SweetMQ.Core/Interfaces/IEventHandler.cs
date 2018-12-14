using System.Threading.Tasks;

namespace SweetMQ.Core.Interfaces
{
    public interface IEventHandler<in TEvent> where TEvent : EventBase
    {
        Task Execute(TEvent message);
    }
}
