using System;
using System.Globalization;

namespace Switchboard.Common.Logger
{
    public class ConsoleLogger : ILogger
    {

        public void LogInformation(object logInformation)
        {
            SendToConsole(logInformation, "Information");
        }

        public void LogWarning(object logWarning)
        {
            SendToConsole(logWarning, "Warning");
        }

        public void LogError(object logError)
        {
            SendToConsole(logError, "Error");
        }

        public void SendToConsole(object message, string key)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0}: {1}", key, message.ToString()));
        }
    }
}
