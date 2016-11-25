using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Switchboard.Common.Composition.IsolatedAppDomain
{
    #region [Non-inherited isolated part]

    /// <summary>
    /// Custom <see cref="ExportAttribute"/> which instantiates the part in isolation.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class IsolatedExportAttribute : ExportAttribute, IIsolationMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedExportAttribute"/> class.
        /// </summary>
        public IsolatedExportAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedExportAttribute"/> class.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        public IsolatedExportAttribute(Type contractType)
            : base(contractType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedExportAttribute"/> class.
        /// </summary>
        /// <param name="contractName">Name of the contract.</param>
        public IsolatedExportAttribute(string contractName)
            : base(contractName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedExportAttribute"/> class.
        /// </summary>
        /// <param name="contractName">The contract name that is used to export the type or member marked with this attribute, or null or an empty string ("") to use the default contract name.</param>
        /// <param name="contractType">The type to export.</param>
        public IsolatedExportAttribute(string contractName, Type contractType)
            : base(contractName, contractType)
        {
        }

        /// <summary>
        /// Gets the isolation for a part.
        /// </summary>
        [DefaultValue(typeof(IsolationLevel), "AppDomain")]
        public IsolationLevel Isolation { get; set; }

        /// <summary>
        /// Gets the config file base name if <see cref="Isolation"/> is not set to <see cref="IsolationLevel.None"/>.
        /// </summary>
        [DefaultValue(typeof(string), "domain")]
        public string ConfigFileBaseName { get; set; }
    }
    
    #endregion [Non-inherited isolated part]

    #region [Inherited isolated part]

    /// <summary>
    /// Custom <see cref="ExportAttribute"/> which instantiates the part in isolation.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class InheritedIsolatedExportAttribute : InheritedExportAttribute, IIsolationMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedIsolatedExportAttribute"/> class.
        /// </summary>
        public InheritedIsolatedExportAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedIsolatedExportAttribute"/> class.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        public InheritedIsolatedExportAttribute(Type contractType)
            : base(contractType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedIsolatedExportAttribute"/> class.
        /// </summary>
        /// <param name="contractName">Name of the contract.</param>
        public InheritedIsolatedExportAttribute(string contractName)
            : base(contractName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedIsolatedExportAttribute"/> class.
        /// </summary>
        /// <param name="contractName">The contract name that is used to export the type or member marked with this attribute, or null or an empty string ("") to use the default contract name.</param>
        /// <param name="contractType">The type to export.</param>
        public InheritedIsolatedExportAttribute(string contractName, Type contractType)
            : base(contractName, contractType)
        {
        }

        /// <summary>
        /// Gets the isolation for a part.
        /// </summary>
        [DefaultValue(typeof(IsolationLevel), "AppDomain")]
        public IsolationLevel Isolation { get; set; }

        /// <summary>
        /// Gets the config file base name if <see cref="Isolation"/> is not set to <see cref="IsolationLevel.None"/>.
        /// </summary>
        [DefaultValue(typeof(string), "domain")]
        public string ConfigFileBaseName { get; set; }
    } 

    #endregion [Inherited isolated part]
}
