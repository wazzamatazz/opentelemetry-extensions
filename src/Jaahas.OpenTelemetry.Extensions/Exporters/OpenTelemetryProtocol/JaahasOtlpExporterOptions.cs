using OpenTelemetry.Exporter;

namespace Jaahas.OpenTelemetry.Exporters.OpenTelemetryProtocol {

    /// <summary>
    /// Options for configuring an OpenTelemetry protocol (OTLP) exporter.
    /// </summary>
    public class JaahasOtlpExporterOptions {

        /// <summary>
        /// Specifies whether the OTLP exporter is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The OTLP exporter protocol to use.
        /// </summary>
        public OtlpExportProtocol Protocol { get; set; } = OtlpExportProtocol.HttpProtobuf;

        /// <summary>
        /// The endpoint that the exporter should send data to.
        /// </summary>
        /// <remarks>
        ///   If <see cref="Endpoint"/> is <see langword="null"/> a default endpoint will be 
        ///   inferred from the configured <see cref="Protocol"/>.
        /// </remarks>
        public Uri? Endpoint { get; set; }

        /// <summary>
        /// The type of signals to send to the OTLP endpoint.
        /// </summary>
        public OtlpExporterSignalKind Signals { get; set; } = OtlpExporterSignalKind.Traces;

        /// <summary>
        /// Optional headers to include in requests to the OTLP endpoint.
        /// </summary>
        public IDictionary<string, string>? Headers { get; set; }

        /// <summary>
        /// The timeout to use when sending data to the OTLP endpoint.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// When <see cref="Protocol"/> is <see cref="OtlpExportProtocol.HttpProtobuf"/>, setting 
        /// this property to <see langword="true"/> will append the standard path for the signal 
        /// type to the <see cref="Endpoint"/> when exporting data if it is not already present.
        /// </summary>
        /// <remarks>
        /// 
        /// <para>
        ///   The standard path for each signal type is:
        /// </para>
        /// 
        /// <list type="table">
        ///   <item>
        ///     <term>traces</term>
        ///     <description><c>/v1/traces</c></description>
        ///   </item>
        ///   <item>
        ///     <term>logs</term>
        ///     <description><c>/v1/logs</c></description>
        ///   </item>
        ///   <item>
        ///     <term>metrics</term>
        ///     <description><c>/v1/metrics</c></description>
        ///   </item>
        /// </list>
        /// 
        /// </remarks>
        public bool AppendSignalPathToEndpoint { get; set; } = true;

    }
}
