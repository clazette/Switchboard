using System;
using System.Diagnostics.CodeAnalysis;
using Switchboard.Common.MessageHandling;

namespace Switchboard.Common.Events
{
    /// <summary>
    /// Base message event arguments.
    /// </summary>
    [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP0100:AdvancedNamingRules", Justification = "Deriving a new base event class.")]
    [Serializable]
    public abstract class MessageEventArgsBase : EventArgs
    {
        private SerializableMessage _message;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventArgsBase"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageEventArgsBase(SerializableMessage message)
        {
            this._message = message;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public SerializableMessage Message
        {
            get { return this._message; }
        }
    }
}
