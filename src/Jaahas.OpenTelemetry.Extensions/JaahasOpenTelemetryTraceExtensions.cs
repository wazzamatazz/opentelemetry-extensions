using Jaahas.OpenTelemetry.Exporters.OpenTelemetryProtocol;

using Microsoft.Extensions.Configuration;

namespace OpenTelemetry.Trace {

    /// <summary>
    /// Extensions for OpenTelemetry trace configuration.
    /// </summary>
    public static class JaahasOpenTelemetryTraceExtensions {

        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// <paramref name="configuration"/>.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="configuration">
        ///   The <see cref="IConfiguration"/> containing the OTLP exporter configuration.
        /// </param>
        /// <param name="configurationSectionName">
        ///   The configuration section to bind the OTLP exporter configuration from. Specify 
        ///   <see langword="null"/> or white space to bind directly against the root of the 
        ///   <paramref name="configuration"/>.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        public static TracerProviderBuilder AddOtlpExporter(this TracerProviderBuilder builder, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection) {
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

            return builder.AddOtlpExporter(options);
        }


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// delegate.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="configure">
        ///   The delegate used to configure the OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public static TracerProviderBuilder AddOtlpExporter(this TracerProviderBuilder builder, Action<JaahasOtlpExporterOptions> configure) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configure);
#else 
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
            if (configure == null) {
                throw new ArgumentNullException(nameof(configure));
            }
#endif

            var options = new JaahasOtlpExporterOptions();
            configure.Invoke(options);

            return builder.AddOtlpExporter(options);
        }


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// options.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="options">
        ///   The OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public static TracerProviderBuilder AddOtlpExporter(this TracerProviderBuilder builder, JaahasOtlpExporterOptions options) {
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
