using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Outbox.Shared.Dtos;
using Outbox.Shared.Models;

namespace Outbox.Shared.Controllers
{
    public class SimpleHttpInboxController: ControllerBase
    {
        private readonly EventDbContext _db;
        private readonly ILogger _logger;
        public SimpleHttpInboxController(EventDbContext db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventMessageDto messageDto)
        {
            try
            {
                if (_db.EventMessages.Any(message => message.Id == messageDto.Id))
                    return Ok(); // Message is already received previously (ensure idempotency).

                await _db.EventMessages.AddAsync(new EventMessage
                {
                    Id = messageDto.Id,
                    EventName = messageDto.EventName,
                    MessageType = MessageType.Inbox,
                    Payload = messageDto.Payload
                });
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                // We may still encounter database-level exceptions due to attempts to insert duplicate entries,
                // despite checks to ensure no existing records are present. These raise conditions are  more likely 
                // during debugging scenarios, where execution is delayed (e.g., stepping through code).
                // These duplicates occur because Hangfire's recurring job runs in a separate process and may trigger
                // the publishing job concurrently. Such issues are rare in production environments, where execution
                // is continuous and not paused between statements, so these exceptions can generally be safely ignored.
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }

            return Ok();
        }
    }
}
