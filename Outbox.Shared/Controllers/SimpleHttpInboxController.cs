using Microsoft.AspNetCore.Mvc;

namespace Outbox.Shared.Controllers
{
    public class SimpleHttpInboxController
    {
        [HttpPost]
        public async Task Post(string eventName, string eventPayload)
        {

        }
    }
}
