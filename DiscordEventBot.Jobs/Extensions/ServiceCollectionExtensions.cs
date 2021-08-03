using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace DiscordEventBot.Jobs.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

        public static IServiceCollection AddEventBotJobs(this IServiceCollection services) => services
            .AddQuartz(config =>
            {
                config.SchedulerId = "discord-event-bot-scheduler";
                config.SchedulerName = "Discord Event Bot Scheduler";
                config.UseMicrosoftDependencyInjectionJobFactory();
                config.UseSimpleTypeLoader();
                config.UseInMemoryStore();
                config.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });
                config.ScheduleJob<NotifyUpcommingEventJob>(trigger => trigger
                    .WithIdentity("every minute")
                    .WithDailyTimeIntervalSchedule(x => x.WithIntervalInMinutes(1))
                );
            })
            .AddQuartzHostedService()
            ;

        #endregion Public Methods
    }
}