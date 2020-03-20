using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStoreRepository.Common.Aggregates;
using EventStoreRepository.Common.Exceptions;
using EventStoreRepository.Common.Settings;
using Microsoft.Extensions.Options;

namespace EventStoreRepository.Common
{
    public class EventStoreRepository : IAggregateRepository
    {
        private readonly IAggregateFactory _factory;
        private readonly IEventStoreConnection _connection;
        private readonly EventStoreSettings _settings;

        public EventStoreRepository(IAggregateFactory factory, IEventStoreConnection connection,
            IOptionsMonitor<EventStoreSettings> settings)
        {
            _factory = factory;
            _connection = connection;
            _settings = settings.CurrentValue;
        }

        public async Task SaveAsync<TAggregate>(IAggregateRoot<TAggregate> aggregateRoot, string stream)
        {
            await _connection.AppendToStreamAsync(stream, aggregateRoot.OriginalVersion,
                aggregateRoot.GetUncommittedEvents());
            aggregateRoot.ClearUncommittedEvents();
        }

        public async Task<TAggregateRoot> GetByStream<TAggregateRoot, TAggregate>(string stream,
            int maxVersion = Int32.MaxValue)
            where TAggregateRoot : class, IAggregateRoot<TAggregate>
        {
            if (maxVersion < 0) throw new InvalidOperationException("Cannot get max version less than 0.");

            var aggregate = _factory.Create<IAggregateRoot<TAggregate>, TAggregateRoot>();

            var sliceStart = 0;
            StreamEventsSlice currentSlice;
            do
            {
                var sliceCount = sliceStart + _settings.ReadPageSize <= maxVersion
                    ? _settings.ReadPageSize
                    : maxVersion - sliceStart + 1;

                currentSlice =
                    await _connection.ReadStreamEventsForwardAsync(stream, sliceStart, sliceCount, true);

                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                {
                    throw new AggregateNotFoundException(stream, typeof(TAggregateRoot));
                }

                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                {
                    throw new AggregateDeletedException(stream, typeof(TAggregateRoot));
                }

                sliceStart = (int) currentSlice.NextEventNumber;

                foreach (var resolvedEvent in currentSlice.Events)
                {
                    aggregate.AddEvent(new EventData(resolvedEvent.Event.EventId,
                        resolvedEvent.Event.EventType, resolvedEvent.Event.IsJson,
                        resolvedEvent.Event.Data, resolvedEvent.Event.Metadata));
                }
            } while (maxVersion >= currentSlice.NextEventNumber && !currentSlice.IsEndOfStream);

            if (aggregate.OriginalVersion != maxVersion && maxVersion < Int32.MaxValue)
            {
                throw new AggregateVersionException(stream, typeof(TAggregateRoot), aggregate.OriginalVersion,
                    maxVersion);
            }

            return aggregate as TAggregateRoot;
        }
    }
}