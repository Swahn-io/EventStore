namespace EventStoreRepository.Common.Settings
{
    public class EventStoreSettings
    {
        public string ConnectionString { get; set; }
        public string AccountEventStream { get; set; }
        public int ReadPageSize { get; set; }
        public string StreamName { get; set; }
    }
}