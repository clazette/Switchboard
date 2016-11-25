using Switchboard.Server;
using System;
using Switchboard.Common.Logger;

using System.Runtime.InteropServices;       // for domain enum
using mscoree;
using System.Collections.Generic;

namespace Switchboard.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var busManager = new MessageBusManager(new ConsoleLogger());

            busManager.Start();

            var domains = GetAppDomains();

            Console.WriteLine("\n");

            foreach (var domain in domains)
            {
                Console.WriteLine(string.Format("Domain {0}: {1}", domain.Id, domain.FriendlyName));
                Console.WriteLine(domain.BaseDirectory.ToString());
                Console.WriteLine("\n");
            }

            Console.ReadLine();
        }

        public static IList<AppDomain> GetAppDomains()
        {
            IList<AppDomain> _IList = new List<AppDomain>();
            IntPtr enumHandle = IntPtr.Zero;
            CorRuntimeHost host = new mscoree.CorRuntimeHost();
            try
            {
                host.EnumDomains(out enumHandle);
                object domain = null;
                while (true)
                {

                    host.NextDomain(enumHandle, out domain);
                    if (domain == null) break;
                    AppDomain appDomain = (AppDomain)domain;
                    _IList.Add(appDomain);
                }
                return _IList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
            finally
            {
                host.CloseEnum(enumHandle);
                Marshal.ReleaseComObject(host);
            }
        }
    }
}
