using System;
using System.Messaging;

namespace Switchboard.Common.Events
{
    public class MessageReceivedEventArgs : EventArgs
    {
        private Message _message;

        public Message Message
        {
            get { return this._message; }
        }

        public MessageReceivedEventArgs(Message message)
        {
            this._message = message;
        }
    }
}
