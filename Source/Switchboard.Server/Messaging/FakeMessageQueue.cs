using System.Messaging;

namespace Switchboard.Server.Messaging
{
    public class FakeMessageQueue : IMessageQueue
    {
        public FakeMessageQueue(string path)
        {
            
        }

        public void Send(object obj)
        {
            
        }

        public event ReceiveCompletedEventHandler ReceiveCompleted;
        
        public void RaiseMessage(Message message)
        {
            
        }
    }
}
