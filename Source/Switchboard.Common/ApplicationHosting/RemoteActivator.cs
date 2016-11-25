using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// Remote <see cref="AppDomain"/> activator.
    /// </summary>
    public class RemoteActivator : MarshalByRefObject
    {
        /// <summary>
        /// Creates the instance from the specified type name and cast to the specified contract type.
        /// </summary>
        /// <param name="implementationTypeName">Name of the implementation type.</param>
        /// <param name="contractTypeName">Name of the contract type.</param>
        /// <returns>An activated object cast to the specified contract type.</returns>
        public object CreateInstanceFromAndCastTo(string implementationTypeName, string contractTypeName)
        {
            Type implementationType = Type.GetType(implementationTypeName);

            object instance = Activator.CreateInstance(implementationType);

            // now if the contractTypeName is specified, cast the object to that interface
            // this ensures that what we return will honor the app domain boundaries and 
            // the shared interface between app domains. e.g. if both domains share an 
            // interface but the remote domain implements that interface with a type
            // not known to the calling domain, then if we didn't cast, the calling domain
            // would throw an exception because it would not have the assembly that contained
            // the implementation. By casting to the specified interface(contract), we 
            // honor what the calling domain says it does know about this remote object, that
            // is it's public contract.
            if (!String.IsNullOrWhiteSpace(contractTypeName))
            {
                Type contractType = Type.GetType(contractTypeName);

                // use reflection to call the helper "Cast" function.
                MethodInfo castMethod = this.GetType().GetMethod("Cast", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).MakeGenericMethod(contractType);
                object castedObject = castMethod.Invoke(null, new object[] { instance });
                instance = castedObject;
            }

            instance = this.OnAfterCreate(instance);

            return instance;
        }

        /// <summary>
        /// Called after the remote object is created.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The instance.</returns>
        protected virtual object OnAfterCreate(object instance)
        {
            return instance;
        }

        /// <summary>
        /// Casts the specified source object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceObject">The source object.</param>
        /// <returns>The object casted to <typeparamref name="T"/></returns>
        /// <remarks>
        /// This is a helper method called via reflection to cast objects to a specified
        /// type.
        /// </remarks>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This is called via reflection.")]
        protected static T Cast<T>(object sourceObject)
        {
            return (T)sourceObject;
        }
    }
}
