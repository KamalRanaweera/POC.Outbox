using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Outbox.Shared.Controllers;
using Outbox.Shared.Dtos;
using Outbox.Shared.Models;
using Outbox.SimpleMessageBroker.Models;

namespace Outbox.SimpleMessageBroker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishController : SimpleHttpInboxController
    {
        public PublishController(EventDbContext db)
            : base(db)
        {
        }

    }
}
