namespace Jaahas.OpenTelemetry {

    /// <summary>
    /// Marks an assembly as definint an OpenTelemetry service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class OpenTelemetryServiceAttribute : Attribute {

        /// <summary>
        /// The name of the service.
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Creates a new <see cref="OpenTelemetryServiceAttribute"/> instance.
        /// </summary>
        /// <param name="name">
        ///   The name of the service.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="name"/> is <see langword="null"/> or white space.
        /// </exception>
        public OpenTelemetryServiceAttribute(string name) {
#if NET8_0_OR_GREATER
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
#else
            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException("The service name cannot be null or white space.", nameof(name));
            }
#endif
            Name = name;
        }

    }

}
