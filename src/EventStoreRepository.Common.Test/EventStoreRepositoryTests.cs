using System.Threading.Tasks;
using AutoFixture.Xunit2;
using EventStore.ClientAPI;
using EventStoreRepository.Common.Aggregates;
using EventStoreRepository.Common.Test.Aggregates;
using EventStoreRepository.Common.Test.AutoData;
using NSubstitute;
using Xunit;

namespace EventStoreRepository.Common.Test
{
    public class EventStoreRepositoryTests
    {
        public class SaveAsyncShould
        {
            [Theory, EventStoreRepositoryAutoData]
            public async Task AppendUncommittedEventsToExistingStream([Frozen] IEventStoreConnection connection,
                IAggregateRoot<TestAggregate> aggregateRoot, EventStoreRepository sut)
            {
                await sut.SaveAsync(aggregateRoot);
                await connection.Received(1).AppendToStreamAsync($"{aggregateRoot.GetType().Name}-{aggregateRoot.Id}",
                    aggregateRoot.OriginalVersion, aggregateRoot.GetUncommittedEvents());
            }

            [Theory, EventStoreRepositoryAutoData]
            public async Task AppendUncommittedEventsToNewStream([Frozen] IEventStoreConnection connection,
                IAggregateRoot<TestAggregate> aggregateRoot, EventStoreRepository sut)
            {
                await sut.SaveAsync(aggregateRoot);
                await connection.Received(1).AppendToStreamAsync($"{aggregateRoot.GetType().Name}-{aggregateRoot.Id}",
                    aggregateRoot.OriginalVersion, aggregateRoot.GetUncommittedEvents());
            }

            [Theory, EventStoreRepositoryAutoData]
            public async Task ClearUncommittedEventsFromAggregate(IAggregateRoot<TestAggregate> aggregateRoot,
                EventStoreRepository sut)
            {
                await sut.SaveAsync(aggregateRoot);
                aggregateRoot.Received(1).ClearUncommittedEvents();
            }
        }

    }
}