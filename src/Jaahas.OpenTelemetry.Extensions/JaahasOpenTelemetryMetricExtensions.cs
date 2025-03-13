using Jaahas.OpenTelemetry.Exporters.OpenTelemetryProtocol;

using Microsoft.Extensions.Configuration;

namespace OpenTelemetry.Metrics {

    /// <summary>
    /// Extensions for OpenTelemetry metrics configuration.
    /// </summary>
    public static class JaahasOpenTelemetryMetricExtensions {

        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// <paramref name="configuration"/>.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="MeterProviderBuilder"/> to configure.
        /// </param>
        /// <param name="configuration">
        ///   The <see cref="IConfiguration"/> containing the OTLP exporter configuration.
        /// </param>
        /// <param name="configurationSectionName">
        ///   The configuration section to bind the OTLP exporter configuration from. Specify 
        ///   <see langword="null"/> or white space to bind directly against the root of the 
        ///   <paramref name="configuration"/>.
        /// </param>
        /// <param name="configure">
        ///   An optional delegate used to further configure the OTLP exporter options after 
        ///   binding the configuration section.
        /// </param>
        /// <returns>
        ///   The updated <see cref="MeterProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        public static MeterProviderBuilder AddOtlpExporter(this MeterProviderBuilder builder, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection, Action<JaahasOtlpExporterOptions>? configure = null)
            => builder.AddOtlpExporter(null, configuration, configurationSectionName, configure);


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// <paramref name="configuration"/>.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="MeterProviderBuilder"/> to configure.
        /// </param>
        /// <param name="name">
        ///   The name for the OTLP exporter configuration.
        /// </param>
        /// <param name="configuration">
        ///   The <see cref="IConfiguration"/> containing the OTLP exporter configuration.
        /// </param>
        /// <param name="configurationSectionName">
        ///   The configuration section to bind the OTLP exporter configuration from. Specify 
        ///   <see langword="null"/> or white space to bind directly against the root of the 
        ///   <paramref name="configuration"/>.
        /// </param>
        /// <param name="configure">
        ///   An optional delegate used to further configure the OTLP exporter options after 
        ///   binding the configuration section.
        /// </param>
        /// <returns>
        ///   The updated <see cref="MeterProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        public static MeterProviderBuilder AddOtlpExporter(this MeterProviderBuilder builder, string? name, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection, Action<JaahasOtlpExporterOptions>? configure = null) {
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
            configure?.Invoke(options);
            
            return builder.AddOtlpExporter(name, options);
        }


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// delehate.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="MeterProviderBuilder"/> to configure.
        /// </param>
        /// <param name="configure">
        ///   The delegate used to configure the OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="MeterProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public static MeterProviderBuilder AddOtlpExporter(this MeterProviderBuilder builder, Action<JaahasOtlpExporterOptions> configure)
            => builder.AddOtlpExporter(null, configure);


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// delehate.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="MeterProviderBuilder"/> to configure.
        /// </param>
        /// <param name="name">
        ///   The name for the OTLP exporter configuration.
        /// </param>
        /// <param name="configure">
        ///   The delegate used to configure the OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="MeterProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public static MeterProviderBuilder AddOtlpExporter(this MeterProviderBuilder builder, string? name, Action<JaahasOtlpExporterOptions> configure) {
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

            return builder.AddOtlpExporter(name, options);
        }


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// options.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="MeterProviderBuilder"/> to configure.
        /// </param>
        /// <param name="options">
        ///   The exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="MeterProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public static MeterProviderBuilder AddOtlpExporter(this MeterProviderBuilder builder, JaahasOtlpExporterOptions options)
            => builder.AddOtlpExporter(null, options);


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// options.
        /// </summary>
        /// <param name="name">
        ///   The name for the OTLP exporter configuration.
        /// </param>
        /// <param name="builder">
        ///   The <see cref="MeterProviderBuilder"/> to configure.
        /// </param>
        /// <param name="options">
        ///   The exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="MeterProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public static MeterProviderBuilder AddOtlpExporter(this MeterProviderBuilder builder, string? name, JaahasOtlpExporterOptions options) {
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

            if (options.Enabled && options.Signals.HasFlag(OtlpExporterSignalKind.Metrics)) {
                builder.AddOtlpExporter(name, opts => OtlpExporterConfigurationUtilities.ConfigureOtlpExporterOptions(opts, options, OtlpExporterSignalKind.Metrics));
            }

            return builder;
        }

    }
}
