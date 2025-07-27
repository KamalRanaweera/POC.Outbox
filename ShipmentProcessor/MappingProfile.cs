using Outbox.Shared;
using Outbox.Shared.Dtos;
using ShipmentProcessor.Dtos;
using ShipmentProcessor.Models;

namespace ShipmentProcessor
{
    public class MappingProfile : MappingProfileBase
    {
        public MappingProfile()
        {
            CreateMap<ShipmentDto, Shipment>().ReverseMap();
        }
    }
}
