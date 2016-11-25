using System;
using System.Collections.Generic;
using Switchboard.Common.ApplicationHosting;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Part activation host base class
    /// </summary>
    public abstract class PartActivationHostBase : ApplicationControlBase, IPartActivationHost
    {
        private object _syncObject = new object();

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="PartActivationHostBase"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        protected PartActivationHostBase(ActivationHostDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            this.Description = description;
            this.ActivatedTypes = new HashSet<Type>();
        }

        #endregion [Constructors]

        #region [Public Methods]

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="contractName">Optional contract name.</param>
        /// <returns>An instance of <paramref name="type"/></returns>
        public abstract object CreateInstance(Type type, string contractName = ""); 

        #endregion [Public Methods]

        #region [Internal Methods]

        /// <summary>
        /// Marks the host as faulted.
        /// </summary>
        /// <param name="exception">The exception.</param>
        internal void MarkFaulted(Exception exception)
        {
            lock (this._syncObject)
            {
                // if host is already marked, do nothing
                if (this.Faulted)
                    return;

                this.Faulted = true;


                try
                {
                    PartHostManager.OnFaulted(this, exception);
                }
                catch { }
            }
        } 

        #endregion [Internal Methods]

        #region [Public Properties]

        /// <summary>
        /// Indicates if the host is faulted.
        /// </summary>
        public bool Faulted { get; internal set; }

        /// <summary>
        /// Gets the host description.
        /// </summary>
        public ActivationHostDescription Description { get; private set; }

        /// <summary>
        /// Gets a set of types activated in this host.
        /// </summary>
        public ISet<Type> ActivatedTypes { get; private set; } 

        #endregion [Public Properties]
    }
}
