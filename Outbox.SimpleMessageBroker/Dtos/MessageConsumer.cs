namespace Outbox.SimpleMessageBroker.Dtos
{
    public class MessageConsumerDto
    {
        public Guid Id { get; set; }
        public string Endpoint {  get; set; } = string.Empty;
    }
}
