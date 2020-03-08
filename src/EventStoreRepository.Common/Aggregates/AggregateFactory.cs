using System;

namespace EventStoreRepository.Common.Aggregates
{
    public class AggregateFactory : IAggregateFactory
    {
        public TAggregateInterface Create<TAggregateInterface, TAggregateImplementation>()
            where TAggregateImplementation : TAggregateInterface
        {
            return Activator.CreateInstance<TAggregateImplementation>();
        }
    }
}