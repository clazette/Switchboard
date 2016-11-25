using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Isolating Composable Part
    /// </summary>
    public class IsolatingComposablePart : ComposablePart
    {
        private readonly IDictionary<ExportDefinition, object> _values;
        private readonly ComposablePartDefinition _definition;
        private readonly IIsolationMetadata _isolationMetadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatingComposablePart"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="isolationMetadata">The isolation metadata.</param>
        public IsolatingComposablePart(ComposablePartDefinition definition, IIsolationMetadata isolationMetadata)
        {
            this._definition        = definition;
            this._values            = new Dictionary<ExportDefinition, object>();
            this._isolationMetadata = isolationMetadata;
        }

        /// <summary>
        /// Gets the exported object described by the specified <see cref="T:System.ComponentModel.Composition.Primitives.ExportDefinition"/> object.
        /// </summary>
        /// <param name="definition">One of the <see cref="T:System.ComponentModel.Composition.Primitives.ExportDefinition"/> objects from the <see cref="P:System.ComponentModel.Composition.Primitives.ComposablePart.ExportDefinitions"/> property that describes the exported object to return.</param>
        /// <returns>
        /// The exported object described by <paramref name="definition"/>.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/> object has been disposed of.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="definition"/> is null.</exception>
        /// <exception cref="T:System.ComponentModel.Composition.Primitives.ComposablePartException">An error occurred getting the exported object described by the <see cref="T:System.ComponentModel.Composition.Primitives.ExportDefinition"/>.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="definition"/> did not originate from the <see cref="P:System.ComponentModel.Composition.Primitives.ComposablePart.ExportDefinitions"/> property on the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/>.</exception>
        /// <exception cref="T:System.InvalidOperationException">One or more prerequisite imports, indicated by <see cref="P:System.ComponentModel.Composition.Primitives.ImportDefinition.IsPrerequisite"/>, have not been set.</exception>
        public override object GetExportedValue(ExportDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            if (this._values.ContainsKey(definition))
            {
                return this._values[definition];
            }                        

            Lazy<Type> type = ReflectionModelServices.GetPartType(this._definition);

            string contractName = definition.ContractName;

            object partProxy = PartHostManager.CreateInstance(type.Value, this._isolationMetadata.ConfigFileBaseName, contractName);

            this._values[definition] = partProxy;

            return partProxy;
        }

        /// <summary>
        /// Sets the import described by the specified <see cref="T:System.ComponentModel.Composition.Primitives.ImportDefinition"/> object to be satisfied by the specified exports.
        /// </summary>
        /// <param name="definition">One of the objects from the <see cref="P:System.ComponentModel.Composition.Primitives.ComposablePart.ImportDefinitions"/> property that specifies the import to be set.</param>
        /// <param name="exports">A collection of <see cref="T:System.ComponentModel.Composition.Primitives.Export"/> objects of which to set the import described by <paramref name="definition"/>.</param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/> object has been disposed of.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="definition"/> is null. -or- <paramref name="exports"/> is null.</exception>
        /// <exception cref="T:System.ComponentModel.Composition.Primitives.ComposablePartException">An error occurred setting the import described by the <see cref="T:System.ComponentModel.Composition.Primitives.ImportDefinition"/> object.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="definition"/> did not originate from the <see cref="P:System.ComponentModel.Composition.Primitives.ComposablePart.ImportDefinitions"/> property on the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/>. -or- <paramref name="exports"/> contains an element that is null. -or- <paramref name="exports"/> is empty and <see cref="P:System.ComponentModel.Composition.Primitives.ImportDefinition.Cardinality"/> is <see cref="F:System.ComponentModel.Composition.Primitives.ImportCardinality.ExactlyOne"/>. -or- <paramref name="exports"/> contains more than one element and <see cref="P:System.ComponentModel.Composition.Primitives.ImportDefinition.Cardinality"/> is <see cref="F:System.ComponentModel.Composition.Primitives.ImportCardinality.ZeroOrOne"/> or <see cref="F:System.ComponentModel.Composition.Primitives.ImportCardinality.ExactlyOne"/>.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// 	<see cref="M:System.ComponentModel.Composition.Primitives.ComposablePart.SetImport(System.ComponentModel.Composition.Primitives.ImportDefinition,System.Collections.Generic.IEnumerable{System.ComponentModel.Composition.Primitives.Export})"/> has been previously called and <see cref="P:System.ComponentModel.Composition.Primitives.ImportDefinition.IsRecomposable"/> is false.</exception>
        public override void SetImport(ImportDefinition definition, IEnumerable<Export> exports)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");
            if (exports == null)
                throw new ArgumentNullException("exports");
            
        }

        /// <summary>
        /// Gets a collection of the <see cref="T:System.ComponentModel.Composition.Primitives.ExportDefinition"/> objects that describe the exported objects provided by the part.
        /// </summary>
        /// <value></value>
        /// <returns>A collection of <see cref="T:System.ComponentModel.Composition.Primitives.ExportDefinition"/> objects that describe the exported objects provided by the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/>.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/> object has been disposed of.</exception>
        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get { return this._definition.ExportDefinitions; }
        }

        /// <summary>
        /// Gets a collection of the <see cref="T:System.ComponentModel.Composition.Primitives.ImportDefinition"/> objects that describe the imported objects required by the part.
        /// </summary>
        /// <value></value>
        /// <returns>A collection of <see cref="T:System.ComponentModel.Composition.Primitives.ImportDefinition"/> objects that describe the imported objects required by the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/>.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/> object has been disposed of.</exception>
        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get { return this._definition.ImportDefinitions; }
        }

        /// <summary>
        /// Gets the metadata of the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/> object.
        /// </summary>
        /// <value></value>
        /// <returns>The metadata of the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/> object. The default is an empty, read-only <see cref="T:System.Collections.Generic.IDictionary`2"/> object. </returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePart"/> object has been disposed of.</exception>
        public override IDictionary<string, object> Metadata
        {
            get { return this._definition.Metadata; }
        }

        /// <summary>
        /// Gets the exported values.
        /// </summary>
        /// <value>The exported values.</value>
        protected IEnumerable<object> ExportedValues
        {
            get { return this._values.Values; }
        }
    }
}
