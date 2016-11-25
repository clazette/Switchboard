using Switchboard.Common.MessageHandling;
using System.Collections.Generic;

namespace Switchboard.Common.Configuration
{
    /// <summary>
    /// Contains the data necessary to start the message bus.
    /// </summary>
    /// <remarks>
    /// A <see cref="MessageBusInfo"/> object can be created from the data retrieved from a <see cref="MessageBusConfiguration"/> custom
    /// configuration section in the configuration file of the host process.
    /// </remarks>
    public class MessageBusInfo
    {
        /// <summary>
        /// Gets or sets the path that contains plugins that provide message handling functionality by inheriting from <see cref="MessageHandlerBase"/>.
        /// </summary>
        public string HandlerPath { get; set; }

        /// <summary>
        /// Gets or sets a list of information regarding the message queues to be used.
        /// </summary>
        public List<QueueInfo> QueueInfos { get; set; }

        /// <summary>
        /// Gets or sets the names of the types handled by all of the message handlers that are found.
        /// </summary>
        public string[] HandledTypeNames { get; set; }
    }
}
