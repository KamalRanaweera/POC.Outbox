using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Outbox.Shared.Controllers;
using Outbox.Shared.Models;

namespace StoreFront.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : SimpleHttpInboxController
    {
        public InboxController(EventDbContext db)
         : base(db)
        {
        }
    }
}
