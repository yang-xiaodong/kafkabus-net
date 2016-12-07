using KafkaBus.Common;
using KafkaBus.Server;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Builder
{
    public static class HostExtensions
    {
        //TODO: See if it's possible to prevent middleware from being added after RunKafkaBusHost() is called, subsequent calls to RunKafkaBusHost must succeed.

        /// <summary>
        /// Starts running a new KafkaBus host.
        /// </summary>
        /// <param name="app">The Application builder</param>
        /// <param name="subscriber">The KafkaBus subscriber</param>
        /// <remarks>The KafkaBus host will not be started if the application is running a KafkaBus server.</remarks>
        public static void RunKafkaBusHost(this IApplicationBuilder app, IKafkaBusSubscriber subscriber) {
            RunKafkaBusHost(app, subscriber, false);
        }

        /// <summary>
        /// Starts running a new KafkaBus host.
        /// </summary>
        /// <param name="app">The Application builder</param>
        /// <param name="subscriber">The KafkaBus subscriber</param>
        /// <param name="skipKafkaBusServerCheck">Set to true to run the host even if the application server is the KafkaBus.AspNet server</param>
        public static void RunKafkaBusHost(this IApplicationBuilder app, IKafkaBusSubscriber subscriber, bool skipKafkaBusServerCheck) {
            if (app == null) throw new ArgumentNullException("app");
            if (subscriber == null) throw new ArgumentNullException("subscriber");

            if (!skipKafkaBusServerCheck && Server.InstanceCount > 0) {
                //The application is running KafkaBusServer, so exit
                return;
            }
            
            var appFunc = app.Build();

            var _loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var diagnosticSource = app.ApplicationServices.GetRequiredService<DiagnosticSource>();
            var httpContextFactory = app.ApplicationServices.GetRequiredService<IHttpContextFactory>();

            //TODO: Work on counting instances (all hosts + server)  and adding the count to the logger name e.g KafkaBus.AspNet (2), consider including the typename as well.
            var application = new HostingApplication(appFunc, _loggerFactory.CreateLogger(typeof(Server).FullName), diagnosticSource, httpContextFactory);
            var server = app.ApplicationServices.GetRequiredService<Server>();

            server.Start(application, subscriber);
        }
    }
}