using System.Reflection;

using Jaahas.OpenTelemetry;

namespace OpenTelemetry.Resources {

    /// <summary>
    /// Extensions for OpenTelemetry resource builders.
    /// </summary>
    public static class JaahasOpenTelemetryResourceBuilderExtensions {

        /// <summary>
        /// Adds service information to a <see cref="ResourceBuilder"/> using the assembly 
        /// returned by <see cref="Assembly.GetEntryAssembly"/> to determine the service name and 
        /// version.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="ResourceBuilder"/>.
        /// </param>
        /// <param name="serviceInstanceId">
        ///   The optional unique identifier for the service instance. If <see langword="null"/> 
        ///   or white space, as service identifier will automatically be generated.
        /// </param>
        /// <returns>
        ///   The <see cref="ResourceBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// 
        /// <para>
        ///   The service name is determined using the following rules:
        /// </para>
        /// 
        /// <list type="number">
        ///   <item>
        ///     If the entry assembly is annotated with an <see cref="OpenTelemetryServiceAttribute"/>, 
        ///     the <see cref="OpenTelemetryServiceAttribute.Name"/> is used as the service name.
        ///   </item>
        ///   <item>
        ///     If <see cref="Assembly.GetName()"/> returns a non-<see langword="null"/> value, the 
        ///     <see cref="AssemblyName.Name"/> property is used as the service name.
        ///   </item>
        ///   <item>
        ///     If neither of the above conditions are met, the service name will be <c>unknown_service</c>.
        ///   </item>
        /// </list>
        /// 
        /// <para>
        ///   The service version is determined using the <see cref="AssemblyName.Version"/> of the 
        ///   entry assembly, formatted as <c>MAJOR.MINOR.PATCH</c>. If an <see cref="AssemblyName"/> 
        ///   is not available for the assembly, the service version will not be set.
        /// </para>
        /// 
        /// </remarks>
        public static ResourceBuilder AddDefaultService(this ResourceBuilder builder, string? serviceInstanceId = null) 
            => builder.AddService(Assembly.GetEntryAssembly()!, serviceInstanceId);


        /// <summary>
        /// Adds service information to a <see cref="ResourceBuilder"/> using the specified 
        /// assembly to determine the service name and version.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="ResourceBuilder"/>.
        /// </param>
        /// <param name="assembly">
        ///   The assembly to use to determine the service name and version.
        /// </param>
        /// <param name="serviceInstanceId">
        ///   The optional unique identifier for the service instance. If <see langword="null"/> 
        ///   or white space, as service identifier will automatically be generated.
        /// </param>
        /// <returns>
        ///   The <see cref="ResourceBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="assembly"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// 
        /// <para>
        ///   The service name is determined using the following rules:
        /// </para>
        /// 
        /// <list type="number">
        ///   <item>
        ///     If the specified <paramref name="assembly"/> is annotated with an <see cref="OpenTelemetryServiceAttribute"/>, 
        ///     the <see cref="OpenTelemetryServiceAttribute.Name"/> is used as the service name.
        ///   </item>
        ///   <item>
        ///     If <see cref="Assembly.GetName()"/> returns a non-<see langword="null"/> value, the 
        ///     <see cref="AssemblyName.Name"/> property is used as the service name.
        ///   </item>
        ///   <item>
        ///     If neither of the above conditions are met, the service name will be <c>unknown_service:{entry_assembly_name}</c> 
        ///     if <see cref="Assembly.GetEntryAssembly"/> has a non-<see langword="null"/> name, 
        ///     or <c>unknown_service</c> otherwise.
        ///   </item>
        /// </list>
        /// 
        /// <para>
        ///   The service version is determined using the <see cref="AssemblyName.Version"/> of the 
        ///   <paramref name="assembly"/>, formatted as <c>MAJOR.MINOR.PATCH</c>. If an <see cref="AssemblyName"/> 
        ///   is not available for the <paramref name="assembly"/>, the service version will not 
        ///   be set.
        /// </para>
        /// 
        /// </remarks>
        public static ResourceBuilder AddService(this ResourceBuilder builder, Assembly assembly, string? serviceInstanceId = null) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(assembly);
#else 
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
            if (assembly == null) {
                throw new ArgumentNullException(nameof(assembly));
            }
#endif
            return builder.AddDetector(new OpenTelemetryServiceAttributeDetector(assembly, serviceInstanceId));
        }


        /// <summary>
        /// Adds service information to a <see cref="ResourceBuilder"/> using the specified type's 
        /// assembly to determine the service name and version.
        /// </summary>
        /// <typeparam name="T">
        ///   The type whose assembly should be used to determine the service name and version.
        /// </typeparam>
        /// <param name="builder">
        ///   The <see cref="ResourceBuilder"/>.
        /// </param>
        /// <param name="serviceInstanceId">
        ///   The optional unique identifier for the service instance. If <see langword="null"/> 
        ///   or white space, as service identifier will automatically be generated.
        /// </param>
        /// <returns>
        ///   The <see cref="ResourceBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// 
        /// <para>
        ///   The service name is determined using the following rules:
        /// </para>
        /// 
        /// <list type="number">
        ///   <item>
        ///     If assembly containing <typeparamref name="T"/> is annotated with an <see cref="OpenTelemetryServiceAttribute"/>, 
        ///     the <see cref="OpenTelemetryServiceAttribute.Name"/> is used as the service name.
        ///   </item>
        ///   <item>
        ///     If <see cref="Assembly.GetName()"/> returns a non-<see langword="null"/> value, the 
        ///     <see cref="AssemblyName.Name"/> property is used as the service name.
        ///   </item>
        ///   <item>
        ///     If neither of the above conditions are met, the service name will be <c>unknown_service:{entry_assembly_name}</c> 
        ///     if <see cref="Assembly.GetEntryAssembly"/> has a non-<see langword="null"/> name, 
        ///     or <c>unknown_service</c> otherwise.
        ///   </item>
        /// </list>
        /// 
        /// <para>
        ///   The service version is determined using the <see cref="AssemblyName.Version"/> of the 
        ///   assembly, formatted as <c>MAJOR.MINOR.PATCH</c>. If an <see cref="AssemblyName"/> is 
        ///   not available for the assembly, the service version will not be set.
        /// </para>
        /// 
        /// </remarks>
        public static ResourceBuilder AddService<T>(this ResourceBuilder builder, string? serviceInstanceId = null)
            => builder.AddService(typeof(T).Assembly, serviceInstanceId);

    }
}
