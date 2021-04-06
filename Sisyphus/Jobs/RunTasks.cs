using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Quartz;
using Sisyphus.Data;
using Sisyphus.Utilities;

namespace Sisyphus.Jobs
{
    [DisallowConcurrentExecution]
    public class RunTasks : IJob
    {
        public Task Execute( IJobExecutionContext context )
        {
            TaskRunner.RunAll();

            return null;
        }
    }
}
