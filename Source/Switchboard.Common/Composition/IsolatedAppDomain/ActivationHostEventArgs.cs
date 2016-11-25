using System;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Activation Host <see cref="EventArgs"/>
    /// </summary>
    public class ActivationHostEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public ActivationHostDescription Description { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationHostEventArgs"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="cause">The cause.</param>
        public ActivationHostEventArgs(ActivationHostDescription description, Exception cause)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            Description = description;
            Cause = cause;
        }

        /// <summary>
        /// Gets or sets the cause.
        /// </summary>
        /// <value>The cause.</value>
        public Exception Cause { get; private set; }
    }
}
