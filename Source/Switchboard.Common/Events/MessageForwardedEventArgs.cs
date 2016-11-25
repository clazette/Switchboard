using System;
using Switchboard.Common.MessageHandling;

namespace Switchboard.Common.Events
{
    /// <summary>
    /// Message Forwarded event arguments.
    /// </summary>
    [Serializable]
    public class MessageForwardedEventArgs : MessageEventArgsBase
    {
        private string _queueKey;

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get { return this._queueKey; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageForwardedEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="queueKey">The queue key.</param>
        public MessageForwardedEventArgs(SerializableMessage message, string queueKey)
            : base(message)
        {
            this._queueKey = queueKey;
        }
    }
}
