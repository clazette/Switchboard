using System;
using System.Diagnostics.CodeAnalysis;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    // Note: This was the most recent signature of the class. We had to revert to the old class because the MBM
    //       is currently using framework v1.26 and this was a breaking change.  26-Sep-2012
    // public class CrossDomainEventRelay<T> : AppDomainRelayEvent<T> where T : EventArgs

    /// <summary>
    /// Event Relay class to allow events to pass from one domain to the other without assembly dependency issues
    /// </summary>
    /// <typeparam name="T">Type of the event arguments. Must be derived from <see cref="EventArgs"/>.</typeparam>
    public class CrossDomainEventRelay<T> : MarshalByRefObject where T : EventArgs
    {
        /// <summary>
        /// Provides an event for the host application to listen for status messages coming from the Dependant Application
        /// </summary>
        public event EventHandler<T> RelayEvent;

        /// <summary>
        /// This handler acts as a relay for the StatusMessage Event
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Considered and rejected because we want this to be a way for callers to raise the relay event.")]
        public void FireRelayEvent(object sender, T args)
        {
            if (this.RelayEvent != null)
                this.RelayEvent(sender, args);
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        public override object InitializeLifetimeService()
        {
            // prevent lifetime services from destroying our instance...
            return null;
        }
    }
}
