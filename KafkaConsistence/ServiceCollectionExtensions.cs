using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaConsistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaConsistence(this IServiceCollection services) {

            return services;
        }
    }
}
