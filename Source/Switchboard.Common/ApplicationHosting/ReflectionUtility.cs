using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Switchboard.Common.ApplicationHosting
{
    /// <summary>
    /// Helper methods for reflection
    /// </summary>
    public static class ReflectionUtility
    {
        /// <summary>
        /// Scans the types in the assembly, and gets the objects that implement an interface.
        /// </summary>
        /// <typeparam name="T">The type of object to find.</typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <returns>List of objects that implement the specified interface.</returns>
        public static List<T> GetObjectsForAnInterface<T>(Assembly assembly) where T : class
        {
            if (assembly == null)
            {
                throw new ArgumentException("Cannot get objects for an interface, because the parameter, assembly, is null.", "assembly");
            }

            // Note: objects will not be null if no types are found. An empty list will be returned.
            IEnumerable<T> objects = from t in assembly.GetTypes()
                                     where t.GetInterfaces().Contains(typeof(T))
                                            && t.GetConstructor(Type.EmptyTypes) != null
                                     select Activator.CreateInstance(t) as T;

            return objects.ToList();
        }

        /// <summary>
        /// Scans the types in the assembly, and gets the objects that derive from a base class.
        /// </summary>
        /// <typeparam name="T">The type of base class, to be used to find its derived types.</typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <param name="args">The arguments to pass to the constructors of the types to activate.</param>
        /// <returns>
        /// List of objects that derive from the specified base class.
        /// </returns>
        public static List<T> GetObjectsDerivedFromBase<T>(Assembly assembly, params object[] args) where T : class
        {
            if (assembly == null)
            {
                throw new ArgumentException("Cannot get objects that derive from a base class, because the parameter, assembly, is null.", "assembly");
            }

            // Note: objects will not be null if no types are found. An empty list will be returned.
            var derivedTypes = typeof(T).GetDerivedTypes(assembly);

            IEnumerable<T> objects = from t in derivedTypes
                                     where !t.IsAbstract
                                     select Activator.CreateInstance(t, args) as T;

            return objects.ToList();
        }

        /// <summary>
        /// Gets the derived types.
        /// </summary>
        /// <param name="baseType">The base type.</param>
        /// <param name="assemblies">The assemblies to inspect.</param>
        /// <returns>A sequence of <see cref="Type">types</see> that are derived from <paramref name="baseType"/></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IEnumerable<Type> GetDerivedTypes(this Type baseType, IEnumerable<Assembly> assemblies)
        {
            if (baseType == null)
            {
                throw new ArgumentNullException("baseType");
            }

            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }

            foreach (Assembly assembly in assemblies)
            {
                var types = baseType.GetDerivedTypes(assembly);

                foreach (var derivedType in types)
                    yield return derivedType;
            }
        }

        /// <summary>
        /// Gets the derived types.
        /// </summary>
        /// <param name="baseType">The base type.</param>
        /// <param name="assembly">The assembly to inspect.</param>
        /// <returns>A sequence of <see cref="Type">types</see> that are derived from <paramref name="baseType"/></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IEnumerable<Type> GetDerivedTypes(this Type baseType, Assembly assembly)
        {
            if (baseType == null)
            {
                throw new ArgumentNullException("baseType");
            }

            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            var types = from t in assembly.GetTypes()
                        where t.IsSubclassOf(baseType)
                        select t;

            return types;
        }

        /// <summary>
        /// Looks for the specified <see cref="Attribute"/> on the type.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Attribute"/> to find in the <see cref="Type"/>.</typeparam>
        /// <param name="type"><see cref="Type"/> to scan.</param>
        /// <returns></returns>
        public static TAttribute GetMatchingAttributeFromType<TAttribute>(Type type) where TAttribute : Attribute
        {
            if (type == null) { throw new ArgumentException("type parameter is null."); }

            TAttribute[] attributes = (TAttribute[])type.GetCustomAttributes(typeof(TAttribute), true);

            if (attributes.Length == 0)
                return null;

            return attributes[0];
        }

        /// <summary>
        /// Gets the public properties with their values.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>A <see cref="Dictionary{K,V}"/> of property names and their values of the object.</returns>
        public static Dictionary<string, object> GetPublicProperties(this object target)
        {
            Dictionary<string, object> propertyKeyValues = new Dictionary<string, object>();

            if (target == null)
                return propertyKeyValues;

            var properties = (from property in target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                              select new
                              {
                                  Name = property.Name,
                                  Value = property.GetValue(target, null)
                              });

            foreach (var property in properties)
            {
                propertyKeyValues.Add(property.Name, property.Value);
            }

            return propertyKeyValues;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="padding">The padding.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(this Dictionary<string, object> properties, string padding = "")
        {
            if (properties == default(Dictionary<string, object>))
                throw new ArgumentNullException("properties");

            var builder = new StringBuilder();

            foreach (var property in properties)
            {
                builder
                    .Append(padding)
                    .Append(property.Key)
                    .Append(" = ")
                    .Append(property.Value)
                    .AppendLine();
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the type name with assembly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A type string that includes the full name of the type and the assembly name but excludes the version information.</returns>
        public static string GetTypeNameWithAssembly(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            AssemblyName assemblyName = type.Assembly.GetName();
            return String.Format(CultureInfo.InvariantCulture, "{0}, {1}", type.FullName, assemblyName.Name);
        }

        /// <summary>
        /// Gets the <see cref="Type"/> described by the specified name.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>The <see cref="Type"/> described by the specified name.</returns>
        public static Type GetTypeByName(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            Type type = null;

            assemblies.ForEach(assembly =>
            {
                if (type != null)
                    return;

                Type test = assembly.GetTypes().ToList().FirstOrDefault(t => t.Name == typeName);

                if (test == null)
                    return;

                type = test;
            });

            return type;
        }

    }
}
