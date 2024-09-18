namespace Jaahas.OpenTelemetry.Exporters.OpenTelemetryProtocol {

    /// <summary>
    /// Describes the OpenTelemetry signal types to send to an OpenTelemetry protocol (OTLP) 
    /// exporter.
    /// </summary>
    [Flags]
    public enum OtlpExporterSignalKind {

        /// <summary>
        /// No signal type is unspecified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Traces.
        /// </summary>
        Traces = 1,

        /// <summary>
        /// Logs.
        /// </summary>
        Logs = 2,

        /// <summary>
        /// Metrics.
        /// </summary>
        Metrics = 4,

        /// <summary>
        /// Traces, logs, and metrics.
        /// </summary>
        TracesAndLogsAndMetrics = Traces | Logs | Metrics,

        /// <summary>
        /// Traces and logs.
        /// </summary>
        TracesAndLogs = Traces | Logs,

        /// <summary>
        /// Traces and metrics.
        /// </summary>
        TracesAndMetrics = Traces | Metrics,

        /// <summary>
        /// Logs and metrics.
        /// </summary>
        LogsAndMetrics = Logs | Metrics

    }

}
