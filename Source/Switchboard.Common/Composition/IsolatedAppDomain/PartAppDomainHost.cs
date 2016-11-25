using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Switchboard.Common.ApplicationHosting;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Part <see cref="AppDomain"/> host.
    /// </summary>
    public class PartAppDomainHost : PartActivationHostBase
    {
        private readonly AppDomain _appDomain;

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="PartAppDomainHost"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public PartAppDomainHost(ActivationHostDescription description)
            : base(description)
        {
            if (description == null)
                throw new ArgumentNullException("description");

            AppDomainSetup appDomainSetup = new AppDomainSetup();
            appDomainSetup.ApplicationBase = description.HostWorkingDirectory;
            appDomainSetup.PrivateBinPath = description.HostWorkingDirectory;

            string configFilePath = Path.Combine(description.HostWorkingDirectory, description.ConfigFileBaseName);
            configFilePath = Path.ChangeExtension(configFilePath, "config");
            if (!File.Exists(configFilePath))
                configFilePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            appDomainSetup.ConfigurationFile = configFilePath;
            this._appDomain = AppDomain.CreateDomain("Plugin_" + description.Id, AppDomain.CurrentDomain.Evidence, appDomainSetup);

            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
        } 

        #endregion [Constructors]

        #region [Public Methods]

        /// <summary>
        /// Creates an instance of <paramref name="type"/>;
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="contractName">Optional contract name.</param>
        /// <returns>An instance of <paramref name="type"/></returns>
        public override object CreateInstance(Type type, string contractName = "")
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsSubclassOf(typeof(MarshalByRefObject)))
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Type {0} must be derived from MarshalByRefObject.", type.Name), "type");

            Type interfaceType = (String.IsNullOrWhiteSpace(contractName)) ? null :
                                                                             type.GetInterfaces().Where(contractType => contractType.FullName == contractName).FirstOrDefault();

            Type remoteActivatorType = typeof(AppDomainActivator);

            string location = Path.GetDirectoryName(type.Assembly.Location);
            string activatorPath = Path.Combine(location, Path.GetFileName(remoteActivatorType.Assembly.Location));

            AppDomainActivator remoteActivator = (AppDomainActivator)this._appDomain.CreateInstanceFromAndUnwrap(activatorPath, remoteActivatorType.FullName);

            string typeNameWithAsesmbly = GetTypeNameWithAssembly(type);
            string interfaceNameWithAssembly = (interfaceType != null) ? GetTypeNameWithAssembly(interfaceType) : String.Empty;

            return remoteActivator.CreateInstanceFromAndCastTo(typeNameWithAsesmbly, interfaceNameWithAssembly);
        } 

        #endregion [Public Methods]

        #region [Protected Methods]

        /// <summary>
        /// Starts the host.
        /// </summary>
        protected override void OnStart()
        {
            // nothing to do here...
        }

        /// <summary>
        /// Stops the host.
        /// </summary>
        protected override void OnStop()
        {
            AppDomain.Unload(this._appDomain);
        } 

        #endregion [Protected Methods]

        #region [Private Methods]

        private static string GetTypeNameWithAssembly(Type type)
        {
            return ReflectionUtility.GetTypeNameWithAssembly(type);
        } 

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var senderDomain = (AppDomain)sender;
            if (senderDomain.Id == this._appDomain.Id)
            {
                this.MarkFaulted(e.ExceptionObject as Exception);
            }
        } 

        #endregion [Private Methods]
    }
}
