
namespace Switchboard.Common.Logger
{
    public interface ILogger
    {
        void LogInformation(object logInformation);

        void LogWarning(object logWarning);

        void LogError(object logError);
    }
}
