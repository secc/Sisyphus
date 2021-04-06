using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sisyphus.Data;
using Sisyphus.Models;
using Sisyphus.Utilities;

namespace Sisyphus.TaskProviders
{
    public class RunRazor : TaskProvider
    {
        private RunRazorSettings settings;
        public override object Settings => settings;

        public RunRazor( Operation opperation ) : base( opperation )
        {
            try
            {
                settings = JsonConvert.DeserializeObject<RunRazorSettings>( opperation.Settings );
            }
            catch
            {
            }

            if ( settings == null )
            {
                settings = new RunRazorSettings();
            }
        }


        public override bool Run( Dictionary<string, string> mergeValues )
        {

            try
            {

                var qry = MergeHelper.Merge( settings.Razor, mergeValues );

                return true;
            }
            catch (Exception es)
            {
                return settings.ContinueOnError;
            }
        }
    }

    public class RunRazorSettings
    {
        [DataType( DataType.MultilineText )]
        public string Razor { get; set; }

        public bool ContinueOnError { get; set; } = false;
    }
}
