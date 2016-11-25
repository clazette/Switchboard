using System;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.CodeAnalysis;


namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Disposable isolated composable part
    /// </summary>
    public class DisposableIsolatingComposablePart : IsolatingComposablePart, IDisposable
    {
        private volatile bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableIsolatingComposablePart"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="isolationMetadata">The isolation metadata.</param>
        public DisposableIsolatingComposablePart(ComposablePartDefinition definition, IIsolationMetadata isolationMetadata)
            : base(definition, isolationMetadata)
        {
        }
       

        #region [IDisposable Members]        

        /// <summary>
        /// Releases all resources used by the <see cref="DisposableIsolatingComposablePart"/> object.
        /// </summary>
        /// <remarks>
        /// This explicitly releases all resources and suppresses finalization. 
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification="This is implemented via DisposeInternal")]
        public void Dispose()
        {
            DisposeInternal(true);
            GC.SuppressFinalize(this);// see Code Analysis CA1816
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed
        {
            get
            {
                return this._isDisposed;
            }
        }

        private void DisposeInternal(bool disposing)
        {
            if (!this._isDisposed)
            {
                this.Dispose(disposing);
                this._isDisposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="DisposableIsolatingComposablePart"/> object.
        /// </summary>
        /// <param name="disposing">True if disposing, else false to release unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {       
            foreach (var disposableValue in ExportedValues)
            {
                PartHostManager.ReleaseInstance(disposableValue);
            }
        }

        /// <summary>
        /// Finalizes the <see cref="DisposableIsolatingComposablePart"/>
        /// </summary>
        /// <remarks>
        /// This destructor will run only if the Dispose method 
        /// does not get called.
        /// 
        /// It gives this base class the opportunity to finalize.
        /// Do not provide a destructor in types derived from this class.
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is implemented via DisposeInternal")]
        ~DisposableIsolatingComposablePart()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            DisposeInternal(false);
        }

        #endregion [IDisposable Members]
    }
}
