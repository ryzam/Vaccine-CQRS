using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Commands;

namespace Vaccine.Core.Domain.BoundedContext.Order
{
    public class PlaceOrderCommand : DomainCommand
    {
        public PlaceOrderCommand()
        {
            ProductOrders = new List<ProductOrder>();
        }
        public Guid CustomerId { get; set; }

        public Guid ProductId { get; set; }

        public IList<ProductOrder> ProductOrders { get; set; }
    }
}
