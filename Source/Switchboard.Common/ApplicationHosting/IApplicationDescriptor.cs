using System;
using System.Collections.Generic;

namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationDescriptor
    {
        /// <summary>
        /// Gets the version of the <see cref="IMesApplication"/>.
        /// </summary>
        /// <value>The version.</value>
        Version Version { get; }

        /// <summary>
        /// Gets the friendly name of the <see cref="IMesApplication"/>.
        /// </summary>
        /// <value>The <see cref="IMesApplication"/> friendly name .</value>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the description of the <see cref="IMesApplication"/>..
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }

        /// <summary>
        /// Gets the application factory.
        /// </summary>
        /// <value>The application factory.</value>
        IMesApplicationFactory ApplicationFactory { get; }

        /// <summary>
        /// Gets the default dependencies for this application.
        /// </summary>
        /// <value>The dependencies.</value>
        /// <remarks>
        /// This is to allow the application host to probe the corresponding application folders for assemblies. The configuration needs to specify
        /// additional application descriptors that this application is dependent upon, e.g. Framework version 2.0.0.0.
        /// </remarks>
        IEnumerable<IApplicationDescriptor> DefaultDependencies { get; }
    }
}
