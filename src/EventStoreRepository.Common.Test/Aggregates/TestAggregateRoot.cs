using System;
using System.Collections.Generic;
using EventStore.ClientAPI;
using EventStoreRepository.Common.Aggregates;

namespace EventStoreRepository.Common.Test.Aggregates
{
    public class TestAggregateRoot : BaseAggregateRoot<TestAggregate>
    {
        public List<EventData> InternalEvents => Events;
        public List<EventData> InternalUncommittedEvents => UncommittedEvents;
        public Action<EventData> EventApplied { get; set; }
        public int AppliedEvents { get; private set; }

        public TestAggregateRoot(Guid id, int version = -1) : base(id, version)
        {
        }

        protected override void ApplyEvent(EventData domainEvent)
        {
            EventApplied.Invoke(domainEvent);
            AppliedEvents++;
        }
    }
}