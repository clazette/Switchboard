using System.Configuration;

namespace Switchboard.Common.Configuration
{
    /// <summary>
    /// Defines a custom configuration element collection in the configuration file of the host process.
    /// </summary>
    public class QueueConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new QueueConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as QueueConfiguration).Id;
        }

        public QueueConfiguration this[int index]
        {
            get { return this.BaseGet(index) as QueueConfiguration; }
        }
    }
}
