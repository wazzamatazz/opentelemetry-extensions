# About

Jaahas.OpenTelemetry.Extensions defines extension methods to simplify the configuration of OpenTelemetry in .NET applications.


# Registering Service Metadata via Attributes

You can annotate an assembly with the `[Jaahas.OpenTelemetry.OpenTelemetryService]` attribute to mark it as an OpenTelemetry service:

```csharp
[assembly: Jaahas.OpenTelemetry.OpenTelemetryService("my-service")]
```

When you construct your OpenTelemetry `ResourceBuilder`, you can easily register the service with the resource builder as follows:

```csharp
services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyServiceType).Assembly));
```

This will register the service using the name specified in the attribute, and the version of the assembly in `MAJOR.MINOR.PATCH` format. You can also optionally specify a service instance ID:

```csharp
services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyServiceType).Assembly, "some-id"));
```


# OpenTelemetry Protocol (OTLP) Exporter Configuration

Jaahas.OpenTelemetry.Extensions makes it easy to configure an OpenTelemetry Protocol (OTLP) exporter via the .NET configuration system. The exporter can be configured to export traces, metrics or logs, or any combination of the three:

```csharp
// Assumes that configuration is an instance of your application's 
// IConfiguration.

services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyServiceType).Assembly))
    .WithTracing(builder => {
        // TODO: configure tracing
        builder.ConfigureOtlpExporter(configuration);
    })
    .WithMetrics(builder => {
        // TODO: configure metrics
        builder.ConfigureOtlpExporter(configuration);
    })
    .WithLogging(configureBuilder: null, configureOptions: options => {
        options.IncludeScopes = true;
        options.ConfigureOtlpExporter(configuration);
    });
```

By default, the `ConfigureOtlpExporter` extension method will bind against a configuration section named `OpenTelemetry:Exporters:OTLP` to configure an instance of [JaahasOtlpExporterOptions](./Exporters/OpenTelemetryProtocol/JaahasOtlpExporterOptions.cs).

The default configuration used is as follows:

```jsonc
{
    "OpenTelemetry": {
        "Exporters": {
            "OTLP": {
                // OTLP exporter is disabled by default.
                "Enabled": false,

                // Protocol can be "Grpc" or "HttpProtobuf".
                "Protocol": "Grpc",

                // Endpoint is inferred from the export protocol if not 
                // specified.
                "Endpoint": null,

                // The OpenTelemetry signals to export. Possible values are: 
                //     Traces 
                //     Logs 
                //     Metrics 
                //     TracesAndLogsAndMetrics 
                //     TracesAndLogs 
                //     TracesAndMetrics 
                //     LogsAndMetrics
                "Signals": "Traces",

                // Optional headers to include in export requests (such as API 
                // keys) are specified as a JSON dictionary.
                "Headers": null,

                // Timeout for export requests.
                "Timeout": "00:00:10",

                // When true and the Protocol is "HttpProtobuf", the exporter 
                // ensures that the standard OTLP signal path is appended to 
                // the endpoint when exporting a given signal type. The 
                // standard OTLP signal path is "/v1/traces", "/v1/logs", and 
                // "/v1/metrics" for traces, logs, and metrics, respectively.
                "AppendSignalPathToEndpoint": true
            }
        }
    }
}
```

Note that the default configuration means that the OTLP exporter is disabled by default and calls to the `ConfigureOtlpExporter` extension method have no effect until it is enabled.


## Example Configuration

To export logs and traces to [Seq](https://datalust.co/seq) using the HTTP protobuf export format, you could use the following configuration:

```json
{
    "OpenTelemetry": {
        "Exporters": {
            "OTLP": {
                "Enabled": true,
                "Protocol": "HttpProtobuf",
                "Endpoint": "http://localhost:5341/ingest/otlp",
                "Signals": "TracesAndLogs",
                "Headers": {
                    "X-Seq-ApiKey": "your-api-key"
                }
            }
        }
    }
}
```
