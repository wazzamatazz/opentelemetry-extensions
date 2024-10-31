using OtlpExporterExample;

using OpenTelemetry;
using OpenTelemetry.Resources;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddDefaultService())
    .AddOtlpExporter(builder.Configuration)
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var host = builder.Build();
host.Run();
