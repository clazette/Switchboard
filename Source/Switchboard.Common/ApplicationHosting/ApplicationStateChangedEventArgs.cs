using System;

namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// Application state changed event arguments.
    /// </summary>
    public class ApplicationStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="previousState">Previous state.</param>
        /// <param name="newState">The new state.</param>
        public ApplicationStateChangedEventArgs(ApplicationState previousState, ApplicationState newState)
        {
            this.PreviousState = previousState;
            this.NewState = newState;
        }

        /// <summary>
        /// Gets the previous state.
        /// </summary>
        /// <value>The state of the previous.</value>
        public ApplicationState PreviousState { get; private set; }

        /// <summary>
        /// Gets the new state.
        /// </summary>
        /// <value>The new state.</value>
        public ApplicationState NewState { get; private set; }
    }
}
