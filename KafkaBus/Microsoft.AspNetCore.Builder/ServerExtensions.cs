using KafkaBus.Common;
using KafkaBus.Server;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class ServerExtensions
    {
        /// <summary>
        /// Configures the KafkaBus server to use a specified subscriber
        /// </summary>
        /// <param name="app">The Application builder</param>
        /// <param name="subscriber">The KafkaBus subscriber</param>
        public static void ConfigureKafkaBusServer(this IApplicationBuilder app, IKafkaBusSubscriber subscriber) {
            if (app == null) throw new ArgumentNullException("app");
            if (subscriber == null) throw new ArgumentNullException("subscriber");

            var feature = app.ServerFeatures.Get<IServerInformation>();
            if (feature == null) return; //Application isn't running KafkaBus server so return

            var serverInfo = feature as ServerInformation;
            if (serverInfo != null) {
                serverInfo.Subscriber = subscriber;
            }
        }
    }
}