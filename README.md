# Jaahas.OpenTelemetry.Extensions

Jaahas.OpenTelemetry.Extensions defines extension methods to simplify the configuration of OpenTelemetry in .NET applications.


# Getting Started

Add the [Jaahas.OpenTelemetry.Extensions](https://www.nuget.org/packages/Jaahas.OpenTelemetry.Extensions) NuGet package to your project.


# Registering Service Metadata via Attributes

You can annotate an assembly with the `[Jaahas.OpenTelemetry.OpenTelemetryService]` attribute to mark it as containing an OpenTelemetry service:

```csharp
[assembly: Jaahas.OpenTelemetry.OpenTelemetryService("my-service")]
```

When you construct your OpenTelemetry `ResourceBuilder`, you can easily register the service with the resource builder as follows:

```csharp
services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyType).Assembly));
```

This will register the service using the name specified in the attribute, and the version of the assembly in `MAJOR.MINOR.PATCH` format. You can also optionally specify a service instance ID:

```csharp
services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyType).Assembly, "some-id"));
```


# Configuring a Multi-Signal OpenTelemetry Protocol (OTLP) Exporter

Jaahas.OpenTelemetry.Extensions makes it easy to configure an OpenTelemetry Protocol (OTLP) exporter via the .NET configuration system or by manually configuring exporter options. The exporter can be configured to export traces, metrics or logs, or any combination of the three:

```csharp
// Assumes that configuration is an instance of your application's 
// IConfiguration.

services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyType).Assembly))
    // TODO: configure trace and metrics instrumentation.
    .AddOtlpExporter(configuration);
```

By default, the `AddOtlpExporter` extension method will bind against a configuration section named `OpenTelemetry:Exporters:OTLP` to configure an instance of [JaahasOtlpExporterOptions](./src/Jaahas.OpenTelemetry.Extensions/Exporters/OpenTelemetryProtocol/JaahasOtlpExporterOptions.cs).

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

Note that the default configuration means that the OTLP exporter is disabled by default and calls to the `AddOtlpExporter` extension method have no effect until it is enabled.

It is also possible to configure the exporter by providing your own `JaahasOtlpExporterOptions` instance, or by providing an `Action<JaahasOtlpExporterOptions>` that can be used to configure the options:

```csharp
// Configure exporter options manually.

var exporterOptions = new JaahasOtlpExporterOptions() {
    Enabled = true,
    Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf,
    Signals = OtlpExporterSignalKind.TracesAndLogs,
};

services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyType).Assembly))
    // TODO: configure trace and metrics instrumentation.
    .AddOtlpExporter(exporterOptions);
```

```csharp
// Configure exporter options manually via callback.

services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyType).Assembly))
    // TODO: configure trace and metrics instrumentation.
    .AddOtlpExporter(exporterOptions => {
        exporterOptions.Enabled = true;
        exporterOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
        exporterOptions.Signals = OtlpExporterSignalKind.TracesAndLogs;
    });
```


## Example Configuration: Seq

[Seq](https://datalust.co/seq) can ingest OTLP traces and logs. To export these signals to a local Seq instance using the HTTP/Protobuf export format, you could use the following configuration:

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


## Example Configuration: Jaeger

[Jaeger](https://www.jaegertracing.io/) can ingest OTLP traces. To export traces to a local Jaeger instance using the HTTP/Protobuf export format, you could use the following configuration:

```json
{
    "OpenTelemetry": {
        "Exporters": {
            "OTLP": {
                "Enabled": true,
                "Protocol": "HttpProtobuf",
                "Signals": "Traces"
            }
        }
    }
}
```

Jaeger's OTLP trace receivers listen on the standard OTLP exporter endpoints i.e. port 4317 for gRPC and port 4318 for HTTP/Protobuf. When the `Endpoint` setting is omitted from the configuration, the exporter will default to the standard port for the export format on `localhost`.



# Building the Solution

The repository uses [Cake](https://cakebuild.net/) for cross-platform build automation. The build script allows for metadata such as a build counter to be specified when called by a continuous integration system such as TeamCity.

A build can be run from the command line using the [build.ps1](/build.ps1) PowerShell script or the [build.sh](/build.sh) Bash script. For documentation about the available build script parameters, see [build.cake](/build.cake).


# Software Bill of Materials

To generate a Software Bill of Materials (SBOM) for the repository in [CycloneDX](https://cyclonedx.org/) XML format, run [build.ps1](./build.ps1) or [build.sh](./build.sh) with the `--target BillOfMaterials` parameter.

The resulting SBOM is written to the `artifacts/bom` folder.
