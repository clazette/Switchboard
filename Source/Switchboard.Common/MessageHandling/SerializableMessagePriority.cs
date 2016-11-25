using System;
using System.Messaging;
using System.Runtime.Serialization;

namespace Switchboard.Common.MessageHandling
{
    /// <summary>
    /// A serializable wrapper enumeration for <see cref="System.Messaging.MessagePriority"/>
    /// </summary>
    [Serializable]
    [DataContract]
    public enum SerializableMessagePriority
    {
#pragma warning disable 1591  // XML comments are unnecessary for each enum member (it couldn't be more intuitive)

        [EnumMember]
        Lowest = MessagePriority.Lowest,

        [EnumMember]
        VeryLow = MessagePriority.VeryLow,

        [EnumMember]
        Low = MessagePriority.Low,

        [EnumMember]
        Normal = MessagePriority.Normal,

        [EnumMember]
        AboveNormal = MessagePriority.AboveNormal,

        [EnumMember]
        High = MessagePriority.High,

        [EnumMember]
        VeryHigh = MessagePriority.VeryHigh,

        [EnumMember]
        Highest = MessagePriority.Highest,

#pragma warning restore 1591
    }
}
