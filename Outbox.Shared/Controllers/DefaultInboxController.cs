using Microsoft.AspNetCore.Mvc;

namespace Outbox.Shared.Controllers
{
    public class DefaultInboxController
    {
        [HttpPost]
        public async Task Post(string eventName, string eventPayload)
        {

        }
    }
}
