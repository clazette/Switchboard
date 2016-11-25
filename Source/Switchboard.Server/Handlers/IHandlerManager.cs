using System;
using System.Messaging;
using Switchboard.Common.Logger;

namespace Switchboard.Server.Handlers
{
    public interface IHandlerManager
    {
        event EventHandler<MessageErrorOccurredEventArgs> MessageErrorOccurred;

        bool TryInitializion(ILogger logger);

        void ProcessMessage(Message message);
    }
}
