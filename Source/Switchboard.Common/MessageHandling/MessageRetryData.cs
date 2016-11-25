using System;
using System.Runtime.Serialization;

namespace Switchboard.Common.MessageHandling
{
    /// <summary>
    /// Stores data about a <see cref="Message"/> instance regarding its retry status.
    /// </summary>
    [DataContract]
    public class MessageRetryData
    {
        /// <summary>
        /// Gets or sets the path of the queue in which the message was first received by the system.
        /// </summary>
        [DataMember]
        public string OriginalQueuePath { get; set; }

        /// <summary>
        /// Gets or sets the FullName property of the <see cref="Type"/> of the message handler that requested the retry.
        /// </summary>
        [DataMember]
        public string RetrySourceTypeName { get; set; }

        /// <summary>
        /// Gets or sets the number of times this message has been submitted for processing but resulted in a retry request.
        /// </summary>
        [DataMember]
        public int Attempts { get; set; }
    }
}
