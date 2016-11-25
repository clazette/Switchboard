using Switchboard.Server.Messaging;

namespace Switchboard.Server.Handlers
{
    public class MessageErrorOccurredEventArgs : MessageEventArgsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageErrorOccurredEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageErrorOccurredEventArgs(IMessage message) : base(message) { }
    }
}
