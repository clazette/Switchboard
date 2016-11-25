using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Switchboard.Common.Composition.IsolatedAppDomain;
using Switchboard.Common.Configuration;

namespace Switchboard.Common.Composition
{
    public class PartComposer
    {
        public void BuildContainer(MessageBusInfo messageBusInfo, params object[] attributedParts)
        {
            if (messageBusInfo == null) throw new ArgumentNullException("messageBusInfo");
            if (attributedParts == null) throw new ArgumentNullException("attributedParts");

            try
            {
                using (var aggregateCatalog = new AggregateCatalog())
                {
                    using (var safeDirectoryCatalog = new SafeDirectoryCatalog(messageBusInfo.HandlerPath))
                    {
                        var isolatedCatalog = new IsolatedAppDomainCatalog(safeDirectoryCatalog);

                        aggregateCatalog.Catalogs.Add(isolatedCatalog);

                        using (var container = new CompositionContainer(aggregateCatalog))
                        {
                            container.ComposeParts(attributedParts);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // TODO: log but swallow.
            }
        }
    }
}
