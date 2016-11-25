using System;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    /// <summary>
    /// Represents a description of an activation host.
    /// </summary>
    [Serializable]
    public class ActivationHostDescription : IEquatable<ActivationHostDescription>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationHostDescription"/> class.
        /// </summary>
        /// <param name="hostWorkingDirectory">The host working directory.</param>
        /// <param name="configFileBaseName">Name of the config file base.</param>
        public ActivationHostDescription(string hostWorkingDirectory, string configFileBaseName)
        {
            this.Id                        = Guid.NewGuid();
            this.HostWorkingDirectory      = hostWorkingDirectory;
            this.ConfigFileBaseName        = configFileBaseName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationHostDescription"/> class.
        /// </summary>
        public ActivationHostDescription() : this("", "")
        {
        }

        /// <summary>
        /// Id of the activation host.
        /// </summary>
        public Guid Id { get; private set; }
       

        /// <summary>
        /// Gets or sets the host working directory.
        /// </summary>
        /// <value>The host working directory.</value>
        public string HostWorkingDirectory { get; set; }

        /// <summary>
        /// Gets the config file base name.
        /// </summary>
        public string ConfigFileBaseName { get; set; }

        #region Equality Implementation

        /// <summary>
        /// Determines whether the specified <see cref="ActivationHostDescription"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The other <see cref="ActivationHostDescription"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="ActivationHostDescription"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ActivationHostDescription other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other.Id.Equals(Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(ActivationHostDescription))
                return false;
            return Equals((ActivationHostDescription)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ActivationHostDescription left, ActivationHostDescription right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ActivationHostDescription left, ActivationHostDescription right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
