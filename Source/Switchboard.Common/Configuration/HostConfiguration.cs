// Copyright (c) Mark Bostleman. All rights reserved. See License.txt in Solution Items for license information.

using System.Configuration;

namespace Switchboard.Common.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class HostConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public static MessageBusConfiguration MessageBusConfiguration
        {
            get
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                return config.GetSection("MessageBusConfiguration") as MessageBusConfiguration;
            }
        }
    }
}
