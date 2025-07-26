using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Shared.Dtos
{
    public class EventMessageDto
    {
        public Guid Id { get; set; }
        public string EventType { get; set; } = String.Empty;
        public string Payload { get; set; } = String.Empty;
    }
}
