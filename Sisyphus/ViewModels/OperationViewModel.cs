using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sisyphus.TaskProviders;

namespace Sisyphus.ViewModels
{
    public class OperationViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid ProviderId { get; set; }

        public object SettingsObject { get; set; }

        public IFormCollection FormCollection { get; set; }
    }
}
