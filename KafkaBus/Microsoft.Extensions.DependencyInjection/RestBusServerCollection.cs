using KafkaBus.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KafkaBusServerCollection
    {
        public static IServiceCollection AddKafkaBusServer(this IServiceCollection services, IConfiguration configuration = null) {
            var information = new ServerInformation(configuration);
            return AddKafkaBusServer(services, information);
        }

        public static IServiceCollection AddKafkaBusServer(this IServiceCollection services, IServerInformation information) {
            var serverFeatures = new KafkaBusFeatureCollection();
            serverFeatures.Set<IServerInformation>(information);
            serverFeatures.Set<IServerAddressesFeature>(information);
            services.AddSingleton(serverFeatures);
            services.AddTransient<Server>();
            return services;
        }
    }
}