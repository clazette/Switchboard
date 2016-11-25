using System.Messaging;

namespace Switchboard.Common.Configuration
{
    /// <summary>
    /// Information associated with a message queue to be used by the message bus.
    /// </summary>
    public class QueueInfo
    {
        public QueueInfo(int id, string key, string path, MessageFormatterName messageFormatterName)
        {
            this.Id = id;
            this.Key = key;
            this.Path = path;

            switch (messageFormatterName)
            {
                case MessageFormatterName.Xml:
                    this.MessageFormatter = new XmlMessageFormatter();
                    break;

                case MessageFormatterName.ActiveX:
                    this.MessageFormatter = new ActiveXMessageFormatter();
                    break;
                case MessageFormatterName.Binary:
                    this.MessageFormatter = new BinaryMessageFormatter();
                    break;

                default:
                    this.MessageFormatter = null;
                    break;
            }
        }

        /// <summary>
        /// Gets or sets the unique numeric identifier of this message queue.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique string identifier of this message queue.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the path to this message queue.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the formatter that is used with this message queue.
        /// </summary>
        public IMessageFormatter MessageFormatter { get; set; }
    }
}
