using Switchboard.Common.Configuration;
using Switchboard.Server.Messaging;

namespace Switchboard.Server.RetryManager
{
    public interface IRetryManager
    {
        void Initialize(MessageBusInfo messageBusInfo);

        void RetryMessage(IMessage message);
    }
}
