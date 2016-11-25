
namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// Simple application states
    /// </summary>
    public enum ApplicationState
    {
#pragma warning disable 1591  // XML comments are unnecessary for each enum member (it couldn't be more intuitive)

        Unloaded,
        Loading,
        Loaded,
        Stopped,
        Starting,
        Running,
        Stopping,
        Unloading

#pragma warning restore 1591
    }
}
