﻿using MultipleDestinationOtlpExporterExample;

using OpenTelemetry;
using OpenTelemetry.Resources;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// Note that both exporters actually export to the same destination in this example, but they
// export different signal types. See appsettings.json for the configuration.
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddDefaultService())
    .AddNamedOtlpExporters(builder.Configuration)
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var host = builder.Build();
host.Run();
