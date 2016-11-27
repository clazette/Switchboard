using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchboardIntegration.Tests.Common
{
    internal static class BenchConfigurationSettings
    {
        // Add app.config keys so that hard coded strings in tests aren't needed.

        public static string IntegrationTest1MessageQueueName = "IntegrationTestsMsmqUtilityUnitTestQueue1";
        public static string IntegrationTest2MessageQueueName = "IntegrationTestsMsmqUtilityUnitTestQueue2";
        public static string IntegrationTest3MessageQueueName = "IntegrationTestsMsmqUtilityUnitTestQueue3";

        public static string ProcessNameCmd     = "cmd";
        public static string ProcessNameNotepad = "notepad";
    }
}
