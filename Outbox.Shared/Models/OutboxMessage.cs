namespace Outbox.Shared
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool Processed { get; set; } = false;
        public DateTime? ProcessedAt { get; set; }
    }
}
