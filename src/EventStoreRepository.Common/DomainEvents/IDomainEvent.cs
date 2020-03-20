namespace EventStoreRepository.Common.DomainEvents
{
    public interface IDomainEvent
    {
        string EventType { get; }
    }
}