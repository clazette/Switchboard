using Bench.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwitchboardIntegration.Tests
{
    [TestClass]
    public static class BenchTestController
    {
        #region [ Private Fields ]

        private static IntegrationTestController _integrationTestController = new IntegrationTestController();
        
        #endregion

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            _integrationTestController.Initialize();
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            _integrationTestController.Cleanup();
        }

        public static T GetTestController<T>() where T : IntegrationTestControllerBase
        {
            return _integrationTestController.GetController<T>();
        }        
    }
}
