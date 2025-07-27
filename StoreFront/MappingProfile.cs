using AutoMapper;
using Outbox.Shared;
using Outbox.Shared.Dtos;
using StoreFront.Models;

namespace StoreFront
{
    public class MappingProfile : MappingProfileBase
    {
        public MappingProfile()
        {
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();
        }
    }
}
