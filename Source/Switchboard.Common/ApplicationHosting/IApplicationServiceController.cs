using System;

namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// Defines controller interface for MES Applications and Application Services.
    /// </summary>
    public interface IApplicationServiceController : IDisposable
    {
        /// <summary>
        /// Starts this application service.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this application service.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets the state of the application.
        /// </summary>
        /// <value>The state of the application.</value>
        ApplicationState ApplicationState { get; }

        /// <summary>
        /// Gets/Sets the owning <see cref="IMesApplication"/>.
        /// </summary>
        /// <value>The owner.</value>
        IApplication Owner { get; set; }
    }
}
