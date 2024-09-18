using Jaahas.OpenTelemetry.Exporters.OpenTelemetryProtocol;

using Microsoft.Extensions.Configuration;

namespace OpenTelemetry.Trace {

    /// <summary>
    /// Extensions for OpenTelemetry trace configuration.
    /// </summary>
    public static class JaahasOpenTelemetryTraceExtensions {

        /// <summary>
        /// Adds an OLTP exporter if it is enabled in the provided <paramref name="configuration"/>.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="configuration">
        ///   The <see cref="IConfiguration"/> containing the OTLP exporter configuration.
        /// </param>
        /// <param name="configurationSectionName">
        ///   The configuration section to bind the OTLP exporter configuration from. If 
        ///   <see langword="null"/>, the root <paramref name="configuration"/> is used.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        public static TracerProviderBuilder ConfigureOtlpExporter(this TracerProviderBuilder builder, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configuration);
#else 
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }
#endif

            var options = new JaahasOtlpExporterOptions();

            var configurationSection = string.IsNullOrWhiteSpace(configurationSectionName)
                ? configuration
                : configuration.GetSection(configurationSectionName!);

            OtlpExporterConfigurationUtilities.Bind(options, configurationSection);

            return builder.ConfigureExporter(options);
        }


        /// <summary>
        /// Adds an OLTP exporter if it is enabled in the provided exporter options.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="options">
        ///   The exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        public static TracerProviderBuilder ConfigureExporter(this TracerProviderBuilder builder, JaahasOtlpExporterOptions options) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(options);
#else 
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }
#endif

            if (options.Enabled && options.Signals.HasFlag(OtlpExporterSignalKind.Traces)) {
                builder.AddOtlpExporter(opts => OtlpExporterConfigurationUtilities.ConfigureOtlpExporterOptions(opts, options, OtlpExporterSignalKind.Traces));
            }

            return builder;
        }

    }
}
