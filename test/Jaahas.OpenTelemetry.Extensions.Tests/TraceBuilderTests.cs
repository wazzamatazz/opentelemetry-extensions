using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Jaahas.OpenTelemetry.Extensions.Tests {

    [TestClass]
    public class TraceBuilderTests {

        private static ActivitySource ActivitySource { get; } = new ActivitySource("Jaahas.OpenTelemetry.Extensions.Tests.TraceBuilderTests");


        [TestMethod]
        public void ShouldAddDefaultTagsInline() {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySource.Name)
                .AddDefaultTags(
                    null,
                    new KeyValuePair<string, object?>("unit-tests.tag1", "value1"), 
                    new KeyValuePair<string, object?>("unit-tests.tag2", "value2"))
                .Build();

            using var activity = ActivitySource.StartActivity("test");
            activity?.Dispose();

            Assert.IsNotNull(activity);
            Assert.AreEqual("value1", activity.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag1").Value);
            Assert.AreEqual("value2", activity.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag2").Value);
        }


        [TestMethod]
        public void ShouldAddDefaultTagsFromConfiguration() {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection([
                    new KeyValuePair<string, string?>("OpenTelemetry:Traces:DefaultTags:unit-tests.tag1", "value1"),
                    new KeyValuePair<string, string?>("OpenTelemetry:Traces:DefaultTags:unit-tests.tag2", "value2"),
                ])
                .Build();

            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySource.Name)
                .AddDefaultTags(configuration)
                .Build();

            using var activity = ActivitySource.StartActivity("test");
            activity?.Dispose();

            Assert.IsNotNull(activity);
            Assert.AreEqual("value1", activity.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag1").Value);
            Assert.AreEqual("value2", activity.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag2").Value);
        }


        [TestMethod]
        public void ShouldAddDefaultTagsFromConfigurationWithCustomSection() {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection([
                    new KeyValuePair<string, string?>("MyTags:unit-tests.tag1", "value1"),
                    new KeyValuePair<string, string?>("MyTags:unit-tests.tag2", "value2"),
                ])
                .Build();

            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySource.Name)
                .AddDefaultTags(configuration, configurationSectionName: "MyTags")
                .Build();

            using var activity = ActivitySource.StartActivity("test");
            activity?.Dispose();

            Assert.IsNotNull(activity);
            Assert.AreEqual("value1", activity.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag1").Value);
            Assert.AreEqual("value2", activity.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag2").Value);
        }


        [TestMethod]
        public void ShouldAddDefaultTagsFromRootConfigurationWithCustomSection() {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection([
                    new KeyValuePair<string, string?>("unit-tests.tag1", "value1"),
                    new KeyValuePair<string, string?>("unit-tests.tag2", "value2"),
                ])
                .Build();

            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySource.Name)
                .AddDefaultTags(configuration, configurationSectionName: "")
                .Build();

            using var activity = ActivitySource.StartActivity("test");
            activity?.Dispose();

            Assert.IsNotNull(activity);
            Assert.AreEqual("value1", activity.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag1").Value);
            Assert.AreEqual("value2", activity.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag2").Value);
        }


        [TestMethod]
        public void ShouldNotAddDefaultTagsToChildActivities() {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySource.Name)
                .AddDefaultTags(
                    null,
                    new KeyValuePair<string, object?>("unit-tests.tag1", "value1"),
                    new KeyValuePair<string, object?>("unit-tests.tag2", "value2"))
                .Build();

            using var parentActivity = ActivitySource.StartActivity("test");
            using var childActivity = ActivitySource.StartActivity("test");

            childActivity?.Dispose();
            parentActivity?.Dispose();

            Assert.IsNotNull(childActivity);
            Assert.AreEqual(0, childActivity.Tags.Count());

            Assert.IsNotNull(parentActivity);
            Assert.AreEqual("value1", parentActivity.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag1").Value);
            Assert.AreEqual("value2", parentActivity.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag2").Value);
        }


        [TestMethod]
        public void ShouldOnlyAddDefaultTagsToMatchingActivities() {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySource.Name)
                .AddDefaultTags(
                    x => x.Kind == ActivityKind.Client,
                    new KeyValuePair<string, object?>("unit-tests.tag1", "value1"))
                .AddDefaultTags(
                    x => x.Kind == ActivityKind.Consumer,
                    new KeyValuePair<string, object?>("unit-tests.tag2", "value2"))
                .Build();

            using var activity1 = ActivitySource.StartActivity("test", ActivityKind.Client);
            activity1?.Dispose();

            using var activity2 = ActivitySource.StartActivity("test", ActivityKind.Consumer);
            activity2?.Dispose();
            
            Assert.IsNotNull(activity1);
            Assert.AreEqual("value1", activity1.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag1").Value);
            Assert.IsNull(activity1.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag2").Value);

            Assert.IsNotNull(activity2);
            Assert.AreEqual("value2", activity2.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag2").Value);
            Assert.IsNull(activity2.Tags.FirstOrDefault(x => x.Key == "unit-tests.tag1").Value);
        }

    }

}
