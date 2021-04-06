using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Sisyphus.Models;

namespace Sisyphus.TaskProviders
{
    public class DeleteFile : TaskProvider
    {
        private DeleteFileSettings settings;
        public override object Settings => settings;

        public DeleteFile( Operation opperation ) : base( opperation )
        {
            try
            {
                settings = JsonConvert.DeserializeObject<DeleteFileSettings>( opperation.Settings );
            }
            catch
            {
            }

            if ( settings == null )
            {
                settings = new DeleteFileSettings();
            }
        }


        public override bool Run( Dictionary<string, string> mergeValues )
        {
            try
            {
                File.Delete( settings.FileLocation );
            }
            catch
            {

            }

            return true;
        }
    }

    internal class DeleteFileSettings
    {
        public string FileLocation { get; set; }
    }
}
