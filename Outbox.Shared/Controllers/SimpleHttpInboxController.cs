using Microsoft.AspNetCore.Mvc;
using Outbox.Shared.Dtos;

namespace Outbox.Shared.Controllers
{
    public class SimpleHttpInboxController
    {
        [HttpPost]
        public async Task Post([FromBody] EventMessage eventmessage)
        {

        }
    }
}
