using Switchboard.Common.Composition.IsolatedAppDomain;
using Switchboard.Common.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Switchboard.Common.MessageHandling;

namespace Switchboard.Common.MessageHandling
{
    [PartNotDiscoverable]
    [InheritedIsolatedExport(typeof(IMessageHandlerBase), Isolation = IsolationLevel.AppDomain, ConfigFileBaseName = "processor")]
    public abstract class MessageHandlerBase : MarshalByRefObject, IMessageHandlerBase
    {
        #region [ Private Fields ]

        private CrossDomainEventRelay<MessageErrorOccurredEventArgs> _messageErrorOccurredRelay;

        private List<Type> _handledTypes = new List<Type>();

        #endregion [ Private Fields ]

        #region [ Constructors ]

        public MessageHandlerBase(Type handledType)
        {
            this._handledTypes.Add(handledType);
        }

        public MessageHandlerBase(IEnumerable<Type> handledTypes)
        {
            this._handledTypes.AddRange(handledTypes);
        }

        #endregion [ Constructors ]

        /// <summary>
        /// Gets the handled type name.
        /// </summary>
        /// <value>The handled type name.</value>
        public string HandledTypeName
        {
            get { return string.Join("|", this._handledTypes.Select(x => x.Name)); }
        }

        /// <summary>
        /// Occurs when a message error occurred.
        /// </summary>
        public event EventHandler<MessageErrorOccurredEventArgs> MessageErrorOccurred
        {
            add
            {
                if (this._messageErrorOccurredRelay == null)
                    this._messageErrorOccurredRelay = new CrossDomainEventRelay<MessageErrorOccurredEventArgs>();
                this._messageErrorOccurredRelay.RelayEvent += value;
            }
            remove
            {
                if (this._messageErrorOccurredRelay == null)
                    this._messageErrorOccurredRelay = new CrossDomainEventRelay<MessageErrorOccurredEventArgs>();
                this._messageErrorOccurredRelay.RelayEvent -= value;
            }
        }

        /// <summary>
        /// Determines whether this instance can process the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///     <c>true</c> if this instance can the specified message; otherwise, <c>false</c>.
        /// </returns>
        public bool CanProcessMessage(SerializableMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            try
            {
                object body = message.GetBody();

                if (body == null) return false;

                if (!this._handledTypes.Contains(body.GetType())) return false;
                
                MessageRetryData messageRetryData = message.GetRetryData();

                if (messageRetryData.Attempts == 1) return true;

                if (messageRetryData.RetrySourceTypeName != this.GetType().FullName) return false;

                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Processes the message data.
        /// </summary>
        /// <param name="message">The message.</param>
        public virtual void ProcessMessageData(SerializableMessage message) { }  

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. 
        /// This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized 
        /// to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
        /// <PermissionSet>
        ///     <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        public override object InitializeLifetimeService()
        {
            // prevent lifetime services from releasing our object.
            return null;
        }
    }
}
