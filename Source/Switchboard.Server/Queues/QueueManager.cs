using System;
using Switchboard.Common.Configuration;
using Switchboard.Server.Messaging;
using System.Collections.Generic;
using System.Messaging;
using Switchboard.Common.MessageHandling;

namespace Switchboard.Server.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public class QueueManager : IQueueManager
    {
        private Dictionary<string, MessageQueue> _queueDictionary;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public QueueManager(Dictionary<string, MessageQueue> queueDictionary)
        {
            this._queueDictionary = queueDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageBusInfo"></param>
        public void Initialize()
        {
            try
            {
                foreach (var entry in this._queueDictionary)
                {
                    entry.Value.MessageReadPropertyFilter = GetMessageReadPropertyFilter();
                    entry.Value.ReceiveCompleted += new ReceiveCompletedEventHandler(this.OnReceiveCompleted);
                }
            }
            catch (Exception ex)
            {
            }

            this.StartListening();
        }

        private MessagePropertyFilter GetMessageReadPropertyFilter()
        {
            var messagePropertyFilter = new MessagePropertyFilter();

            messagePropertyFilter.CorrelationId = true;
            messagePropertyFilter.Label = true;
            messagePropertyFilter.AppSpecific = true;
            messagePropertyFilter.Priority = true;
            messagePropertyFilter.Extension = true;

            return messagePropertyFilter;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartListening()
        {
            foreach (var entry in this._queueDictionary)
            {
                entry.Value.BeginReceive();
            }
        }

        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            MessageQueue queue = sender as MessageQueue;

            Message message = queue.EndReceive(e.AsyncResult);

            // If this is the first time this message has been handled, we need to save the path
            // of the queue it came from in case it needs to be retried.
            MessageRetryData messageRetryData = message.GetRetryData();

            queue.BeginReceive();
        }
    }
}
