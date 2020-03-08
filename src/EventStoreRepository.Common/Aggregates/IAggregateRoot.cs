using System;
using EventStore.ClientAPI;

namespace EventStoreRepository.Common.Aggregates
{
    public interface IAggregateRoot<TEntity>
    {
        Guid Id { get; }
        long OriginalVersion { get; }
        long UncommittedVersion { get; }
        TEntity Aggregate { get; }
        void AddEvent(EventData domainEvent, bool isUncommitted = true);
        EventData[] GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
}