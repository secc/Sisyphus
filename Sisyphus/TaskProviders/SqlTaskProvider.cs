using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sisyphus.Data;
using Sisyphus.Models;
using Sisyphus.Utilities;

namespace Sisyphus.TaskProviders
{
    public class SqlTaskProvider : TaskProvider
    {
        private SqlTaskSettings settings;
        public override object Settings => settings;

        public SqlTaskProvider( Operation opperation ) : base( opperation )
        {
            try
            {
                settings = JsonConvert.DeserializeObject<SqlTaskSettings>( opperation.Settings );
            }
            catch
            {
            }

            if ( settings == null )
            {
                settings = new SqlTaskSettings();
            }
        }


        public override bool Run( Dictionary<string, string> mergeValues )
        {
            var qry = MergeHelper.Merge( settings.Query, mergeValues );

            try
            {
                if ( settings.UseSisyphusContext )
                {
                    var context = new DataContext();
                    context.RawSqlQuery( qry, 3600 );
                }
                else
                {
                    var context = new RestorationContext();
                    context.RawSqlQuery( qry, 3600 );
                }

                return true;
            }
            catch ( Exception e )
            {
                return settings.ContinueOnError;
            }
        }
    }

    public class SqlTaskSettings
    {
        [DataType( DataType.MultilineText )]
        public string Query { get; set; }

        public bool ContinueOnError { get; set; } = false;

        public bool UseSisyphusContext { get; set; } = false;
    }
}
