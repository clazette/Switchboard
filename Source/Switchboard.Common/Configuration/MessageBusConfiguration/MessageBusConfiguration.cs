using System.Configuration;

namespace Switchboard.Common.Configuration
{
    /// <summary>
    /// Defines a custom configuration section in the configuratio file of the host process.
    /// </summary>
    /// <remarks>
    /// After this class is has deserialized the data from the configuration file, it can be 
    /// projected in to a <see cref="MessageBusInfo"/> instance for use by the rest of the message bus system.
    /// </remarks>
    public class MessageBusConfiguration : ConfigurationSection
    {
        private const string InvalidCharacterList = "~!@#$%&*()<>[]{}/;'|";

        /// <summary>
        /// Gets the path that contains plugins that provide message handling functionality by inheriting from <see cref="MessageHandlerBase"/>.
        /// </summary>
        [ConfigurationProperty("HandlerPath", DefaultValue = "c:\\MessageHandlers", IsRequired=false, IsKey=false)]
        [StringValidator(InvalidCharacters = InvalidCharacterList, MinLength=4, MaxLength=255)]
        public string HandlerPath { get { return this["HandlerPath"] as string; } }

        /// <summary>
        /// Gets a list of information describing the message queues to be used.
        /// </summary>
        [ConfigurationProperty("Queues", IsDefaultCollection=false)]
        public QueueConfigurationCollection Queues { get { return base["Queues"] as QueueConfigurationCollection; } }

        /// <summary>
        /// Gets information regarding how message handling should be retried when messages are returned by handlers becasue they cannot be processed.
        /// </summary>
        [ConfigurationProperty("RetryBehavior", IsDefaultCollection = false)]
        public RetryBehaviorConfiguration RetryBehavior { get { return base["RetryBehavior"] as RetryBehaviorConfiguration; } }
    }
}
