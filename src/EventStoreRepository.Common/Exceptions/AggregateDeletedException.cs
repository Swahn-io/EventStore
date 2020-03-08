using System;

namespace EventStoreRepository.Common.Exceptions
{
    public class AggregateDeletedException : Exception
    {
        public AggregateDeletedException(string streamName, Type type) : 
            base($"Stream: {streamName} for type: {type.Name} has been deleted.")
        {
        }
    }
}