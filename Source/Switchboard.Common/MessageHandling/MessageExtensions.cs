using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Messaging;

namespace Switchboard.Common.MessageHandling
{
    /// <summary>
    /// <see cref="SerializableMessage"/> extensions
    /// </summary>
    public static class SerializedMessageExtensions
    {
        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedMessage">The serialized message.</param>
        /// <returns>The message body as <typeparamref name="T"/></returns>
        public static T GetBody<T>(this SerializableMessage serializedMessage)
        {
            return (T)serializedMessage.GetBody();
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <param name="serializedMessage">The serialized message.</param>
        /// <returns>The message body as an <see cref="Object"/></returns>
        public static object GetBody(this SerializableMessage serializedMessage)
        {
            if (serializedMessage == null)
                throw new ArgumentNullException("serializedMessage");

            lock (serializedMessage)
            {
                // create an instance of the message formatter supplied in the serialized message.
                IMessageFormatter messageFormatter = (IMessageFormatter)Activator.CreateInstance(serializedMessage.FormatterType);

                // now use that formatter to read the message.
                using (MemoryStream memoryStream = new MemoryStream(serializedMessage.MessageData))
                {
                    using (Message message = new Message())
                    {
                        message.BodyStream = memoryStream;

                        if (messageFormatter.CanRead(message))
                            return messageFormatter.Read(message);
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Sets the body.
        /// </summary>
        /// <param name="serializedMessage">The serialized message.</param>
        /// <param name="body">The body.</param>
        /// <returns>The <see cref="SerializableMessage"/></returns>
        public static SerializableMessage SetBody(this SerializableMessage serializedMessage, object body)
        {
            if (serializedMessage == null)
                throw new ArgumentNullException("serializedMessage");

            lock (serializedMessage)
            {
                // create an instance of the message formatter supplied in the serialized message.
                IMessageFormatter messageFormatter = serializedMessage.GetFormatter();

                // in order to ensure that the message is written to the MessageData property correctly,
                // use the message formatter to write to a new Message object, then extract the serialized
                // data from the Message.BodyStream.
                using (Message message = new Message())
                {
                    // setup the message object...
                    message.Formatter = messageFormatter;
                    message.Body = body;

                    // use the formatter to write the object to the message
                    // this populates the message.BodyStream property.
                    messageFormatter.Write(message, body);

                    // reset the stream position to zero just in case the message formatter neglects to do so...
                    message.BodyStream.Position = 0;

                    serializedMessage.MessageData = StreamUtility.ReadFully(message.BodyStream);

                    return serializedMessage;
                }
            }
        }

        /// <summary>
        /// Sets the body.
        /// </summary>
        /// <param name="serializedMessage">The serialized message.</param>
        /// <param name="bodyStream">The body stream.</param>
        /// <returns>The <see cref="SerializableMessage"/></returns>
        [SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Justification = "Needs to be thread safe.")]
        public static SerializableMessage SetBodyStream(this SerializableMessage serializedMessage, Stream bodyStream)
        {
            if (serializedMessage == null)
                throw new ArgumentNullException("serializedMessage");

            if (bodyStream == null)
                throw new ArgumentNullException("bodyStream");

            lock (serializedMessage)
            {
                lock (bodyStream)
                {
                    bodyStream.Position = 0;
                    serializedMessage.MessageData = StreamUtility.ReadFully(bodyStream);
                    return serializedMessage;
                }
            }
        }

        /// <summary>
        /// Gets the formatter.
        /// </summary>
        /// <param name="serializedMessage">The serialized message.</param>
        /// <returns>An instance that implements <see cref="IMessageFormatter"/></returns>
        public static IMessageFormatter GetFormatter(this SerializableMessage serializedMessage)
        {
            if (serializedMessage == null)
                throw new ArgumentNullException("serializedMessage");

            lock (serializedMessage)
            {
                if (serializedMessage.FormatterType == null)
                    throw new ArgumentNullException("serializedMessage", "The serializable message formatter type cannot be null.");

                IMessageFormatter messageFormatter = (IMessageFormatter)Activator.CreateInstance(serializedMessage.FormatterType);
                return messageFormatter;
            }
        }

        /// <summary>
        /// Converts the <see cref="SerializableMessage"/> to a <see cref="Message"/>.
        /// </summary>
        /// <param name="serializedMessage">The serialized message.</param>
        /// <param name="setBody">if set to <c>true</c> set <see cref="Message.Body"/> property to the deserialized object.</param>
        /// <returns>A <see cref="Message"/></returns>
        public static Message AsMessage(this SerializableMessage serializedMessage, bool setBody = false)
        {
            if (serializedMessage == null)
                throw new ArgumentNullException("serializedMessage");


            // use two message variables to satisfy CA for disposing objects on exception paths....
            Message message = null;
            Message returnMessage = null;

            try
            {
                lock (serializedMessage)
                {
                    /////////////////////////////////////////////////////////////////////////////
                    // to convert to a Sytem.Messaging.Message: 
                    // 1. create a new Message object
                    // 2. set the formatter
                    // 3. copy serialized properties
                    // 4. get the object "body" from the serialized message
                    // 5. set the body to the message and use the formatter to write it
                    //    so that the BodyStream is populated.

                    message = new Message();
                    message.Formatter = serializedMessage.GetFormatter();
                    message.CorrelationId = serializedMessage.CorrelationId;
                    message.Label = serializedMessage.Label;
                    message.AppSpecific = serializedMessage.AppSpecific;
                    message.Priority = (MessagePriority)serializedMessage.MessagePriority;
                    message.Extension = serializedMessage.Extension;

                    if (setBody)
                    {
                        object body = serializedMessage.GetBody();
                        message.Body = body;
                        message.Formatter.Write(message, body);
                    }
                    else
                    {
                        message.BodyStream = new MemoryStream(serializedMessage.MessageData);
                    }
                    /////////////////////////////////////////////////////////////////////////////
                }

                returnMessage = message;
                message = null;

                return returnMessage;
            }
            finally
            {
                if (message != null)
                    message.Dispose();
            }

        }

        /// <summary>
        /// Converts the <see cref="Message"/> to a <see cref="SerializableMessage">serializable message</see>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="SerializableMessage"/></returns>
        [SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Justification = "Needs to be thread safe.")]
        public static SerializableMessage AsSerializableMessage(this Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            lock (message)
            {
                if (message.Formatter == null)
                    throw new ArgumentNullException("message", "The message's formatter cannot be null.");

                /////////////////////////////////////////////////////////////////////////////
                // To convert a System.Messaging.Message to a SerializableMessage:
                // 1. extract the formatter type from the message
                // 2. extract serialized additional properties
                // 3. set the message.BodyStream to the serialized message MessageData property. 
                //    see SetBodyStream extension method

                SerializableMessage serializedMessage = new SerializableMessage();
                serializedMessage.CorrelationId = message.Id;
                serializedMessage.FormatterType = message.Formatter.GetType();
                serializedMessage.Label = message.Label;
                serializedMessage.AppSpecific = message.AppSpecific;
                serializedMessage.MessagePriority = (SerializableMessagePriority)message.Priority;
                serializedMessage.Extension = message.Extension;

                bool canRead = false;

                try
                {
                    canRead = message.Formatter.CanRead(message);
                }
                catch// if an exception is thrown, most likely that the BodyStream isn't set.
                {
                    if (message.Body == null) // nothing more to do...
                        return serializedMessage;
                }

                if (canRead)
                    serializedMessage.SetBodyStream(message.BodyStream);
                else
                {
                    message.Formatter.Write(message, message.Body);
                    serializedMessage.SetBodyStream(message.BodyStream);
                }

                /////////////////////////////////////////////////////////////////////////////

                return serializedMessage;
            }
        }
    }
}
