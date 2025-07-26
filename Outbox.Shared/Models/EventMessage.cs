namespace Outbox.Shared
{
    public class EventMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public MessageType MessageType { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool Processed { get; set; } = false;
        public DateTime? ProcessedAt { get; set; } = null;

    }

    public enum MessageType { Inbox = 0, Outbox }
}
