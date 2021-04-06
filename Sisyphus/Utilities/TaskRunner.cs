using System;
using System.Collections.Generic;
using System.Linq;
using Sisyphus.Data;
using Sisyphus.Models;

namespace Sisyphus.Utilities
{
    public class TaskRunner
    {
        public static bool RunAll()
        {
            DataContext dataContext = new DataContext();
            var bundles = dataContext.Bundles;
            foreach ( var bundle in bundles )
            {
                if ( !RunBundle( bundle ) )
                {
                    return false;
                }
            }

            return true;
        }

        public static bool RunBundle( Bundle bundle )
        {
            Dictionary<string, string> mergeValues = GetMergeValues( bundle );

            var bundleOperations = bundle.BundleOperations.OrderBy( bo => bo.Order ).ToList();
            foreach ( var bundleOperation in bundleOperations )
            {
                var operation = bundleOperation.Operation;
                var taskProvider = operation.GetTaskProvider();
                if ( taskProvider != null )
                {
                    if ( !taskProvider.Run( mergeValues ) )
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static Dictionary<string, string> GetMergeValues( Bundle bundle )
        {
            DataContext dataContext = new DataContext();
            var values = dataContext.AppSettings.ToDictionary( s => s.Key, s => s.Value );
            values["BundleName"] = bundle.Name;
            return values;
        }
    }
}
