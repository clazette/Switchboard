using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Switchboard.Common.Composition
{
    /// <summary>
    /// A <see cref="ComposablePartCatalog"/> that catches and logs <see cref="ReflectionTypeLoadException"/> objects 
    /// when thrown, but does rethrow them.
    /// </summary>
    /// <remarks>
    /// Often, an assembly with exported parts will have dependencies on other assemblies that are not required for the 
    /// use of the exported parts. However, MEF requires that all dependencies referenced be loaded. If a dependency cannot be
    /// found, MEF throws an <see cref="ReflectionTypeLoadException"/>. By swallowing and logging these exceptions, the
    /// <see cref="SafeDirectoryCatalog"/> allows developers building importable extensions to avoid having to distribute 
    /// unused assemblies.
    /// </remarks>
    public class SafeDirectoryCatalog : ComposablePartCatalog
    {
        private AggregateCatalog _catalog;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="directory">The file directory in which files will be searched for exportable parts.</param>
        /// <param name="searchFilter">A filename mask to limit the files searched in the directory.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose is overridden to dispose of AssemblyCatalog and AggregateCatalog instances.")]
        public SafeDirectoryCatalog(string directory, string searchFilter = "")
        {
            searchFilter = string.Format(CultureInfo.InvariantCulture, "*{0}.dll", searchFilter);

            var files = Directory.EnumerateFiles(directory, searchFilter, SearchOption.AllDirectories);

            this._catalog = new AggregateCatalog();

            foreach (var file in files)
            {
                try
                {
                    var assemblyCatalog = new AssemblyCatalog(file);

                    // Force MEF to load exported parts from the assembly if there are any.
                    if (assemblyCatalog.Parts.ToList().Count > 0)
                        this._catalog.Catalogs.Add(assemblyCatalog);
                }
                catch (ReflectionTypeLoadException reflectionTypeLoadException)
                {
                    // Exported parts were found in an assembly, but the assembly had dependencies that could not be
                    // located. Swallow the exception, but log it. The original assembly was loaded successfully anyway.
                    
                    // TODO: Add logger
                }
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="SafeDirectoryCatalog"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">Determines whether managed resources are disposed in addition to unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!disposing) return;

                if (this._catalog == null) return;

                foreach (AssemblyCatalog catalog in this._catalog.Catalogs)
                {
                    catalog.Dispose();
                }

                this._catalog.Dispose();
                this._catalog = null;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Returns the definitions of exported parts found, if any.
        /// </summary>
        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return this._catalog.Parts; }
        }
    }
}
