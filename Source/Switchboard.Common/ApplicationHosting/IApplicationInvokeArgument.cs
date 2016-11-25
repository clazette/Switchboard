
namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// Defines the interface for an application argument passed in the <see cref="IMesApplication.Invoke"/> method.
    /// </summary>
    public interface IApplicationInvokeArgument
    {
        /// <summary>
        /// Gets the name of the argument.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the value of the argument.
        /// </summary>
        /// <value>The value.</value>
        object Value { get; }
    }
}
