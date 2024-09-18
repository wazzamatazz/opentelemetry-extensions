using Microsoft.Extensions.Configuration;

using OpenTelemetry.Exporter;

namespace Jaahas.OpenTelemetry.Exporters.OpenTelemetryProtocol {

    /// <summary>
    /// Utilities for OpenTelemetry configuration.
    /// </summary>
    public static class OtlpExporterConfigurationUtilities {

        /// <summary>
        /// The default configuration section for the OTLP exporter options.
        /// </summary>
        public const string DefaultOtlpExporterConfigurationSection = "OpenTelemetry:Exporters:OTLP";


        /// <summary>
        /// Attempts to bind the specified <see cref="JaahasOtlpExporterOptions"/> to configuration 
        /// values using the provided <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="options">
        ///   The <see cref="JaahasOtlpExporterOptions"/> to bind configuration values to.
        /// </param>
        /// <param name="configuration">
        ///   The <see cref="IConfiguration"/> to bind configuration values from.
        /// </param>
        internal static void Bind(JaahasOtlpExporterOptions options, IConfiguration configuration) {
            configuration.Bind(options);

            var headers = configuration.GetSection("Headers")
                .GetChildren()
                .Select(x => new KeyValuePair<string, string?>(x.Key, x.Value))
                .ToArray();

            if (headers.Length > 0) {
                options.Headers = new Dictionary<string, string>(headers.Length);
                foreach (var header in headers) {
                    options.Headers[header.Key] = header.Value ?? string.Empty;
                }
            }
        }


        /// <summary>
        /// Configures the specified OTLP exporter options.
        /// </summary>
        /// <param name="openTelemetryOptions">
        ///   The OpenTelemetry <see cref="OtlpExporterOptions"/>.
        /// </param>
        /// <param name="exporterOptions">
        ///   The <see cref="JaahasOtlpExporterOptions"/> to configure the <paramref name="openTelemetryOptions"/> with.
        /// </param>
        /// <param name="signalKind">
        ///   The kind of signal to configure the exporter for.
        /// </param>
        internal static void ConfigureOtlpExporterOptions(OtlpExporterOptions openTelemetryOptions, JaahasOtlpExporterOptions exporterOptions, OtlpExporterSignalKind signalKind) {
            openTelemetryOptions.Protocol = exporterOptions.Protocol;

            if (exporterOptions.Endpoint != null && exporterOptions.Endpoint.IsAbsoluteUri) {
                var endpoint = exporterOptions.Endpoint;
                if (openTelemetryOptions.Protocol == OtlpExportProtocol.HttpProtobuf && exporterOptions.AppendSignalPathToEndpoint) {
                    endpoint = AppendPathIfNotPresent(endpoint, GetDefaultSignalPath(signalKind));
                }

                openTelemetryOptions.Endpoint = endpoint;
            }

            if (exporterOptions.Headers != null && exporterOptions.Headers.Count > 0) {
                openTelemetryOptions.Headers = string.Join(",", exporterOptions.Headers.Select(x => $"{x.Key}={x.Value}"));
            }

            if (exporterOptions.Timeout > TimeSpan.Zero) {
                openTelemetryOptions.TimeoutMilliseconds = (int) exporterOptions.Timeout.TotalMilliseconds;
            }

            static string GetDefaultSignalPath(OtlpExporterSignalKind signalKind) {
                return signalKind switch {
                    OtlpExporterSignalKind.Traces => "v1/traces",
                    OtlpExporterSignalKind.Logs => "v1/logs",
                    OtlpExporterSignalKind.Metrics => "v1/metrics",
                    _ => "v1/traces"
                };
            }

            static Uri AppendPathIfNotPresent(Uri uri, string path) {
                var absoluteUri = uri.AbsoluteUri;
                if (absoluteUri.EndsWith("/", StringComparison.Ordinal)) {
                    return absoluteUri.EndsWith($"/{path}/", StringComparison.OrdinalIgnoreCase)
                        ? uri
                        : new Uri($"{absoluteUri}{path}");
                }

                return absoluteUri.EndsWith($"/{path}", StringComparison.OrdinalIgnoreCase)
                    ? uri
                    : new Uri($"{absoluteUri}/{path}");
            }
        }

    }
}
