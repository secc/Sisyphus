using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sisyphus.Data;
using Sisyphus.Models;
using Sisyphus.ViewModels;

namespace Sisyphus.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly DataContext dataContext;
        public OperationsService( DataContext context )
        {
            dataContext = context;
        }

        public void UpdateFromViewModel( OperationViewModel operationViewModel )
        {
            var operation = dataContext.Operations.Where( o => o.Id == operationViewModel.Id ).FirstOrDefault();
            if ( operation == null )
            {
                operation = new Operation();
                dataContext.Operations.Add( operation );
            }

            operation.Name = operationViewModel.Name;
            operation.Provider = dataContext.Providers.Where( p => p.Id == operationViewModel.ProviderId ).FirstOrDefault();

            var settingsObj = operation.SettingsObject;
            foreach ( var item in operationViewModel.FormCollection )
            {
                var properties = settingsObj.GetType().GetProperties();
                if ( item.Key.StartsWith( "SettingsObject." ) )
                {
                    var propName = item.Key.Split( "." )[1];
                    var property = properties.Where( p => p.Name == propName ).FirstOrDefault();
                    if ( property != null )
                    {
                        //Booleans are handled weird?
                        if ( property.PropertyType == typeof( bool ) || property.PropertyType == typeof( bool? ) )
                        {
                            property.SetValue( settingsObj, item.Value.Contains( "true" ) );
                        }
                        else
                        {
                            TypeConverter typeConverter = TypeDescriptor.GetConverter( property.PropertyType );
                            object propValue = typeConverter.ConvertFromString( item.Value );

                            property.SetValue( settingsObj, propValue );
                        }
                    }
                }
            }

            operation.Settings = JsonConvert.SerializeObject( settingsObj );
            dataContext.SaveChanges();
        }
    }

    public interface IOperationsService
    {
        public void UpdateFromViewModel( OperationViewModel operationViewModel );
    }
}
