using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sisyphus.Models;

namespace Sisyphus.TaskProviders
{
    public abstract class TaskProvider
    {
        public Operation Opperation { get; set; }
        public abstract object Settings { get; }
        public TaskProvider( Operation opperation )
        {
            Opperation = opperation;
        }
        public abstract bool Run(Dictionary<string,string> mergeValues );
    }
}
