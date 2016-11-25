using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Part Host Manager
    /// </summary>
    public static class PartHostManager
    {
        private static List<PartActivationHostBase> _hosts = new List<PartActivationHostBase>();

        /// <summary>
        /// Creates an instance of the specified type in an activation host, creating a new host if none is available.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="configFileBaseName">Name of the config file base.</param>
        /// <param name="contractName">Optional name of the contract.</param>
        /// <returns>An activated object instance.</returns>
        internal static object CreateInstance(System.Type type, string configFileBaseName, string contractName = "")
        {
            IPartActivationHost host = CreateActivationHost(type, configFileBaseName);

            object part = null;
            try
            {
                part = host.CreateInstance(type, contractName);
                host.ActivatedTypes.Add(type);                
            }
            catch(Exception exception)
            {
                throw new ActivationException(
                    String.Format(CultureInfo.InvariantCulture, "Unable to activate instance of {0}.", type.Name),
                    exception);
            }

            return part;
        }

        private static IPartActivationHost CreateActivationHost(Type type, string configFileBaseName)
        {
            // use two variables to satisfy CA disposable object exception path....
            PartActivationHostBase selectedHost = null;
            PartActivationHostBase tempSelectedHost = null;

            try
            {

                lock (_hosts)
                {
                    // find an appropriate existing host if any...
                    var query = from host in _hosts
                                let workDirectory = Path.GetDirectoryName(type.Assembly.Location)
                                where host.ActivatedTypes.Contains(type) || host.Description.HostWorkingDirectory == workDirectory
                                select host;

                    tempSelectedHost = query.FirstOrDefault();

                    // if a host was found, return it...
                    if (tempSelectedHost != null)
                    {
                        selectedHost     = tempSelectedHost;
                        tempSelectedHost = null;
                        return selectedHost;
                    }

                    // if no host found, create a new one...

                    string location = Path.GetDirectoryName(type.Assembly.Location);
                    var description = new ActivationHostDescription(location, configFileBaseName);

                    tempSelectedHost = new PartAppDomainHost(description);

                    _hosts.Add(tempSelectedHost);
                    
                }

                // start the host...
                tempSelectedHost.Start();

                selectedHost = tempSelectedHost;
                tempSelectedHost = null;
            }
            finally
            {
                if (tempSelectedHost != null)
                    tempSelectedHost.Dispose();
            }

            return selectedHost;
        }

        /// <summary>
        /// Releases the instance.
        /// </summary>
        /// <param name="disposableValue">The disposable value.</param>
        internal static void ReleaseInstance(object disposableValue)
        {
            IDisposable disposableInstance = disposableValue as IDisposable;
            if (disposableInstance != null)
                disposableInstance.Dispose();

        }        

        /// <summary>
        /// Called when an exception occurs in the isolated part.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="exception">The exception.</param>
        internal static void OnFaulted(PartActivationHostBase host, Exception exception)
        {
            if (Faulted != null)
            {
                Faulted(host, new ActivationHostEventArgs(host.Description, exception));
            }
        }

        /// <summary>
        /// Occurs when the host has faulted.
        /// </summary>
        public static event EventHandler<ActivationHostEventArgs> Faulted;
    }
}
