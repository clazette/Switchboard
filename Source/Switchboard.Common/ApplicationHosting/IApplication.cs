using System;
using System.Collections.Generic;

namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// Defines the interface to control and query an application.
    /// </summary>
    public interface IApplication : IApplicationServiceController
    {
        /// <summary>
        /// Invokes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>A command specific return value.</returns>
        object Invoke(string command, IEnumerable<IApplicationInvokeArgument> arguments);

        /// <summary>
        /// Gets the application descriptor.
        /// </summary>
        /// <value>The application descriptor.</value>
        IApplicationDescriptor ApplicationDescriptor { get; }

        /// <summary>
        /// Gets the application id.
        /// </summary>
        /// <value>The application id.</value>
        Guid ApplicationId { get; }
    }
}
