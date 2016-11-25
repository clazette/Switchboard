using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Switchboard.Common.Events;
using Switchboard.Common.MessageHandling;
using Switchboard.Common.Composition.IsolatedAppDomain;
using Switchboard.Server.Messaging;
using Switchboard.Common.Logger;
using System.Collections.Concurrent;
using System.Messaging;
using System.Threading.Tasks;
using System.Linq;

namespace Switchboard.Server.Handlers
{
    public class HandlerManager : IHandlerManager
    {
        #region [ Private Fields ]

        [ImportMany(typeof(IMessageHandlerBase))]
        private List<IMessageHandlerBase> _messageHandlerBaseList;

        private ILogger _logger;

        /// <summary>
        /// Using a <see cref="CrossDomainEventRelay{T}"/> ensures that the object handling the <see cref="MessageForwarded"/>
        /// event is derived from MarshalByRefObject and shared across both domains. Otherwise the other domain would need the
        /// assembly this class is declared in and ProcessorManager would need to be derived from MarshalByRefObject. By using 
        /// the CrossDomainEventRelay, we isolate the shared components and makes this cleaner.
        /// </summary>
        private CrossDomainEventRelay<MessageForwardedEventArgs> _messageForwardedRelay = new CrossDomainEventRelay<MessageForwardedEventArgs>();

        /// <summary>
        /// Using a <see cref="CrossDomainEventRelay{T}"/> ensures that the object handling the <see cref="MessageErrorOccurred"/>
        /// event is derived from MarshalByRefObject and shared across both domains. Otherwise the other domain would need the
        /// assembly this class is declared in and ProcessorManager would need to be derived from MarshalByRefObject. By using 
        /// the CrossDomainEventRelay, we isolate the shared components and makes this cleaner.
        /// </summary>
        private CrossDomainEventRelay<MessageErrorOccurredEventArgs> _messageErrorOccurredRelay = new CrossDomainEventRelay<MessageErrorOccurredEventArgs>();

        #endregion [ Private Fields ]

        public event EventHandler<MessageErrorOccurredEventArgs> MessageErrorOccurred;

        public bool TryInitializion(ILogger logger)
        {
            this._logger = logger;

            if(this._messageHandlerBaseList == null || this._messageHandlerBaseList.Count == 0)
            {
                this._logger.LogInformation("No handlers found.");
                return false;
            }

            this._logger.LogInformation(string.Format("{0} {1}", this._messageHandlerBaseList.Count, "handlers loaded."));
            this._messageHandlerBaseList.ForEach(handler => this._logger.LogInformation(handler.GetType().Name));

            return true;
        }

        public void ProcessMessage(Message message)
        {
            if (message == null) throw new ArgumentNullException("message");

            List<IMessageHandlerBase> processors = null;
            ConcurrentQueue<Exception> exceptionQueue = new ConcurrentQueue<Exception>();

            //////////////////////////////////////////////////////////////////////////////////////////////////
            // have to do this here because access to the underlying message.BodyStream in not thread safe...
            SerializableMessage serializedMessage = message.AsSerializableMessage();
            //////////////////////////////////////////////////////////////////////////////////////////////////

            try
            {
                // When no processor exists for a given type, the GetType invocation throws an exception.
                processors = this.ResolveProcessor(serializedMessage);

                if (processors == null)
                {
                    // TODO: Send log message.
                    return;
                }
            }
            catch (Exception ex)
            {
                // TODO: Send log message.
                return;
            }

            Parallel.ForEach(processors, ((processor) =>
            {
                try
                {
                    processor.ProcessMessageData(serializedMessage);
                }
                catch (Exception ex)
                {
                    exceptionQueue.Enqueue(ex);
                }
            }));

            // Log here so that we know something has happened within a processor but we can't bring the down service over this.
            // So, get something on the event log and press on.
            if (exceptionQueue.Count > 0)
            {
                try
                {
                    foreach (Exception exception in exceptionQueue)
                    {
                        // TODO: Send log message.
                    }
                }
                catch
                {
                    // Eat it...keep the service alive.
                }
            }
        }

        /// <summary>
        /// Returns an instance of the MessageProcessorBase that handles messageType.
        /// </summary>
        /// <param name="serializedMessage">The message to be processed.</param>
        /// <returns>A List of of MessageProcessorBase classes that handle the message.</returns>
        private List<IMessageHandlerBase> ResolveProcessor(SerializableMessage serializedMessage)
        {
            return (from processor in this._messageHandlerBaseList.AsParallel()
                    where processor.CanProcessMessage(serializedMessage)
                    select processor).ToList();
        }
    }
}
