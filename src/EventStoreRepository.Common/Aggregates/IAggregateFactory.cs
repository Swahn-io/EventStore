namespace EventStoreRepository.Common.Aggregates
{
    public interface IAggregateFactory
    {
        TAggregateInterface Create<TAggregateInterface, TAggregateImplementation>()
            where TAggregateImplementation : TAggregateInterface;
    }
}