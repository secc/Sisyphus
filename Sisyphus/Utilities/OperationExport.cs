using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sisyphus.Data;
using Sisyphus.Models;

namespace Sisyphus.Utilities
{
    public class OperationExport
    {
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public string ProviderFullyQualifiedTypeName { get; set; }
        public string Settings { get; set; }


        public static OperationExport GetExport( Operation operation )
        {
            if ( operation == null )
            {
                return null;
            }

            var export = new OperationExport
            {
                Name = operation.Name,
                ProviderName = operation.Provider.Name,
                ProviderFullyQualifiedTypeName = operation.Provider.FullyQualifiedTypeName,
                Settings = operation.Settings
            };
            return export;
        }

        public static Operation Import( string import )
        {
            try
            {
                OperationExport operationExport = JsonConvert.DeserializeObject<OperationExport>( import );

                DataContext dataContext = new DataContext();
                var provider = dataContext.Providers.Where( p => p.FullyQualifiedTypeName == operationExport.ProviderFullyQualifiedTypeName ).FirstOrDefault();

                var operation = new Operation()
                {
                    Id = Guid.NewGuid(),
                    Provider = provider,
                    Settings = operationExport.Settings,
                    Name = operationExport.Name
                };

                dataContext.Operations.Add( operation );
                dataContext.SaveChanges();

                return operation;
            }
            catch
            {
                return null;
            }


        }

    }
}
