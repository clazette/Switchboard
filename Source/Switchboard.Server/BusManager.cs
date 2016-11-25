using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Reflection;
using Switchboard.Common.Composition;
using Switchboard.Common.Configuration;
using Switchboard.Common.Logger;
using Switchboard.Server.Handlers;
using Switchboard.Server.Queues;

namespace Switchboard.Server
{
    public class MessageBusManager
    {
        private IQueueManager _queueController;
        private IHandlerManager _handlerManager;
        private PartComposer _partComposer;
        private ILogger _logger;

        public MessageBusManager(ILogger logger)
        {
            this._handlerManager = new HandlerManager();
            this._partComposer = new PartComposer();
            this._logger = logger;
        }

        /// <summary>
        /// Retrieves the <see cref="MessageBusConfiguration"/> section from the host configuration and
        /// starts a <see cref="QueueController"/>.
        /// </summary>
        public void Start()
        {
            this._logger.LogInformation("Starting the bus manager...");

            MessageBusInfo messageBusInfo = HostConfiguration.MessageBusConfiguration.Map();

            if (!this.IsConfigurationValid(messageBusInfo))
            {
                this._logger.LogInformation("Configuration is not valid, stopping bus manager.");
                return;
            }
            else
            {
                this._logger.LogInformation("Configuration is valid.");
            }

            this._partComposer.BuildContainer(messageBusInfo, this._handlerManager);

            if (!this._handlerManager.TryInitializion(this._logger))
            {
                this._logger.LogInformation("The HandlerManager could not be initialized, stopping bus manager.");
                return;
            }

            this._queueController = new QueueManager(this.BuildQueues(messageBusInfo));

            this._queueController.Initialize();

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.ResolveEventHandler);
        }

        private Dictionary<string, MessageQueue> BuildQueues(MessageBusInfo messageBusInfo)
        {
            var queues = new Dictionary<string, MessageQueue>();

            messageBusInfo.QueueInfos.ForEach(queueInfo =>
            {
                var queue = new MessageQueue(queueInfo.Path);
                queues.Add(queueInfo.Key, queue);
            });

            return queues;
        }

        private bool IsConfigurationValid(MessageBusInfo messageBusInfo)
        {
            if (messageBusInfo == null)
            {
                this._logger.LogInformation("Configuration is missing.");
                return false;
            }

            if (messageBusInfo.HandlerPath == null)
            {
                this._logger.LogInformation("Handler path is missing in configuration.");
                return false;
            }

            this._logger.LogInformation(string.Format("{0} {1}", "Using handler path ", messageBusInfo.HandlerPath));

            if (messageBusInfo.QueueInfos.Count == 0 || messageBusInfo.QueueInfos == null)
            {
                this._logger.LogInformation("No queues configured.");
                return false;
            }

            this._logger.LogInformation(string.Format("{0} {1}", messageBusInfo.QueueInfos.Count, "queues configured."));

            messageBusInfo.QueueInfos.ForEach(queueInfo => this._logger.LogInformation(queueInfo.Path));

            return true;
        }

        private Assembly ResolveEventHandler(object sender, ResolveEventArgs resolveArgs)
        {
            // This event handler is used to work around a bug in the .Net configuration process.  When a custom
            // configuration section is referenced in the host process's configuration file, if the assembly that the section is defined in
            // was loaded at runtime from a directory other than the host process's application directory, then when the configuration
            // section is called, .Net configuration will not be able to load the type.  The reason for this is that .Net
            // configuration only looks in assemblies in the host process's application folder and the GAC - not among
            // the assemblies already loaded in the AppDomain.

            // To get around this, we use the AssemblyResolve event which is raised when the AppDomain is trying to load
            // an assembly but can't find it.  If this event handler returns an assembly, the AppDomain will attempt to use it. So for
            // the purpose of the .Net configuration issue, we simply search for the assembly that the AppDomain is looking for from
            // among those already loaded.  If we find it, we return it.

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var assembly = loadedAssemblies.Where(asm => asm.FullName == resolveArgs.Name).FirstOrDefault();

            // In some cases, the requested assembly only has the assembly name in the string and not the version
            // and culture components - this works in those cases. But it might also have unintended consequences
            // since it may not return the correct version.
            if (assembly == null)
            {
                assembly = loadedAssemblies.Where(asm => asm.FullName.Contains(resolveArgs.Name)).FirstOrDefault();
            }

            return assembly;
        }
    }
}

