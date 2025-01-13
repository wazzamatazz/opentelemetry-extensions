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

If your entry assembly is annotated with an `[Jaahas.OpenTelemetry.OpenTelemetryService]` attribute you can also use the `AddDefaultService` extension method to simplify registration:

```csharp
services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddDefaultService());
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


## Manually Specifying Options

You can configure the exporter by manually providing a `JaahasOtlpExporterOptions` if preferred:

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


## Configuring Options via Callback

You can also configure the exporter by providing an `Action<JaahasOtlpExporterOptions>` that can be used to configure the options:


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


## Combining Configuration and Callbacks

You can also specify a delegate to configure the options after the configuration section has been bound:

```csharp
// Bind configuration and then update via callback.

services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyType).Assembly))
    // TODO: configure trace and metrics instrumentation.
    .AddOtlpExporter(configuration, configure: exporterOptions => {
        exporterOptions.Enabled = true;
    });
```


## Using `IConfigureOptions<T>`, `IConfigureNamedOptions<T>` or `IPostConfigureOptions<T>`

You can register a custom `IConfigureOptions<JaahasOtlpExporterOptions>`, `IConfigureNamedOptions<JaahasOtlpExporterOptions>` or `IPostConfigureOptions<JaahasOtlpExporterOptions>` as a singleton service to configure or override the exporter options. This is useful when you need to perform more complex configuration that cannot be achieved via the configuration system alone.

```csharp
// Perform configuration via IPostConfigureOptions<T>.

services.AddSingleton<IPostConfigureOptions<JaahasOtlpExporterOptions>, MyPostConfigureOptions>();

services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyType).Assembly))
    // TODO: configure trace and metrics instrumentation.
    .AddOtlpExporter();
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


## Configuring Multiple Exporters

You can also configure multiple named exporters by providing a name to the `AddOtlpExporter` extension method. For example, if you wanted to export logs to Seq and traces to Jaeger:

```csharp
services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyType).Assembly))
    // TODO: configure trace and metrics instrumentation.
    .AddOtlpExporter("seq", 
        configuration, 
        configurationSectionName: "OpenTelemetry:Exporters:OTLP:Seq")
    .AddOtlpExporter("jaeger", 
        configuration, 
        configurationSectionName: "OpenTelemetry:Exporters:OTLP:Jaeger");
```

You could then configure the exporters in your `appsettings.json` file as follows:

```json
{
    "OpenTelemetry": {
        "Exporters": {
            "OTLP": {
                "Seq": {
                    "Enabled": true,
                    "Protocol": "HttpProtobuf",
                    "Endpoint": "http://localhost:5341/ingest/otlp",
                    "Signals": "Logs",
                    "Headers": {
                        "X-Seq-ApiKey": "your-api-key"
                    }
                },
                "Jaeger": {
                    "Enabled": true,
                    "Protocol": "HttpProtobuf",
                    "Signals": "Traces"
                }
            }
        }
    }
}
```

Alternatively, you can also use the `AddNamedOtlpExporters` extension method to automatically configure a named exporter for every child section found in a given configuration section:

```csharp
services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(typeof(MyType).Assembly))
    // TODO: configure trace and metrics instrumentation.
    .AddNamedOtlpExporters(
        configuration, 
        configurationSectionName: "OpenTelemetry:Exporters:OTLP"));
```

This approach is particularly useful when the number and names of the exporters is not known at compile time, as it allows additional exporters to be added solely via a configuration change.


# Adding Default Tags to Root Activities

The `AddDefaultTags` extension method for the `TracerProviderBuilder` type can be used to add a default set of tags to observed activities. This is useful for recording information such as the deployment environment that the trace is being generated by (e.g. `development`, `stage`, `production`):

```csharp
services.AddOpenTelemetry()
    .WithTracing(builder => builder.AddDefaultTags(new KeyValuePair<string, object?>("app.environment", "production")));
```

You can also define default tags by passing an `IConfiguration` to the method:

```csharp
services.AddOpenTelemetry()
    .WithTracing(builder => builder.AddDefaultTags(configuration));
```

By default, tags are added from the `OpenTelemetry:Traces:DefaultTags` section (one per subsection). You can also specify a custom configuration section name:

```csharp
services.AddOpenTelemetry()
    .WithTracing(builder => builder.AddDefaultTags(
        configuration, 
        configurationSectionName: "MySectionName"));
```

The default behaviour is to add default tags to top-level activities only (i.e. activities where `Activity.Parent` is `null`). You can also specify a delegate to determine if default tags should be added to a given activity:

```csharp
services.AddOpenTelemetry()
    .WithTracing(builder => builder.AddDefaultTags(
        activity => activity.Kind == ActivityKind.Server,
        new KeyValuePair<string, object?>("app.environment", "production")));
```

`AddDefaultTags` is additive and can be called multiple times to register multiple sets of default tags. Each set can be regi


# Building the Solution

The repository uses [Cake](https://cakebuild.net/) for cross-platform build automation. The build script allows for metadata such as a build counter to be specified when called by a continuous integration system such as TeamCity.

A build can be run from the command line using the [build.ps1](/build.ps1) PowerShell script or the [build.sh](/build.sh) Bash script. For documentation about the available build script parameters, see [build.cake](/build.cake).


# Software Bill of Materials

To generate a Software Bill of Materials (SBOM) for the repository in [CycloneDX](https://cyclonedx.org/) XML format, run [build.ps1](./build.ps1) or [build.sh](./build.sh) with the `--target BillOfMaterials` parameter.

The resulting SBOM is written to the `artifacts/bom` folder.
