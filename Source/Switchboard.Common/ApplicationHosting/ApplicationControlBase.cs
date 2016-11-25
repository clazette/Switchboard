using System;

namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// Base class for controllable application objects.
    /// </summary>
    public abstract class ApplicationControlBase : MarshalByRefObject, IApplicationServiceController
    {
        #region [Private Fields]

        private ApplicationState _applicationState;
        private IApplication _owner;

        #endregion [Private Fields]

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="MesApplicationControlBase"/> class.
        /// </summary>
        protected ApplicationControlBase()
        {
            // don't use the property as that results in a virtual call and makes CA unhappy...
            // plus it is not needed to raise the event in the c'tor
            this._applicationState = ApplicationState.Loaded;
        }

        #endregion [Constructors]

        #region [Public]

        #region [Public Methods]

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (this.ApplicationState == ApplicationState.Starting || this.ApplicationState == ApplicationState.Running)
                this.Stop();

            if (!OnBeforeStart())
                return;

            this.ApplicationState = ApplicationState.Starting;

            OnStart();

            this.ApplicationState = ApplicationState.Running;

            OnAfterStart();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (this.ApplicationState == ApplicationState.Stopped || this.ApplicationState == ApplicationState.Stopping)
                return;

            if (!OnBeforeStop())
                return;

            this.ApplicationState = ApplicationState.Stopping;

            OnStop();

            this.ApplicationState = ApplicationState.Stopped;

            OnAfterStop();
        }

        #endregion [Public Methods]

        #region [Public Properties]

        /// <summary>
        /// Gets the state of the application.
        /// </summary>
        /// <value>The state of the application.</value>
        public ApplicationState ApplicationState
        {
            get
            {
                return this._applicationState;
            }
            protected set
            {
                if (this._applicationState != value)
                {
                    var previousState = this._applicationState;
                    this._applicationState = value;
                    this.OnApplicationStateChanged(new ApplicationStateChangedEventArgs(previousState, value));
                }
            }
        }

        #region [IMesApplicationService Members]

        /// <summary>
        /// Gets/Sets the owning <see cref="IMesApplication"/>.
        /// </summary>
        /// <value>The owner.</value>
        public IApplication Owner
        {
            get
            {
                return this._owner;
            }
            set
            {
                this._owner = value;
            }
        }

        #endregion [IMesApplicationService Members]

        #endregion [Public Properties]

        #region [Public Events]

        /// <summary>
        /// Fired when application state has changed.
        /// </summary>
        public event EventHandler<ApplicationStateChangedEventArgs> ApplicationStateChanged;

        #endregion [Public Events]

        #endregion [Public]

        #region [Protected]

        /// <summary>
        /// Raises the <see cref="E:ApplicationStateChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="ApplicationStateChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnApplicationStateChanged(ApplicationStateChangedEventArgs args)
        {
            if (this.ApplicationStateChanged != null)
            {
                this.ApplicationStateChanged(this, args);
            }
        }

        #region [Application Control]

        /// <summary>
        /// Called before <see cref="OnStart"/>.
        /// </summary>
        /// <returns>true to indicate <see cref="OnStart"/> should be called, otherwise false to cancel the call to <see cref="OnStart"/></returns>
        protected virtual bool OnBeforeStart()
        {
            return true;
        }

        /// <summary>
        /// Called to start up the application.
        /// </summary>
        /// <remarks>
        /// Derived application classes must override this method to start the application.
        /// </remarks>
        protected abstract void OnStart();

        /// <summary>
        /// Called after <see cref="OnStart"/> has completed.
        /// </summary>
        protected virtual void OnAfterStart()
        {
        }

        /// <summary>
        /// Called before <see cref="OnStop"/>.
        /// </summary>
        /// <returns>true to indicate <see cref="OnStop"/> should be called, otherwise false to cancel the call to <see cref="OnStop"/></returns>
        protected virtual bool OnBeforeStop()
        {
            return true;
        }

        /// <summary>
        /// Called to stop the application.
        /// </summary>
        /// <remarks>
        /// Derived application classes must override this method to stop the application.
        /// </remarks>
        protected abstract void OnStop();

        /// <summary>
        /// Called after <see cref="OnStop"/> has completed.
        /// </summary>
        protected virtual void OnAfterStop()
        {
        }

        #endregion [Protected]

        #endregion [Application Control]

        #region [Dispose]

        private bool _disposed;

        /// <summary>
        /// Releases all resources used by the <see cref="MesApplicationControlBase"/> object.
        /// </summary>
        /// <remarks>
        /// This explicitly releases all resources and suppresses finalization. 
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);// see Code Analysis CA1816
        }

        /// <summary>
        /// Releases all resources used by the <see cref="MesApplicationControlBase"/> object.
        /// </summary>
        /// <param name="disposing">True if disposing, else false to release unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                this._disposed = true;
            }
        }

        /// <summary>
        /// Finalizes the <see cref="MesApplicationControlBase"/>
        /// </summary>
        /// <remarks>
        /// This destructor will run only if the Dispose method 
        /// does not get called.
        /// 
        /// It gives this base class the opportunity to finalize.
        /// </remarks>
        ~ApplicationControlBase()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion [Dispose]
    }
}
