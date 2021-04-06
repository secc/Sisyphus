using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sisyphus.TaskProviders;

namespace Sisyphus.Models
{
    public class Provider
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullyQualifiedTypeName { get; set; }
    }
}
