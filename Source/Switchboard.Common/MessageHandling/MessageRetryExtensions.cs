using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Switchboard.Common.MessageHandling
{
    public static class MessageRetryExtensions
    {
        /// <summary>
        /// Retrieves and deserializes the Extension property of a <see cref="SerializableMessage"/> if it is
        /// a <see cref="MessageRetryData"/>, othewise returns null.
        /// </summary>
        /// <param name="serializableMessage">The <see cref="SerializableMessage"/> from which to retrieve the Extension property.</param>
        /// <returns>The <see cref="MessageRetryData"/> instance serialized in the message if there is one, otherwise null.</returns>
        public static MessageRetryData GetRetryData(this SerializableMessage serializableMessage)
        {
            if (serializableMessage == null) throw new ArgumentNullException("serializableMessage");

            return ConvertToObject(serializableMessage.Extension);
        }

        /// <summary>
        /// Retrieves and deserializes the Extension property of a <see cref="Message"/> if it is
        /// a <see cref="MessageRetryData"/>, othewise returns null.
        /// </summary>
        /// <param name="message">The <see cref="Message"/> from which to retrieve the Extension property.</param>
        /// <returns>The <see cref="MessageRetryData"/> instance serialized in the message if there is one, otherwise null.</returns>
        public static MessageRetryData GetRetryData(this Message message)
        {
            if (message == null) throw new ArgumentNullException("message");

            return ConvertToObject(message.Extension);
        }

        private static MessageRetryData ConvertToObject(byte[] bytes)
        {
            MessageRetryData messageRetryData = null;

            using (var memoryStream = new MemoryStream(bytes))
            {
                var serializer = new DataContractSerializer(typeof(MessageRetryData));

                if (memoryStream.Length != 0)
                    messageRetryData = serializer.ReadObject(memoryStream) as MessageRetryData;
            }

            return messageRetryData;
        }

        /// <summary>
        /// Serializes and assigns a <see cref="MessageRetryData"/> instance to the Extension 
        /// property of a <see cref="SerializableMessage"/>.
        /// </summary>
        /// <param name="serializableMessage">The object on which the Extension property will be set.</param>
        /// <param name="messageRetryData">The object that will be serialized and written to the Extension property.</param>
        /// <returns>The <see cref="SerializedMessage"/> instance after the <see cref="MessageRetryData"/> instance has been serialized and assigned to its Extension property.</returns>
        public static SerializableMessage SetRetryData(this SerializableMessage serializableMessage, MessageRetryData messageRetryData)
        {
            if (serializableMessage == null) throw new ArgumentNullException("serializableMessage");
            if (messageRetryData == null) throw new ArgumentNullException("messageRetryData");

            serializableMessage.Extension = ConvertToBytes(messageRetryData);

            return serializableMessage;
        }

        /// <summary>
        /// Serializes and assigns a <see cref="MessageRetryData"/> instance to the Extension 
        /// property of a <see cref="Message"/>.
        /// </summary>
        /// <param name="message">The object on which the Extension property will be set.</param>
        /// <param name="messageRetryData">The object that will be serialized and written to the Extension property.</param>
        /// <returns>The <see cref="SerializedMessage"/> instance after the <see cref="MessageRetryData"/> instance has been serialized and assigned to its Extension property.</returns>
        public static Message SetRetryData(this Message message, MessageRetryData messageRetryData)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (messageRetryData == null) throw new ArgumentNullException("messageRetryData");

            message.Extension = ConvertToBytes(messageRetryData);

            return message;
        }

        private static byte[] ConvertToBytes(MessageRetryData messageRetryData)
        {
            byte[] bytes;

            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(MessageRetryData));

                serializer.WriteObject(memoryStream, messageRetryData);

                bytes = memoryStream.ToArray();
            }

            return bytes;
        }
    }
}
