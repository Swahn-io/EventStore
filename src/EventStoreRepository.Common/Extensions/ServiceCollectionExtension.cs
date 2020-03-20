using EventStoreRepository.Common.Aggregates;
using Microsoft.Extensions.DependencyInjection;

namespace EventStoreRepository.Common.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventStoreRepository(this IServiceCollection services)
        {
            services.AddSingleton<IAggregateRepository, EventStoreRepository>();
            services.AddSingleton<IAggregateFactory, AggregateFactory>();
            return services;
        }
    }
}