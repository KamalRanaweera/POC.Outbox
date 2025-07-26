using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Outbox.Shared.Controllers;

namespace ShipmentProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : SimpleHttpInboxController
    {
    }
}
