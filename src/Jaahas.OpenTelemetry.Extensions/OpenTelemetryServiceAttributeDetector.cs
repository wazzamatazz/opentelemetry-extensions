using System.Reflection;

using OpenTelemetry.Resources;

namespace Jaahas.OpenTelemetry {

    /// <summary>
    /// <see cref="IResourceDetector"/> that uses an <see cref="OpenTelemetryServiceAttribute"/> 
    /// on an assembly to infer service information.
    /// </summary>
    /// <remarks>
    ///   If the specified assembly is not annotated with an <see cref="OpenTelemetryServiceAttribute"/>, 
    ///   the service name of the resource will be set to the name of the assembly.
    /// </remarks>
    internal class OpenTelemetryServiceAttributeDetector : IResourceDetector {

        /// <summary>
        /// The assembly.
        /// </summary>
        private readonly Assembly _assembly;

        /// <summary>
        /// The service instance identifier.
        /// </summary>
        private readonly string? _serviceInstanceId;


        /// <summary>
        /// Creates a new <see cref="OpenTelemetryServiceAttributeDetector"/> instance.
        /// </summary>
        /// <param name="assembly">
        ///   The assembly to use to determine the service name and version.
        /// </param>
        /// <param name="serviceInstanceId">
        ///   The optional unique identifier for the service instance. A service instance 
        ///   identifier will be automatically generated if this value is <see langword="null"/> 
        ///   or white space.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="assembly"/> is <see langword="null"/>.
        /// </exception>
        public OpenTelemetryServiceAttributeDetector(Assembly assembly, string? serviceInstanceId) {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _serviceInstanceId = string.IsNullOrWhiteSpace(serviceInstanceId)
                ? null
                : serviceInstanceId;
        }


        /// <inheritdoc/>
        public Resource Detect() {
            var attr = _assembly.GetCustomAttribute<OpenTelemetryServiceAttribute>();
            var serviceName = attr?.Name ?? _assembly.GetName()?.Name ?? throw new InvalidOperationException("Unable to infer service name from assembly. The assembly must be annotated with an [OpenTelemetryService] attribute or it must define an assembly name.");

            return ResourceBuilder.CreateDefault()
                .AddService(
                    serviceName,
                    serviceNamespace: attr?.Namespace,
                    serviceVersion: _assembly.GetName()?.Version?.ToString(3),
                    serviceInstanceId: _serviceInstanceId)
                .Build();
        }

    }
}
