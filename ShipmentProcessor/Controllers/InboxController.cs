using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Outbox.Shared.Controllers;
using Outbox.Shared.Models;

namespace ShipmentProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : SimpleHttpInboxController
    {
        public InboxController(EventDbContext db, ILogger<InboxController> logger)
            : base(db, logger)
        {
        }
    }
}
