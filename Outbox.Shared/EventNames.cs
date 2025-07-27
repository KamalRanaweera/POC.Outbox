using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Shared
{
    public static class EventNames
    {
        public static readonly string OrderPlaced = "OrderPlaced";
        public static readonly string OrderInProgress = "OrderInProgress";
        public static readonly string OrderComplete = "OrderComplete";
    }
}
