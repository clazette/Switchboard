using System.ComponentModel.Composition;

namespace Switchboard.Common.MessageHandling
{
    [InheritedExport(typeof(IMessageHandlerBase))]
    public interface IMessageHandlerBase
    {
        /// <summary>
        /// Determines whether this instance can process the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///     <c>true</c> if this instance can the specified message; otherwise, <c>false</c>.
        /// </returns>
        bool CanProcessMessage(SerializableMessage message);

        /// <summary>
        /// Processes the message data.
        /// </summary>
        /// <param name="message">The message.</param>
        void ProcessMessageData(SerializableMessage message);
    }
}
