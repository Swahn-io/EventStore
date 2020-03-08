using System;

namespace EventStoreRepository.Common.Exceptions
{
    public class AggregateVersionException : Exception
    {
        public AggregateVersionException(string streamName, Type type, long actualVersion, int expectedVersion) : 
            base($"Got the wrong version for stream: {streamName} for aggregate: {type.Name}. Expected {expectedVersion} but got {actualVersion}.")
        {
        }
    }
}