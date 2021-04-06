using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sisyphus.Data;
using Sisyphus.Models;

namespace Sisyphus.TaskProviders
{
    public class Delay : TaskProvider
    {
        private DelaySettings settings;
        public override object Settings => settings;

        public Delay( Operation opperation ) : base( opperation )
        {
            try
            {
                settings = JsonConvert.DeserializeObject<DelaySettings>( opperation.Settings );
            }
            catch
            {
            }

            if ( settings == null )
            {
                settings = new DelaySettings();
            }
        }


        public override bool Run( Dictionary<string, string> mergeValues )
        {
            Task.Delay( settings.SecondsToWait * 1000 );
            return true;
        }
    }

    internal class DelaySettings
    {
        public int SecondsToWait { get; set; }
    }
}
