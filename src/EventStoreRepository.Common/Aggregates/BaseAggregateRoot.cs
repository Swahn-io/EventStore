using System;
using System.Collections.Generic;
using EventStore.ClientAPI;

namespace EventStoreRepository.Common.Aggregates
{
    public abstract class BaseAggregateRoot<TAggregate> : IAggregateRoot<TAggregate> where TAggregate : new()
    {
        protected readonly List<EventData> Events = new List<EventData>();
        protected readonly List<EventData> UncommittedEvents = new List<EventData>();
        
        public Guid Id { get; }
        public long OriginalVersion { get; private set; }
        public long UncommittedVersion { get; private set; }
        public TAggregate Aggregate { get; } = new TAggregate();

        protected BaseAggregateRoot(Guid id, int version = -1)
        {
            Id = id;
            UncommittedVersion = OriginalVersion = version;
        }
        
        public void AddEvent(EventData domainEvent, bool isUncommitted = true)
        {
            ApplyEvent(domainEvent);
            Events.Add(domainEvent);
            if (isUncommitted)
            {
                UncommittedEvents.Add(domainEvent);
            }
            else
            {
                OriginalVersion++;
            }
            UncommittedVersion++;
        }

        public EventData[] GetUncommittedEvents()
        {
            return UncommittedEvents.ToArray();
        }

        public void ClearUncommittedEvents()
        {
            UncommittedEvents.Clear();
        }

        protected abstract void ApplyEvent(EventData domainEvent);
    }
}