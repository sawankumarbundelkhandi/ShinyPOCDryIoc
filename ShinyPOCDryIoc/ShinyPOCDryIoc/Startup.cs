using System;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny;

namespace ShinyPOCDryIoc
{
    public class Startup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            services.UseGps<GpsDelegate>();
            services.UseFirebaseMessaging<PushDelegate>();
        }


        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            // This registers and initializes the Container with Prism ensuring
            // that both Shiny & Prism use the same container
            ContainerLocator.SetContainerExtension(() => new DryIocContainerExtension());
            var container = ContainerLocator.Container.GetContainer();
            container.Populate(services);
            return container.GetServiceProvider();
        }
    }
}