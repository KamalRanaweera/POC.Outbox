namespace Outbox.SimpleMessageBroker.Models
{
    public class MessageConsumer
    {
        public Guid Id { get; set; }
        public string Endpoint {  get; set; } = string.Empty;
    }
}
