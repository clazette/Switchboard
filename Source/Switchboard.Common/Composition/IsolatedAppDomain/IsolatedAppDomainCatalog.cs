using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{ 

    /// <summary>
    /// Represents a catalog which can instantiate parts in isolation. Because of the isolation,
    /// part has no other dependencies injected, and is created using default constructor.
    /// </summary>
    public class IsolatedAppDomainCatalog : ComposablePartCatalog
    {
        private readonly object _syncRoot = new object();
        private readonly ComposablePartCatalog _interceptedCatalog;
        private volatile IQueryable<ComposablePartDefinition> _innerPartsQueryable;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedAppDomainCatalog"/> class.
        /// </summary>
        /// <param name="interceptedCatalog">Source catalog.</param>
        public IsolatedAppDomainCatalog(ComposablePartCatalog interceptedCatalog)
        {
            if (interceptedCatalog == null)
            {
                throw new ArgumentNullException("interceptedCatalog");
            }

            this._interceptedCatalog = interceptedCatalog;
        }

        /// <summary>
        /// Gets the part definitions that are contained in the catalog.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/> contained in the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartCatalog"/>.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartCatalog"/> object has been disposed of.</exception>
        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return GetParts(); }
        }

        #region [Private Methods]

        private IQueryable<ComposablePartDefinition> GetParts()
        {
            if (this._innerPartsQueryable == null)
            {
                lock (this._syncRoot)
                {
                    if (this._innerPartsQueryable == null)
                    {
                        IEnumerable<ComposablePartDefinition> parts =
                            new List<ComposablePartDefinition>(this._interceptedCatalog.Parts);

                        this._innerPartsQueryable = parts.Select(GetPart).AsQueryable();
                    }
                }
            }

            return _innerPartsQueryable;
        }

        /// <summary>
        /// Gets the part.
        /// </summary>
        /// <param name="original">The original <see cref="ComposablePartDefinition"/>.</param>
        private static ComposablePartDefinition GetPart(ComposablePartDefinition original)
        {
            foreach (var exportDefinition in original.ExportDefinitions)
            {
                IIsolationMetadata isolationMetadata;

                // only create an IsolatingComposablePartDefinition if it is required.
                if (RequiresIsolation(exportDefinition.Metadata, out isolationMetadata))
                {
                    return new IsolatingComposablePartDefinition(original, isolationMetadata);
                }
            }

            return original;
        }

        /// <summary>
        /// Checks the metadata to see if isolation is required.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="isolationMetadata">The isolation metadata.</param>
        /// <returns>
        /// 	<c>true</c> if isolation is required, otherwise <c>false</c>
        /// </returns>
        private static bool RequiresIsolation(IDictionary<string, object> metadata, out IIsolationMetadata isolationMetadata)
        {
            if (metadata.ContainsKey("Isolation"))
            {
                isolationMetadata = AttributedModelServices.GetMetadataView<IIsolationMetadata>(metadata);

                if (isolationMetadata != null && isolationMetadata.Isolation != IsolationLevel.None)
                {
                    return true;
                }
            }

            isolationMetadata = null;

            return false;
        } 

        #endregion [Private Methods]
    }
}