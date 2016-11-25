using System.Collections.Generic;

namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// Defines the <see cref="IMesApplication"/> configuration interface.
    /// </summary>
    public interface IApplicationConfiguration
    {
        /// <summary>
        /// Gets the configuration object.
        /// </summary>
        /// <value>The configuration object.</value>
        /// <remarks>
        /// This can be anything the application can consume. The application needs to be the one that knows how
        /// to consume this object.
        /// </remarks>
        object Config { get; }

        /// <summary>
        /// Gets the application descriptor used to identify the application and the version to load.
        /// </summary>
        /// <value>The application descriptor.</value>
        IApplicationDescriptor ApplicationDescriptor { get; }

        /// <summary>
        /// Gets the dependencies for this application.
        /// </summary>
        /// <value>The dependencies.</value>
        /// <remarks>
        /// This is to allow the application host to probe the corresponding application folders for assemblies. The configuration needs to specify
        /// additional application descriptors that this application is dependent upon, e.g. Framework version 2.0.0.0.
        /// </remarks>
        IEnumerable<IApplicationDescriptor> Dependencies { get; }
    }
}
