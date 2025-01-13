using System.Diagnostics;

using Jaahas.OpenTelemetry.Exporters.OpenTelemetryProtocol;
using Jaahas.OpenTelemetry.Trace;

using Microsoft.Extensions.Configuration;

namespace OpenTelemetry.Trace {

    /// <summary>
    /// Extensions for OpenTelemetry trace configuration.
    /// </summary>
    public static class JaahasOpenTelemetryTraceExtensions {

        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// <paramref name="configuration"/>.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="configuration">
        ///   The <see cref="IConfiguration"/> containing the OTLP exporter configuration.
        /// </param>
        /// <param name="configurationSectionName">
        ///   The configuration section to bind the OTLP exporter configuration from. Specify 
        ///   <see langword="null"/> or white space to bind directly against the root of the 
        ///   <paramref name="configuration"/>.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        public static TracerProviderBuilder AddOtlpExporter(this TracerProviderBuilder builder, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection) 
            => builder.AddOtlpExporter(null, configuration, configurationSectionName);


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// <paramref name="configuration"/>.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
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
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        public static TracerProviderBuilder AddOtlpExporter(this TracerProviderBuilder builder, string? name, IConfiguration configuration, string? configurationSectionName = OtlpExporterConfigurationUtilities.DefaultOtlpExporterConfigurationSection) {
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

            var options = new JaahasOtlpExporterOptions();

            var configurationSection = string.IsNullOrWhiteSpace(configurationSectionName)
                ? configuration
                : configuration.GetSection(configurationSectionName!);

            OtlpExporterConfigurationUtilities.Bind(options, configurationSection);

            return builder.AddOtlpExporter(name, options);
        }


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// delegate.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="configure">
        ///   The delegate used to configure the OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public static TracerProviderBuilder AddOtlpExporter(this TracerProviderBuilder builder, Action<JaahasOtlpExporterOptions> configure) 
            => builder.AddOtlpExporter(null, configure);


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// delegate.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="name">
        ///   The name for the OTLP exporter configuration.
        /// </param>
        /// <param name="configure">
        ///   The delegate used to configure the OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public static TracerProviderBuilder AddOtlpExporter(this TracerProviderBuilder builder, string? name, Action<JaahasOtlpExporterOptions> configure) {
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

            var options = new JaahasOtlpExporterOptions();
            configure.Invoke(options);

            return builder.AddOtlpExporter(name, options);
        }


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// options.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="options">
        ///   The OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public static TracerProviderBuilder AddOtlpExporter(this TracerProviderBuilder builder, JaahasOtlpExporterOptions options) 
            => builder.AddOtlpExporter(null, options);


        /// <summary>
        /// Adds an OpenTelemetry Protocol (OTLP) exporter that is configured using the provided 
        /// options.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="name">
        ///   The name for the OTLP exporter configuration.
        /// </param>
        /// <param name="options">
        ///   The OTLP exporter options.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public static TracerProviderBuilder AddOtlpExporter(this TracerProviderBuilder builder, string? name, JaahasOtlpExporterOptions options) {
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

            if (options.Enabled && options.Signals.HasFlag(OtlpExporterSignalKind.Traces)) {
                builder.AddOtlpExporter(name, opts => OtlpExporterConfigurationUtilities.ConfigureOtlpExporterOptions(opts, options, OtlpExporterSignalKind.Traces));
            }

            return builder;
        }


        /// <summary>
        /// Configures default tags that will be added to trace activities.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="tags">
        ///   The default tags to add to trace activities.
        /// </param>
        /// <param name="shouldAdd">
        ///   A delegate that determines if the default tags should be added to a given activity. 
        ///   Specify <see langword="null"/> to add default tags if the activity is a top-level 
        ///   activity only (i.e. <see cref="Activity.Parent"/> is <see langword="null"/>).
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <remarks>
        ///   This method can be called multiple times to add multiple sets of default tags.
        /// </remarks>
        public static TracerProviderBuilder AddDefaultTags(this TracerProviderBuilder builder, TagList tags, Func<Activity, bool>? shouldAdd = null) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(builder);
#else 
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
#endif
            return builder.AddProcessor(new DefaultTagsTraceProcessor(tags, shouldAdd));
        }


        /// <summary>
        /// Configures default tags that will be added trace activities.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="tags">
        ///   The default tags to add to trace activities.
        /// </param>
        /// <param name="shouldAdd">
        ///   A delegate that determines if the default tags should be added to a given activity. 
        ///   Specify <see langword="null"/> to add default tags if the activity is a top-level 
        ///   activity only (i.e. <see cref="Activity.Parent"/> is <see langword="null"/>).
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <remarks>
        ///   This method can be called multiple times to add multiple sets of default tags.
        /// </remarks>
        public static TracerProviderBuilder AddDefaultTags(this TracerProviderBuilder builder, ReadOnlySpan<KeyValuePair<string, object?>> tags, Func<Activity, bool>? shouldAdd = null) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(builder);
#else 
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
#endif
            return builder.AddProcessor(new DefaultTagsTraceProcessor(new TagList(tags), shouldAdd));
        }


        /// <summary>
        /// Configures default tags that will be added to trace activities.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="tags">
        ///   The default tags to add to trace activities.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <remarks>
        ///   This method can be called multiple times to add multiple sets of default tags.
        /// </remarks>
        public static TracerProviderBuilder AddDefaultTags(this TracerProviderBuilder builder, params KeyValuePair<string, object?>[] tags) => builder.AddDefaultTags(null, tags);


        /// <summary>
        /// Configures default tags that will be added to trace activities.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="shouldAdd">
        ///   A delegate that determines if the default tags should be added to the activity. 
        ///   Specify <see langword="null"/> to add default tags if the activity is a top-level 
        ///   activity (i.e. <see cref="Activity.Parent"/> is <see langword="null"/>).
        /// </param>
        /// <param name="tags">
        ///   The default tags to add to trace activities.
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <remarks>
        ///   This method can be called multiple times to add multiple sets of default tags.
        /// </remarks>
        public static TracerProviderBuilder AddDefaultTags(this TracerProviderBuilder builder, Func<Activity, bool>? shouldAdd, params KeyValuePair<string, object?>[] tags) {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(builder);
#else 
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
#endif
            return builder.AddProcessor(new DefaultTagsTraceProcessor(new TagList(tags), shouldAdd));
        }


        /// <summary>
        /// Configures default tags that will be added to activities.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="TracerProviderBuilder"/> to configure.
        /// </param>
        /// <param name="configuration">
        ///   The <see cref="IConfiguration"/> containing the default tags. A tag will be added 
        ///   for each child section.
        /// </param>
        /// <param name="configurationSectionName">
        ///   The name of the configuration section containing the default tags. Specify <see langword="null"/> 
        ///   or white space to bind directly against the root of the <paramref name="configuration"/>.
        /// </param>
        /// <param name="shouldAdd">
        ///   A delegate that determines if the default tags should be added to a given activity. 
        ///   Specify <see langword="null"/> to add default tags if the activity is a top-level 
        ///   activity only (i.e. <see cref="Activity.Parent"/> is <see langword="null"/>).
        /// </param>
        /// <returns>
        ///   The updated <see cref="TracerProviderBuilder"/>.
        /// </returns>
        /// <remarks>
        ///   This method can be called multiple times to add multiple sets of default tags.
        /// </remarks>
        public static TracerProviderBuilder AddDefaultTags(this TracerProviderBuilder builder, IConfiguration configuration, string? configurationSectionName = "OpenTelemetry:Traces:DefaultTags", Func<Activity, bool>? shouldAdd = null) {
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

            var section = string.IsNullOrWhiteSpace(configurationSectionName)
                ? configuration
                : configuration.GetSection(configurationSectionName!);

            var tagList = new TagList(section.GetChildren().Select(x => new KeyValuePair<string, object?>(x.Key, x.Value)).ToArray());

            return builder.AddProcessor(new DefaultTagsTraceProcessor(tagList, shouldAdd));
        }

    }
}
