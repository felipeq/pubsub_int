using System.Threading.Tasks;

namespace EventAggregator
{
    public interface ISubscriber
    {
        Task<bool> ReceiveMsg(Message msg);
    }
}
