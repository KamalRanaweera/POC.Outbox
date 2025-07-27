using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Shared.Interfaces
{
    public interface IMessageProcessor
    {
        void ProcessMessages();
        Task ProcessMessagesAsync();
        Task ProcessMessageByIdAsync(Guid messageId);
    }
}
