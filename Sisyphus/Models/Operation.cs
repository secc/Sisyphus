using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sisyphus.Data;
using Sisyphus.TaskProviders;

namespace Sisyphus.Models
{
    public class Operation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public Guid ProviderId { get; set; }
        public virtual Provider Provider { get; set; }
        public string Settings { get; set; }

        private object settingsObject;

        [NotMapped]
        public object SettingsObject
        {
            get
            {
                if ( settingsObject == null )
                {
                    var taskProvider = GetTaskProvider();
                    if ( taskProvider == null )
                    {
                        return null;
                    }
                    settingsObject = taskProvider.Settings;
                }
                return settingsObject;
            }
        }
        public TaskProvider GetTaskProvider()
        {
            if ( Provider == null )
            {
                return null;
            }
            var type = Type.GetType( Provider.FullyQualifiedTypeName );
            var taskProvider = ( TaskProvider ) Activator.CreateInstance( type, this );
            return taskProvider;
        }
    }
}
