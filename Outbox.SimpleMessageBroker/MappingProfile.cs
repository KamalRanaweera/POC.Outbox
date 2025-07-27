using Outbox.Shared;
using Outbox.Shared.Dtos;
using Outbox.SimpleMessageBroker.Dtos;
using Outbox.SimpleMessageBroker.Models;

namespace Outbox.SimpleMessageBroker
{
    public class MappingProfile : MappingProfileBase
    {
        public MappingProfile()
        {
            CreateMap<MessageConsumerDto, MessageConsumer>().ReverseMap();
        }
    }
}
