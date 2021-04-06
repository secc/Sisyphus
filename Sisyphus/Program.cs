using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sisyphus.Data;
using Sisyphus.Jobs;

namespace Sisyphus
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var host = CreateHostBuilder( args ).Build();

            CreateDbIfNotExists( host );
            StartJobs();

            host.Run();
        }

        private static void StartJobs()
        {
            JobSetup.InitJobs();
        }

        private static void CreateDbIfNotExists( IHost host )
        {
            using ( var scope = host.Services.CreateScope() )
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    DbInitializer.Initialize( context );
                }
                catch ( Exception ex )
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError( ex, "An error occurred creating the DB." );
                }
            }
        }

        public static IHostBuilder CreateHostBuilder( string[] args ) =>
            Host.CreateDefaultBuilder( args )
                .ConfigureWebHostDefaults( webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 } );
    }
}
