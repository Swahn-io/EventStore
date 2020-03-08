using System;
using System.Linq;
using EventStore.ClientAPI;
using EventStoreRepository.Common.Test.AutoData;
using FluentAssertions;
using Xunit;

namespace EventStoreRepository.Common.Test.Aggregates
{
    public class BaseAggregateRootTests
    {
        public class ConstructorShould
        {
            [Theory, AutoNSubstituteData]
            public void SetId(Guid id)
            {
                var sut = new TestAggregateRoot(id);
                sut.Id.Should().Be(id);
            }
            
            [Theory, AutoNSubstituteData]
            public void SetOriginalVersion(int version)
            {
                var sut = new TestAggregateRoot(Guid.NewGuid(), version);
                sut.OriginalVersion.Should().Be(version);
            }
            
            [Fact]
            public void SetOriginalVersionToDefault()
            {
                var sut = new TestAggregateRoot(Guid.NewGuid());
                sut.OriginalVersion.Should().Be(-1);
            }
            
            [Theory, AutoNSubstituteData]
            public void SetUncommittedVersion(int version)
            {
                var sut = new TestAggregateRoot(Guid.NewGuid(), version);
                sut.UncommittedVersion.Should().Be(version);
            }
            
            [Fact]
            public void SetUncommittedVersionToDefault()
            {
                var sut = new TestAggregateRoot(Guid.NewGuid());
                sut.UncommittedVersion.Should().Be(-1);
            }
            
            [Fact]
            public void InitializeAggregate()
            {
                var sut = new TestAggregateRoot(Guid.NewGuid());
                sut.Aggregate.Should().NotBeNull();
            }
        }
        
        public class AddEventShould
        {
            [Theory, AutoNSubstituteData]
            public void AddEvent(EventData eventData, TestAggregateRoot sut)
            {
                sut.AddEvent(eventData);
                sut.InternalEvents.Last().Should().Be(eventData);
            }

            [Theory, AutoNSubstituteData]
            public void AddUncommittedEvent(EventData eventData, TestAggregateRoot sut)
            {
                sut.AddEvent(eventData);
                sut.InternalUncommittedEvents.Last().Should().Be(eventData);
            }

            [Theory, AutoNSubstituteData]
            public void NotAddUncommittedEvent(EventData eventData, TestAggregateRoot sut)
            {
                sut.AddEvent(eventData, false);
                sut.InternalUncommittedEvents.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void NotIncreaseOriginalVersion_IfUncommittedEvent(EventData eventData, TestAggregateRoot sut)
            {
                var originalVersion = sut.OriginalVersion;
                sut.AddEvent(eventData);
                sut.OriginalVersion.Should().Be(originalVersion);
            }

            [Theory, AutoNSubstituteData]
            public void IncreaseOriginalVersion_IfNotUncommittedEvent(EventData eventData, TestAggregateRoot sut)
            {
                var originalVersion = sut.OriginalVersion;
                sut.AddEvent(eventData, false);
                sut.OriginalVersion.Should().Be(originalVersion + 1);
            }

            [Theory, AutoNSubstituteData]
            public void IncreaseUncommittedVersion_IfUncommittedEvent(EventData eventData, TestAggregateRoot sut)
            {
                var originalVersion = sut.UncommittedVersion;
                sut.AddEvent(eventData);
                sut.AddEvent(eventData);
                sut.AddEvent(eventData);
                sut.UncommittedVersion.Should().Be(originalVersion + 3);
            }

            [Theory, AutoNSubstituteData]
            public void CallApplyEvent(EventData eventData, TestAggregateRoot sut)
            {
                sut.EventApplied = appliedEvent =>
                {
                    appliedEvent.Should().Be(eventData);
                };
                sut.AddEvent(eventData);
                sut.AppliedEvents.Should().Be(1);
            }
        }

        public class ClearUncommittedEventsShould
        {
            [Theory, AutoNSubstituteData]
            public void ClearUncommittedEvents(EventData eventData, TestAggregateRoot sut)
            {
                sut.AddEvent(eventData);
                sut.ClearUncommittedEvents();
                sut.InternalUncommittedEvents.Should().BeEmpty();
            }
        }
    }
}