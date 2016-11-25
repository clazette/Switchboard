using Switchboard.Common.Configuration;
using Switchboard.Server.Messaging;

namespace Switchboard.Server.RetryManager
{
    /// <summary>
    /// 
    /// </summary>
    public class RetryManager : IRetryManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageBusInfo"></param>
        public void Initialize(MessageBusInfo messageBusInfo)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void RetryMessage(IMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
