using Hangfire;
using Codity.Services.Interfaces;

namespace Codity.Services.Helpers
{
    public class HangfireScheduler
    {
        public static void ScheduleRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IStatisticService>(nameof(IStatisticService),
               c => c.SendWeeklyStatisticSummary(),
               Cron.Hourly);
        }
    }
}
