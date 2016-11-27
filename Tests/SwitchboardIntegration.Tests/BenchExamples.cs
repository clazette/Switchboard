using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bench.Common;
using Bench.Controllers;
using SwitchboardIntegration.Tests.Common;
using System.Collections.Generic;
using System.Threading;

namespace SwitchboardIntegration.Tests
{
    /*
     * Each app.config Bench configuration node has a corresponding Test Controller class that will provide access
     * to specific settings. This allows you to access specific message queue information for eaxample.
     * 
     * Adding keys to the BenchConfigurationSettings class is way to consolidate strings instead of having
     * hard coded values all over in your tests.
     * 
     * Once the test project is started and the BenchTestController initializes everything configured in your app.config,
     * the integration test resources you need will be available. 
     */ 

    [TestClass]
    public class BenchExamples
    {
        [TestMethod]
        public void DemonstrateAccessingAMsmqTestControllerConfigElement()
        {
            MessageQueueConfigurationElement configElement = BenchTestController.GetTestController<MsmqTestController>()
                                                                                .GetConfigurationElement(BenchConfigurationSettings.IntegrationTest1MessageQueueName);

            Assert.AreEqual(@".\private$\IntegrationTestsMsmqUtilityUnitTestQueue1", configElement.Path);
        }

        [TestMethod]
        public void DemonstrateAccessingAProcessTestControllerConfigElement()
        {
            ProcessConfigurationElement configElement = BenchTestController.GetTestController<ProcessTestController>()
                                                                           .GetConfigurationElement(BenchConfigurationSettings.ProcessNameNotepad);

            Assert.AreEqual(@"C:\Windows\System32\notepad.exe", configElement.PathToExe);
            Assert.AreEqual("5000", configElement.StartDelayInMilliseconds);

            Thread.Sleep(5000); //Wait for processes to start so that they can be shutdown during Cleanup();
        }

        [TestMethod]
        public void DemonstrateGettingAListOfTestMessageQueues()
        {
            List<MessageQueueConfigurationElement> messageQueues = BenchTestController.GetTestController<MsmqTestController>().MessageQueues;

            Assert.AreEqual(3, messageQueues.Count);
        }
    }
}
