using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sisyphus.Models;
using Sisyphus.TaskProviders;
using Sisyphus.Utilities;

namespace Sisyphus.Data
{
    public class DbInitializer
    {
        public static void Initialize( DataContext context )
        {
            context.Database.EnsureCreated();

            //Register task providers
            var taskProviderTypes = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany( assembly => assembly.GetTypes() )
                       .Where( type => type.IsSubclassOf( typeof( TaskProvider ) ) );

            foreach ( var taskProviderType in taskProviderTypes )
            {
                var matchedType = context.Providers.Where( p => p.FullyQualifiedTypeName == taskProviderType.AssemblyQualifiedName ).FirstOrDefault();
                if ( matchedType == null )
                {
                    matchedType = new Provider
                    {
                        FullyQualifiedTypeName = taskProviderType.AssemblyQualifiedName,
                        Name = taskProviderType.Name
                    };
                    context.Providers.Add( matchedType );
                }
            }
            context.SaveChanges();

            //Add settings
            var keys = new List<string> {
                Constants.BackupFileLocation,
                Constants.RestorationDatabase,
                Constants.RolledFilesLocation };

            foreach ( var key in keys )
                if ( !context.AppSettings.Any( s => s.Key == key ) )
                {
                    context.AppSettings.Add( new AppSetting { Key = key } );
                    context.SaveChanges();
                }
        }
    }
}
