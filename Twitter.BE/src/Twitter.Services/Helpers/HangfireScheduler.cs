using Hangfire;
using Twitter.Services.Interfaces;

namespace Twitter.Services.Helpers
{
    public class HangfireScheduler
    {
        public static void ScheduleRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IStatisticService>(nameof(IStatisticService),
                c => c.SendWeeklyStatisticSummary(),
                "*/5 * * * *");
            //RecurringJob.AddOrUpdate<IStatisticService>(nameof(IStatisticService),
            //   c => c.SendWeeklyStatisticSummary(),
            //   Cron.Weekly);
        }
    }
}
