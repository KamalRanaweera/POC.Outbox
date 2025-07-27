using AutoMapper;
using Hangfire.Server;
using Outbox.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Shared
{
    public class MappingProfileBase : Profile
    {
        public MappingProfileBase()
        {
            CreateMap<EventMessageDto, EventMessage>().ReverseMap();
        }
    }
}
