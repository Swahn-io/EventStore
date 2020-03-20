using System;
using System.Threading.Tasks;
using EventStoreRepository.Common.Aggregates;

namespace EventStoreRepository.Common
{
    public interface IAggregateRepository
    {
        Task SaveAsync<TEntity>(IAggregateRoot<TEntity> aggregateRoot, string stream);
        Task<TAggregate> GetByStream<TAggregate, TEntity>(string stream, int maxVersion = Int32.MaxValue)
            where TAggregate : class, IAggregateRoot<TEntity>;
    }
}