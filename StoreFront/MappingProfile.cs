using AutoMapper;
using Outbox.Shared.Dtos;
using StoreFront.Models;

namespace StoreFront
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();
        }
    }
}
