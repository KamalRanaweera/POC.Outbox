using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Outbox.Shared.Dtos;
using Outbox.SimpleMessageBroker.Models;

namespace Outbox.SimpleMessageBroker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        [HttpPost]
        public async Task Post([FromBody] EventMessage message)
        {
            Console.WriteLine(message);
        }
    }
}
