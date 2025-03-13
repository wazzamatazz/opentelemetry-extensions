using Jaahas.OpenTelemetry.Exporters.OpenTelemetryProtocol;

using Microsoft.Extensions.Configuration;

namespace OpenTelemetry.Logs {

    /// <summary>
    /// Extensions for OpenTelemetry logging configuration.
    /// </summary>
    public static class JaahasOpenTelemetryLogExtensions {

        /// <summary>
        /// Adds an OLTP exporter that is configured using the provided <paramref name="configuration"/>.
        /// </summary>
        /// <param name="loggerOptions">
        ///   The <see cref="OpenTelemetryLoggerOptions"/> to configure.
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
        ///   The updated <see cref="OpenTelemetryLoggerOptions"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="loggerOptions"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        public static OpenTelemetryLoggerOptions AddOtlpExporter(this OpenTelemetryLoggerOptions loggerOptions, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection, Action<JaahasOtlpExporterOptions>? configure = null) 
            => loggerOptions.AddOtlpExporter(null, configuration, configurationSectionName, configure);


        /// <summary>
        /// Adds an OLTP exporter that is configured using the provided <paramref name="configuration"/>.
        /// </summary>
        /// <param name="loggerOptions">
        ///   The <see cref="OpenTelemetryLoggerOptions"/> to configure.
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
        ///   The updated <see cref="OpenTelemetryLoggerOptions"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="loggerOptions"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        public static OpenTelemetryLoggerOptions AddOtlpExporter(this OpenTelemetryLoggerOptions loggerOptions, string? name, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection, Action<JaahasOtlpExporterOptions>? configure = null) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(loggerOptions);
            ArgumentNullException.ThrowIfNull(configuration);
#else 
            if (loggerOptions == null) {
                throw new ArgumentNullException(nameof(loggerOptions));
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

            return loggerOptions.AddOtlpExporter(name, options);
        }


        /// <summary>
        /// Adds an OLTP exporter that is configured using the provided delegate.
        /// </summary>
        /// <param name="loggerOptions">
        ///   The <see cref="OpenTelemetryLoggerOptions"/> to configure.
        /// </param>
        /// <param name="configure">
        ///   The delegate used to configure the OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryLoggerOptions"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="loggerOptions"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public static OpenTelemetryLoggerOptions AddOtlpExporter(this OpenTelemetryLoggerOptions loggerOptions, Action<JaahasOtlpExporterOptions> configure) 
            => loggerOptions.AddOtlpExporter(null, configure);


        /// <summary>
        /// Adds an OLTP exporter that is configured using the provided delegate.
        /// </summary>
        /// <param name="loggerOptions">
        ///   The <see cref="OpenTelemetryLoggerOptions"/> to configure.
        /// </param>
        /// <param name="name">
        ///   The name for the OTLP exporter configuration.
        /// </param>
        /// <param name="configure">
        ///   The delegate used to configure the OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryLoggerOptions"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="loggerOptions"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public static OpenTelemetryLoggerOptions AddOtlpExporter(this OpenTelemetryLoggerOptions loggerOptions, string? name, Action<JaahasOtlpExporterOptions> configure) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(loggerOptions);
            ArgumentNullException.ThrowIfNull(configure);
#else
            if (loggerOptions == null) {
                throw new ArgumentNullException(nameof(loggerOptions));
            }
            if (configure == null) {
                throw new ArgumentNullException(nameof(configure));
            }
#endif

            var options = new JaahasOtlpExporterOptions();
            configure.Invoke(options);

            return loggerOptions.AddOtlpExporter(name, options);
        }


        /// <summary>
        /// Adds an OLTP exporter that is configured using the provided options.
        /// </summary>
        /// <param name="loggerOptions">
        ///   The <see cref="OpenTelemetryLoggerOptions"/> to configure.
        /// </param>
        /// <param name="options">
        ///   The exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryLoggerOptions"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="loggerOptions"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public static OpenTelemetryLoggerOptions AddOtlpExporter(this OpenTelemetryLoggerOptions loggerOptions, JaahasOtlpExporterOptions options) 
            => loggerOptions.AddOtlpExporter(null, options);


        /// <summary>
        /// Adds an OLTP exporter that is configured using the provided options.
        /// </summary>
        /// <param name="loggerOptions">
        ///   The <see cref="OpenTelemetryLoggerOptions"/> to configure.
        /// </param>
        /// <param name="name">
        ///   The name for the OTLP exporter configuration.
        /// </param>
        /// <param name="options">
        ///   The exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryLoggerOptions"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="loggerOptions"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public static OpenTelemetryLoggerOptions AddOtlpExporter(this OpenTelemetryLoggerOptions loggerOptions, string? name, JaahasOtlpExporterOptions options) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(loggerOptions);
            ArgumentNullException.ThrowIfNull(options);
#else
            if (loggerOptions == null) {
                throw new ArgumentNullException(nameof(loggerOptions));
            }
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }
#endif

            if (options.Enabled && options.Signals.HasFlag(OtlpExporterSignalKind.Logs)) {
                loggerOptions.AddOtlpExporter(name, opts => OtlpExporterConfigurationUtilities.ConfigureOtlpExporterOptions(opts, options, OtlpExporterSignalKind.Logs));
            }

            return loggerOptions;
        }

    }
}
