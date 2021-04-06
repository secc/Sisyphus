using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sisyphus.Models
{
    public class BundleOperation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid BundleId { get; set; }
        public Guid OperationId { get; set; }
        public virtual Bundle Bundle { get; set; }
        public virtual Operation Operation { get; set; }
        public int Order { get; set; }
    }
}
