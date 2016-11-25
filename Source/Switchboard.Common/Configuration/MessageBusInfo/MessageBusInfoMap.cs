using System;
using System.Collections.Generic;

namespace Switchboard.Common.Configuration
{
    public static class MessageBusInfoMap
    {
        public static MessageBusInfo Map(this MessageBusConfiguration messageBusConfiguration)
        {
            if (messageBusConfiguration == null)
                throw new ArgumentNullException(paramName: "messageBusConfiguration");

            var messageBusInfo = new MessageBusInfo();

            messageBusInfo.HandlerPath = messageBusConfiguration.HandlerPath;

            var queueInfos = new List<QueueInfo>();

            foreach (QueueConfiguration queueConfiguration in messageBusConfiguration.Queues)
            {
                queueInfos.Add( new QueueInfo(queueConfiguration.Id, 
                                              queueConfiguration.Key, 
                                              queueConfiguration.Path, 
                                              queueConfiguration.MessageFormatter)
                              );
            }

            messageBusInfo.QueueInfos = queueInfos;

            return messageBusInfo;
        }
    }
}
