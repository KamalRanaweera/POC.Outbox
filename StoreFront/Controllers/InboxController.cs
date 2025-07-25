using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Outbox.Shared.Controllers;

namespace StoreFront.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : DefaultInboxController
    {
    }
}
