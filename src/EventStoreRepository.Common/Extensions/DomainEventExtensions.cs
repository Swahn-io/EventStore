using System.Text;
using System.Text.Json;
using EventStoreRepository.Common.DomainEvents;

namespace EventStoreRepository.Common.Extensions
{
    public static class DomainEventExtensions
    {
        public static byte[] GetBytes(this IDomainEvent domainEvent)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(domainEvent, domainEvent.GetType()));
        }
        
        public static T GetEventData<T>(this byte[] bytes)
        {
            var jsonString = Encoding.UTF8.GetString(bytes);
            var eventData = JsonSerializer.Deserialize<T>(jsonString);
            return eventData;
        }
    }
}