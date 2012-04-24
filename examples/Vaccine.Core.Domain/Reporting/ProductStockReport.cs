using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaccineExample.Core.Domain.Reporting
{
    public class ProductStockReport
    {
        public ProductStockReport()
        {
            Id = Guid.NewGuid();
        }

        public virtual Guid Id { get; set; }

        public virtual Guid AggregateRootId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual int Stock { get; set; }

        public virtual decimal Price { get; set; }
    }
}
