using System;
using Switchboard.Server.Messaging;

namespace Switchboard.Server.Handlers
{
    [Serializable]
    public abstract class MessageEventArgsBase : EventArgs
    {
        private IMessage _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventArgsBase"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageEventArgsBase(IMessage message)
        {
            this._message = message;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public IMessage Message
        {
            get { return this._message; }
        }
    }
}
