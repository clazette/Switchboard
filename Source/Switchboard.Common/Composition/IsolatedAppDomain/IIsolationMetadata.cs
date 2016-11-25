
namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Metadata describing the isolation for a particular part.
    /// </summary>
    public interface IIsolationMetadata
    {
        /// <summary>
        /// Gets the isolation for a part.
        /// </summary>
        IsolationLevel Isolation { get; }

        /// <summary>
        /// Gets the name of the config file base.
        /// </summary>
        /// <value>The name of the config file base.</value>
        string ConfigFileBaseName { get; }
    }
}
