using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Switchboard.Common.ApplicationHosting;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Remote <see cref="AppDomain"/> activator.
    /// </summary>
    public class AppDomainActivator : RemoteActivator
    {
        /// <summary>
        /// Called after the remote object is created.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The instance.</returns>
        protected override object OnAfterCreate(object instance)
        {
            instance = base.OnAfterCreate(instance);

            return ComposePart(instance);
        }

        /// <summary>
        /// Composes the part.
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <returns>The source object after composition</returns>
        private static object ComposePart(object sourceObject)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            using (var aggregateCatalog = new AggregateCatalog())
            {
                //using (new LoggerExemptionScope(typeof(ConsoleLogger)))
                //{
                    // Use a SafeDirectoryCatalog so that ReflectionTypeLoadExceptions are handled gracefully...                
                    using (var safeDirectoryCatalog = new SafeDirectoryCatalog(baseDirectory))
                    {
                        aggregateCatalog.Catalogs.Add(safeDirectoryCatalog);

                        using (var container = new CompositionContainer(aggregateCatalog))
                        {
                            container.ComposeParts(sourceObject);
                            return sourceObject;
                        }
                    }
                //}
            }
        }
    }
}
