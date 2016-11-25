using System;
using System.Messaging;

namespace Switchboard.Server.Messaging
{
    public class MsMessageQueue : IMessageQueue
    {
        MessageQueue _messageQueue;

        public MsMessageQueue(string path)
        {
            this._messageQueue = new MessageQueue(path);

            this._messageQueue.MessageReadPropertyFilter.CorrelationId = true;
            this._messageQueue.MessageReadPropertyFilter.Label = true;
            this._messageQueue.MessageReadPropertyFilter.AppSpecific = true;
            this._messageQueue.MessageReadPropertyFilter.Priority = true;
            this._messageQueue.MessageReadPropertyFilter.Extension = true;

            this._messageQueue.ReceiveCompleted += _messageQueue_ReceiveCompleted;
        }

        private void _messageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            if (this.ReceiveCompleted != null)
                this.ReceiveCompleted(this, e);
        }

        public void Send(object obj)
        {
            this._messageQueue.Send(obj);
        }

        public event ReceiveCompletedEventHandler ReceiveCompleted;
        
    }
}
