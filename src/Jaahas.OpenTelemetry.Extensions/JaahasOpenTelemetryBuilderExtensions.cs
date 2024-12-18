using Jaahas.OpenTelemetry.Exporters.OpenTelemetryProtocol;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace OpenTelemetry {

    /// <summary>
    /// Extensions for <see cref="OpenTelemetryBuilder"/>.
    /// </summary>
    public static class JaahasOpenTelemetryBuilderExtensions {

        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that uses the default configuration.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="OpenTelemetryBuilder"/> to configure.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///   
        /// <para>
        ///   The default configuration does not enable the OTLP exporter. You should use an 
        ///   <see cref="Microsoft.Extensions.Options.IConfigureOptions{TOptions}"/>,
        ///   <see cref="Microsoft.Extensions.Options.IConfigureNamedOptions{TOptions}"/> or 
        ///   <see cref="Microsoft.Extensions.Options.IPostConfigureOptions{TOptions}"/> to 
        ///   explicitly enable the exporter when calling this overload.
        /// </para>
        /// 
        /// <para>
        ///   If the OLTP exporter is enabled, the exporter is automatically registered 
        ///   for each signal that it is enabled for (traces, logs, and metrics).
        /// </para>
        /// 
        /// </remarks>
        public static OpenTelemetryBuilder AddOtlpExporter(this OpenTelemetryBuilder builder)
            => builder.AddOtlpExporter(null, _ => { });


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that uses the default configuration.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="OpenTelemetryBuilder"/> to configure.
        /// </param>
        /// <param name="name">
        ///   The name for the OTLP exporter configuration.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///   
        /// <para>
        ///   The default configuration does not enable the OTLP exporter. You should use an 
        ///   <see cref="Microsoft.Extensions.Options.IConfigureOptions{TOptions}"/>,
        ///   <see cref="Microsoft.Extensions.Options.IConfigureNamedOptions{TOptions}"/> or 
        ///   <see cref="Microsoft.Extensions.Options.IPostConfigureOptions{TOptions}"/> to 
        ///   explicitly enable the exporter when calling this overload.
        /// </para>
        /// 
        /// <para>
        ///   If the OLTP exporter is enabled, the exporter is automatically registered 
        ///   for each signal that it is enabled for (traces, logs, and metrics).
        /// </para>
        /// 
        /// </remarks>
        public static OpenTelemetryBuilder AddOtlpExporter(this OpenTelemetryBuilder builder, string? name)
            => builder.AddOtlpExporter(name, _ => { });


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// <paramref name="configuration"/>.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="OpenTelemetryBuilder"/> to configure.
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
        ///   The updated <see cref="OpenTelemetryBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///   
        /// <para>
        ///   The <paramref name="configuration"/> is used to configure an instance of 
        ///   <see cref="JaahasOtlpExporterOptions"/>. By default, the <see cref="OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection"/> 
        ///   configuration section is used. An alternative configuration section can be specified 
        ///   using the <paramref name="configurationSectionName"/> parameter. Specify <see langword="null"/> 
        ///   or white space to bind directly against the root of the <paramref name="configuration"/>.
        /// </para>
        /// 
        /// <para>
        ///   If the OLTP exporter is enabled, the exporter is automatically registered 
        ///   for each signal that it is enabled for (traces, logs, and metrics).
        /// </para>
        /// 
        /// </remarks>
        public static OpenTelemetryBuilder AddOtlpExporter(this OpenTelemetryBuilder builder, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection, Action<JaahasOtlpExporterOptions>? configure = null) 
            => builder.AddOtlpExporter(null, configuration, configurationSectionName, configure);


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// <paramref name="configuration"/>.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="OpenTelemetryBuilder"/> to configure.
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
        ///   The updated <see cref="OpenTelemetryBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///   
        /// <para>
        ///   The <paramref name="configuration"/> is used to configure an instance of 
        ///   <see cref="JaahasOtlpExporterOptions"/>. By default, the <see cref="OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection"/> 
        ///   configuration section is used. An alternative configuration section can be specified 
        ///   using the <paramref name="configurationSectionName"/> parameter. Specify <see langword="null"/> 
        ///   or white space to bind directly against the root of the <paramref name="configuration"/>.
        /// </para>
        /// 
        /// <para>
        ///   If the OLTP exporter is enabled, the exporter is automatically registered 
        ///   for each signal that it is enabled for (traces, logs, and metrics).
        /// </para>
        /// 
        /// </remarks>
        public static OpenTelemetryBuilder AddOtlpExporter(this OpenTelemetryBuilder builder, string? name, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection, Action<JaahasOtlpExporterOptions>? configure = null) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configuration);
#else
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }
#endif

            var configurationSection = string.IsNullOrWhiteSpace(configurationSectionName)
                ? configuration
                : configuration.GetSection(configurationSectionName!);

            return builder.AddOtlpExporter(name, options => {
                OtlpExporterConfigurationUtilities.Bind(options, configurationSection);
                configure?.Invoke(options);
            });
        }


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// delegate.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="OpenTelemetryBuilder"/> to configure.
        /// </param>
        /// <param name="configure">
        ///   The delegate used to configure the OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///
        /// <para>
        ///   If the OLTP exporter is enabled, the exporter is automatically registered
        ///   for each signal that it is enabled for (traces, logs, and metrics).
        /// </para>
        /// 
        /// </remarks>
        public static OpenTelemetryBuilder AddOtlpExporter(this OpenTelemetryBuilder builder, Action<JaahasOtlpExporterOptions> configure) 
            => builder.AddOtlpExporter(null, configure);


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// delegate.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="OpenTelemetryBuilder"/> to configure.
        /// </param>
        /// <param name="name">
        ///   The name for the OTLP exporter configuration.
        /// </param>
        /// <param name="configure">
        ///   The delegate used to configure the OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///
        /// <para>
        ///   If the OLTP exporter is enabled, the exporter is automatically registered
        ///   for each signal that it is enabled for (traces, logs, and metrics).
        /// </para>
        /// 
        /// </remarks>
        public static OpenTelemetryBuilder AddOtlpExporter(this OpenTelemetryBuilder builder, string? name, Action<JaahasOtlpExporterOptions> configure) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configure);
#else
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
            if (configure == null) {
                throw new ArgumentNullException(nameof(configure));
            }
#endif

            var exporterOptions = new JaahasOtlpExporterOptions();
            configure.Invoke(exporterOptions);

            return builder.AddOtlpExporter(name, exporterOptions);
        }



        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// options.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="OpenTelemetryBuilder"/> to configure.
        /// </param>
        /// <param name="options">
        ///   The OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///
        /// <para>
        ///   If the OLTP exporter is enabled, the exporter is automatically registered 
        ///   for each signal that it is enabled for (traces, logs, and metrics).
        /// </para>
        /// 
        /// </remarks>
        public static OpenTelemetryBuilder AddOtlpExporter(this OpenTelemetryBuilder builder, JaahasOtlpExporterOptions options) 
            => builder.AddOtlpExporter(null, options);


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// options.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="OpenTelemetryBuilder"/> to configure.
        /// </param>
        /// <param name="name">
        ///   The name for the OTLP exporter configuration.
        /// </param>
        /// <param name="options">
        ///   The OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="OpenTelemetryBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///
        /// <para>
        ///   If the OLTP exporter is enabled, the exporter is automatically registered 
        ///   for each signal that it is enabled for (traces, logs, and metrics).
        /// </para>
        /// 
        /// </remarks>
        public static OpenTelemetryBuilder AddOtlpExporter(this OpenTelemetryBuilder builder, string? name, JaahasOtlpExporterOptions options) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(options);
#else
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }
#endif

            if (!options.Enabled) {
                return builder;
            }

            if (options.Signals.HasFlag(OtlpExporterSignalKind.Traces)) {
                builder.WithTracing(builder => builder.AddOtlpExporter(name, options));
            }
            if (options.Signals.HasFlag(OtlpExporterSignalKind.Metrics)) {
                builder.WithMetrics(builder => builder.AddOtlpExporter(name, options));
            }
            if (options.Signals.HasFlag(OtlpExporterSignalKind.Logs)) {
                builder.WithLogging(configureBuilder: null, configureOptions: builder => {
                    builder.IncludeScopes = true;
                    builder.AddOtlpExporter(name, options);
                });
            }

            return builder;
        }

    }
}
