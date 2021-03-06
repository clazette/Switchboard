﻿using System.ComponentModel.Composition.Hosting;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Represents an isolation level for a part.
    /// </summary>
    public enum IsolationLevel
    {
        /// <summary>
        /// Part is activated in the same domain as the <see cref="CompositionContainer"/> used to instantiate it.
        /// </summary>
        None,

        /// <summary>
        /// Part is activated in a new app domain.
        /// </summary>
        AppDomain,
    }
}
