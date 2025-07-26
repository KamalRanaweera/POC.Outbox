using Microsoft.AspNetCore.Mvc;
using Outbox.Shared.Dtos;
using Outbox.Shared.Models;

namespace Outbox.Shared.Controllers
{
    public class SimpleHttpInboxController: ControllerBase
    {
        private readonly EventDbContext _db;
        public SimpleHttpInboxController(EventDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventMessageDto message)
        {
            if (_db.EventMessages.Any(message => message.Id == message.Id))
                return Ok(); // Message is already received previously (ensure idempotency).

            await _db.EventMessages.AddAsync(new EventMessage
            {
                Id = message.Id,
                EventName = message.EventName,
                MessageType = MessageType.Inbox,
                Payload = message.Payload
            });
            _db.SaveChanges();

            return Ok();
        }
    }
}
