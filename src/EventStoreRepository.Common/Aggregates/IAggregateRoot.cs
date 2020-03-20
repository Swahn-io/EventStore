using EventStore.ClientAPI;
using EventStoreRepository.Common.DomainEvents;

namespace EventStoreRepository.Common.Aggregates
{
    public interface IAggregateRoot<TEntity>
    {
        string Id { get; }
        long OriginalVersion { get; }
        long UncommittedVersion { get; }
        TEntity Aggregate { get; }
        void AddEvent(EventData eventData);
        void AddEvent(IDomainEvent domainDomain, bool isUncommitted = true);
        EventData[] GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
}