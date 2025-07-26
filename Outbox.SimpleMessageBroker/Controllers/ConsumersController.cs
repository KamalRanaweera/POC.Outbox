using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Outbox.SimpleMessageBroker.Models;

namespace Outbox.SimpleMessageBroker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumersController : ControllerBase
    {
        private readonly MessageBrokerDbContext _context;
        public ConsumersController(MessageBrokerDbContext context)
        {
            _context = context;
        }

        [HttpPost("subscribe")]
        public async Task Subscribe([FromBody] string endpoint)
        {
            if(!await _context.Consumers.AnyAsync(c => c.Endpoint == endpoint))
            {
                _context.Consumers.Add(new MessageConsumer() { Id = Guid.NewGuid(), Endpoint = endpoint });
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost("unsubscribe/{id}")]
        public async Task<StatusCodeResult> Unsubscribe(Guid id)
        {
            var entry = await _context.Consumers.FindAsync(id);
            if (entry != null)
            {
                _context.Consumers.Remove(entry);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return NotFound();
        }


        [HttpGet("list")]
        public async Task<List<MessageConsumer>> List()
        {
            return await _context.Consumers.ToListAsync();
        }
    }
}
