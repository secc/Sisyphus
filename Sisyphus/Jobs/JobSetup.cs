using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Sisyphus.Jobs
{
    public static class JobSetup
    {
        public static async void InitJobs()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();

            // get a scheduler
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<KeepAlive>()
                .WithIdentity( "keepAliveJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity( "keepAliveTriger" )
                .WithSimpleSchedule( x => x
                     .WithIntervalInSeconds( 60 )
                     .RepeatForever() )
            .Build();

            await scheduler.ScheduleJob( job, trigger );

        }
    }
}
