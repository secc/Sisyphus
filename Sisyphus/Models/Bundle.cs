using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sisyphus.Models
{
    public class Bundle
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<BundleOperation> BundleOperations { get; set; }

        public void SquashOrder()
        {
            //Sink everything down to lowest number in order.
            var bundleOps = this.BundleOperations.OrderBy( bo => bo.Order ).ToList();
            var i = 0;
            bundleOps.ForEach( bo => bo.Order = i++ );
        }

        public int GetNextOperationId()
        {
            return BundleOperations.Any() ? ( BundleOperations.Max( b => b.Order ) + 1 ) : 0;
        }
    }
}
