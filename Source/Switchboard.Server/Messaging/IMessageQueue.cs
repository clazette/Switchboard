using System;
using System.Messaging;

namespace Switchboard.Server.Messaging
{
    public interface IMessageQueue
    {
        void Send(object obj);

        event ReceiveCompletedEventHandler ReceiveCompleted;
    }
}
