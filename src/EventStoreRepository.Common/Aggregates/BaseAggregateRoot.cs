using System;
using System.Collections.Generic;
using EventStore.ClientAPI;
using EventStoreRepository.Common.DomainEvents;
using EventStoreRepository.Common.Extensions;

namespace EventStoreRepository.Common.Aggregates
{
    public abstract class BaseAggregateRoot<TAggregate> : IAggregateRoot<TAggregate> where TAggregate : new()
    {
        protected byte[] Metadata;
        protected readonly List<IDomainEvent> DomainEvents = new List<IDomainEvent>();
        protected readonly List<EventData> UncommittedEvents = new List<EventData>();
        
        public string Id { get; protected set; }
        public long OriginalVersion { get; private set; }
        public long UncommittedVersion { get; private set; }
        public TAggregate Aggregate { get; } = new TAggregate();

        protected BaseAggregateRoot()
        {
            UncommittedVersion = OriginalVersion = -1;
        }

        public void AddEvent(EventData eventData)
        {
            var domainEvent = GetDomainEvent(eventData);
            AddEvent(domainEvent, false);
        }

        public void AddEvent(IDomainEvent domainDomain, bool isUncommitted = true)
        {
            ApplyEvent(domainDomain);
            DomainEvents.Add(domainDomain);
            if (isUncommitted)
            {
                var eventData = new EventData(Guid.NewGuid(), domainDomain.EventType,
                    true, domainDomain.GetBytes(), GetMetaData());
                UncommittedEvents.Add(eventData);
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

        protected abstract void ApplyEvent(IDomainEvent domainDomain);
        protected abstract IDomainEvent GetDomainEvent(EventData eventData);
        protected abstract byte[] GetMetaData();
    }
}