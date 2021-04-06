using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Quartz;
using Sisyphus.Utilities;

namespace Sisyphus.Jobs
{
    public class KeepAlive : IJob
    {
        public Task Execute( IJobExecutionContext context )
        {
            HttpClient httpClient = new HttpClient();
            httpClient.GetAsync( ConfigurationHelper.Configuration.GetValue<string>( "KeepAliveUrl" ) ).GetAwaiter().GetResult();

            return null;
        }
    }
}
