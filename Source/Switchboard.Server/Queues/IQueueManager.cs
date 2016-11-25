using System;
using Switchboard.Common.Configuration;

namespace Switchboard.Server.Queues
{
    public interface IQueueManager
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        void Initialize();

        void StartListening();
    }
}
