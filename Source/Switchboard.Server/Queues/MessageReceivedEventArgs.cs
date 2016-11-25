using System;
using Switchboard.Server.Messaging;

namespace Switchboard.Server.Queues
{
    public class MessageReceivedEventArgs : EventArgs
    {
        private IMessage _message;

        public IMessage Message
        {
            get { return this._message; }
        }

        public MessageReceivedEventArgs(IMessage message)
        {
            this._message = message;
        }
    }
}
