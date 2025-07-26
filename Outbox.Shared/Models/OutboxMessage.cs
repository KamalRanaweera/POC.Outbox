namespace Outbox.Shared
{
    public class OutboxMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EventType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool Processed { get; set; } = false;
        public DateTime? ProcessedAt { get; set; }

    }
}
