using System.Configuration;

namespace Switchboard.Common.Configuration
{
    public class QueueConfiguration : ConfigurationElement
    {
        /// <summary>
        /// Defines a custom configuration element in the configuration file of the host process.
        /// </summary>
        private const string InvalidCharacterList = "~!@#$%&*()<>[]{}/;'|";

        /// <summary>
        /// Gets the unique numeric identifier of a configured message queue.
        /// </summary>
        [ConfigurationProperty("Id", DefaultValue=0, IsRequired=true, IsKey=true)]
        public int Id { get { return (int)this["Id"]; } }

        /// <summary>
        /// Gets the unique string identifier of a configured message queue.
        /// </summary>
        [ConfigurationProperty("Key", DefaultValue = "Default", IsRequired = true, IsKey = true)]
        [StringValidator(InvalidCharacters = InvalidCharacterList, MinLength = 1, MaxLength = 255)]
        public string Key { get { return this["Key"] as string; } }

        /// <summary>
        /// Gets the path of the message queue.
        /// </summary>
        [ConfigurationProperty("Path", DefaultValue = "C:\\MessageHandlers", IsRequired = true, IsKey = true)]
        [StringValidator(MinLength = 0, MaxLength = 255)]
        public string Path { get { return this["Path"] as string; } }

        /// <summary>
        /// Gets the name of the message formatter to use for the queue.
        /// </summary>
        [ConfigurationProperty("MessageFormatterName", DefaultValue = MessageFormatterName.Xml, IsRequired = true, IsKey = true)]
        public MessageFormatterName MessageFormatter { get { return (MessageFormatterName)this["MessageFormatterName"]; } }
    }
}
