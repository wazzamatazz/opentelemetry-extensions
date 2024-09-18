using Jaahas.OpenTelemetry.Exporters.OpenTelemetryProtocol;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace OpenTelemetry.Logs {

    /// <summary>
    /// Extensions for OpenTelemetry logging configuration.
    /// </summary>
    public static class JaahasOpenTelemetryLogExtensions {

        /// <summary>
        /// Adds an OLTP exporter if it is enabled in the provided <paramref name="configuration"/>.
        /// </summary>
        /// <param name="options">
        ///   The <see cref="OpenTelemetryLoggerOptions"/> to configure.
        /// </param>
        /// <param name="configuration">
        ///   The <see cref="IConfiguration"/> containing the OTLP exporter configuration.
        /// </param>
        /// <param name="configurationSectionName">
        ///   The configuration section to bind the OTLP exporter configuration from. If 
        ///   <see langword="null"/>, the root <paramref name="configuration"/> is used.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryLoggerOptions"/>.
        /// </returns>
        public static OpenTelemetryLoggerOptions ConfigureOtlpExporter(this OpenTelemetryLoggerOptions options, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(configuration);
#else 
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }
            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }
#endif
            var exporterOptions = new JaahasOtlpExporterOptions();

            var configurationSection = string.IsNullOrWhiteSpace(configurationSectionName) 
                ? configuration 
                : configuration.GetSection(configurationSectionName!);

            OtlpExporterConfigurationUtilities.Bind(exporterOptions, configurationSection);

            return options.ConfigureOtlpExporter(exporterOptions);
        }


        /// <summary>
        /// Adds an OLTP exporter if it is enabled in the provided exporter options.
        /// </summary>
        /// <param name="options">
        ///   The <see cref="OpenTelemetryLoggerOptions"/> to configure.
        /// </param>
        /// <param name="exporterOptions">
        ///   The exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryLoggerOptions"/>.
        /// </returns>
        public static OpenTelemetryLoggerOptions ConfigureOtlpExporter(this OpenTelemetryLoggerOptions loggerOptions, JaahasOtlpExporterOptions exporterOptions) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(loggerOptions);
            ArgumentNullException.ThrowIfNull(exporterOptions);
#else
            if (loggerOptions == null) {
                throw new ArgumentNullException(nameof(loggerOptions));
            }
            if (exporterOptions == null) {
                throw new ArgumentNullException(nameof(exporterOptions));
            }
#endif


            if (exporterOptions.Enabled && exporterOptions.Signals.HasFlag(OtlpExporterSignalKind.Logs)) {
                loggerOptions.AddOtlpExporter(opts => OtlpExporterConfigurationUtilities.ConfigureOtlpExporterOptions(opts, exporterOptions, OtlpExporterSignalKind.Logs));
            }

            return loggerOptions;
        }

    }
}
