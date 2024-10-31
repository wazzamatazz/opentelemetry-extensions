using System.Reflection;

using OpenTelemetry.Resources;

namespace Jaahas.OpenTelemetry.Extensions.Tests {
    [TestClass]
    public class ResourceBuilderTests {

        [TestMethod]
        public void ShouldAddServiceFromEntryAssembly() {
            var resource = ResourceBuilder.CreateEmpty()
                .AddDefaultService()
                .Build();

            Assert.AreEqual(Assembly.GetEntryAssembly()!.GetName().Name, resource.Attributes.FirstOrDefault(x => x.Key == "service.name").Value as string);
        }


        [TestMethod]
        public void ShouldAddServiceFromAnnotatedAssembly() {
            var resource = ResourceBuilder.CreateEmpty()
                .AddService(typeof(ResourceBuilderTest1.Constants).Assembly)
                .Build();

            Assert.AreEqual(ResourceBuilderTest1.Constants.ServiceName, resource.Attributes.FirstOrDefault(x => x.Key == "service.name").Value as string);
        }


        [TestMethod]
        public void ShouldAddServiceFromUnannotatedAssembly() {
            var resource = ResourceBuilder.CreateEmpty()
                .AddService(typeof(ResourceBuilderTest2.Constants).Assembly)
                .Build();

            Assert.AreEqual(typeof(ResourceBuilderTest2.Constants).Assembly.GetName().Name, resource.Attributes.FirstOrDefault(x => x.Key == "service.name").Value as string);
        }

    }
}
