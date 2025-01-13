using System.Diagnostics;

using OpenTelemetry;

namespace Jaahas.OpenTelemetry.Trace {

    /// <summary>
    /// Trace processor that adds default tags to activities.
    /// </summary>
    internal sealed class DefaultTagsTraceProcessor : BaseProcessor<Activity> {

        /// <summary>
        /// The default tags to add to activities.
        /// </summary>
        private readonly TagList _defaultTags;

        /// <summary>
        /// A function that determines if tags should be added to an activity.
        /// </summary>
        private readonly Func<Activity, bool> _shouldAdd;


        /// <summary>
        /// Creates a new <see cref="DefaultTagsTraceProcessor"/> with the specified default tags.
        /// </summary>
        /// <param name="defaultTags">
        ///   The default tags to add to activities.
        /// </param>
        /// <param name="shouldAdd">
        ///   A function that determines if the default tags should be added to an activity. 
        ///   Specify <see langword="null"/> to add tags to all top-level activities.
        /// </param>
        public DefaultTagsTraceProcessor(TagList defaultTags, Func<Activity, bool>? shouldAdd) {
            _defaultTags = defaultTags;
            _shouldAdd = shouldAdd ?? IsRootActivity;
        }


        /// <inheritdoc/>
        public override void OnStart(Activity data) {
            base.OnStart(data);

            if (data == null) {
                return;
            }

            if (!_shouldAdd(data)) {
                return;
            }

            foreach (var tag in _defaultTags) {
                data!.AddTag(tag.Key, tag.Value);
            }
        }


        private static bool IsRootActivity(Activity activity) => activity.Parent == null;

    }
}
