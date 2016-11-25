using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Messaging;
using System.Runtime.Serialization;

namespace Switchboard.Common.MessageHandling
{
    /// <summary>
    /// Class wrapper for a serialized <see cref="Message"/>
    /// </summary>
    [DataContract]
    [Serializable]
    public class SerializableMessage
    {
        /// <summary>
        /// Gets or sets the correlation id.
        /// </summary>
        /// <value>The correlation id.</value>
        [DataMember]
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the type of the formatter.
        /// </summary>
        /// <value>The type of the formatter.</value>
        [DataMember]
        public Type FormatterType { get; set; }

        /// <summary>
        /// Gets or sets the message data.
        /// </summary>
        /// <value>The message data.</value>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "For serialization.")]
        [DataMember]
        public byte[] MessageData { get; set; }

        /// <summary>
        /// Gets or sets the extension data.
        /// </summary>
        /// <value>The extension data.</value>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "For serialization.")]
        [DataMember]
        public byte[] Extension { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        [DataMember]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the application specific data.
        /// </summary>
        /// <value>The application specific data.</value>
        [DataMember]
        public int AppSpecific { get; set; }

        /// <summary>
        /// Gets or sets the message priority.
        /// </summary>
        /// <value>The message priority.</value>
        [DataMember]
        public SerializableMessagePriority MessagePriority { get; set; }
    }
}
