using System;
using Switchboard.Common.MessageHandling;

namespace Switchboard.Common.Events
{
    /// <summary>
    /// Message Error Occurred event arguments.
    /// </summary>
    [Serializable]
    public class MessageErrorOccurredEventArgs : MessageEventArgsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageErrorOccurredEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageErrorOccurredEventArgs(SerializableMessage message) : base(message) { }
    }
}
