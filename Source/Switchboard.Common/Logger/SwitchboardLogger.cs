using System.Collections.Generic;
using System.Threading.Tasks;

namespace Switchboard.Common.Logger
{
    public class SwitchboardLogger : ILogger
    {
        private List<ILogger> _loggers = new List<ILogger>();

        public SwitchboardLogger(ILogger logger)
        {
            this._loggers.Add(logger);
        }

        public SwitchboardLogger(List<ILogger> loggers)
        {
            this._loggers = loggers;
        }

        public void LogInformation(object logInformation)
        {
            Parallel.ForEach<ILogger>(this._loggers, logger => logger.LogInformation(logInformation));
        }

        public void LogWarning(object logWarning)
        {
            Parallel.ForEach<ILogger>(this._loggers, logger => logger.LogInformation(logWarning));
        }

        public void LogError(object logError)
        {
            Parallel.ForEach<ILogger>(this._loggers, logger => logger.LogInformation(logError));
        }
    }
}
