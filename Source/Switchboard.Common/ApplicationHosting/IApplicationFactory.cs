
namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// Defines the interface that describes and creates a <see cref="IMesApplication"/>
    /// </summary>
    public interface IMesApplicationFactory
    {
        /// <summary>
        /// Creates the <see cref="IMesApplication"/> given the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns><see cref="IMesApplication"/></returns>
        IApplication Create(IApplicationConfiguration configuration);
    }
}
