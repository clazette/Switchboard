using System;
using System.Collections.Generic;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Interface for part activation hosts.
    /// </summary>
    public interface IPartActivationHost
    {
        /// <summary>
        /// Gets the activated types.
        /// </summary>
        /// <value>The activated types.</value>
        ISet<Type> ActivatedTypes { get; }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="contractName">Optional contract name.</param>
        /// <returns>An instance of <paramref name="type"/></returns>
        object CreateInstance(Type type, string contractName = "");

        /// <summary>
        /// Gets the host description.
        /// </summary>
        /// <value>The description.</value>
        ActivationHostDescription Description { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IPartActivationHost"/> is faulted.
        /// </summary>
        /// <value><c>true</c> if faulted; otherwise, <c>false</c>.</value>
        bool Faulted { get; }
    }
}
