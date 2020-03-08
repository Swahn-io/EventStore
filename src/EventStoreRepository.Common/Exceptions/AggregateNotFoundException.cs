using System;

namespace EventStoreRepository.Common.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(string streamName, Type type) : 
            base($"Stream: {streamName} not found for aggregate: {type.Name}.")
        {
        }
    }
}