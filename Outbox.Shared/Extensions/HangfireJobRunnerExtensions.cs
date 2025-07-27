using Hangfire;
using Microsoft.Extensions.Hosting;
using Outbox.Shared.Interfaces;
using Outbox.Shared.Strategy.Abstractions;
using Outbox.Shared.Strategy.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Shared.Extensions
{
    public static class HangfireJobRunnerExtensions
    {
        public static void ScheduleHangfireRecurrentJobRunner(this IHost host, string jobName, string cronTiming)
        {
            //// Configure recurrent Hangfire job to process event messages
            RecurringJob.AddOrUpdate<IMessageProcessor>(
                jobName,
                processor => processor.ProcessMessagesAsync(),
                cronTiming
            );
        }
    }
}
