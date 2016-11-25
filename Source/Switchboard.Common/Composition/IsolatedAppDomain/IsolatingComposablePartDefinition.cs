using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Isolated composable part definition
    /// </summary>
    public class IsolatingComposablePartDefinition : ComposablePartDefinition
    {
        private readonly ComposablePartDefinition _sourcePart;
        private readonly IIsolationMetadata _isolationMetadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatingComposablePartDefinition"/> class.
        /// </summary>
        /// <param name="sourcePart">The source part.</param>
        /// <param name="isolationMetadata">The isolation metadata.</param>
        public IsolatingComposablePartDefinition(ComposablePartDefinition sourcePart, IIsolationMetadata isolationMetadata)
        {
            if (sourcePart == null)
            {
                throw new ArgumentNullException("sourcePart");
            }

            this._sourcePart        = sourcePart;
            this._isolationMetadata = isolationMetadata;
        }

        /// <summary>
        /// Creates a new instance of a part that the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/> describes.
        /// </summary>
        /// <returns>The created part.</returns>
        public override ComposablePart CreatePart()
        {
            ComposablePart part = null;
            ComposablePart tempPart = null;

            try
            {
                tempPart = ReflectionModelServices.IsDisposalRequired(this._sourcePart)
                               ? new DisposableIsolatingComposablePart(this._sourcePart, this._isolationMetadata)
                               : new IsolatingComposablePart(this._sourcePart, this._isolationMetadata);

                part = tempPart;
                tempPart = null;
            }
            finally
            {
                if (tempPart != null)
                {
                    var disposablePart = (tempPart as IDisposable);
                    if (disposablePart != null)
                        disposablePart.Dispose();
                }
            }

            return part;
        }

        /// <summary>
        /// Gets a collection of <see cref="T:System.ComponentModel.Composition.Primitives.ExportDefinition"/> objects that describe the objects exported by the part defined by this <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/> object.
        /// </summary>
        /// <value></value>
        /// <returns>A collection of <see cref="T:System.ComponentModel.Composition.Primitives.ExportDefinition"/> objects that describe the exported objects provided by <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/> objects created by the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/>.</returns>
        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get { return this._sourcePart.ExportDefinitions; }
        }

        /// <summary>
        /// Gets a collection of <see cref="T:System.ComponentModel.Composition.Primitives.ImportDefinition"/> objects that describe the imports required by the part defined by this <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/> object.
        /// </summary>
        /// <value></value>
        /// <returns>A collection of <see cref="T:System.ComponentModel.Composition.Primitives.ImportDefinition"/> objects that describe the imports required by <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/> objects created by the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/>.</returns>
        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get { return this._sourcePart.ImportDefinitions; }
        }

        /// <summary>
        /// Gets a collection of the metadata for this <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/> object.
        /// </summary>
        /// <value></value>
        /// <returns>A collection that contains the metadata for the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/>. The default is an empty, read-only <see cref="T:System.Collections.Generic.IDictionary`2"/> object.</returns>
        public override IDictionary<string, object> Metadata
        {
            get { return this._sourcePart.Metadata; }
        }
    }
}
