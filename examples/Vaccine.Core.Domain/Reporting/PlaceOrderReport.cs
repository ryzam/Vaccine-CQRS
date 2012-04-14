using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Domain.BoundedContext.Order;
using Vaccine.Core.Domain.BoundedContext.CustomerRegistration;

namespace Vaccine.Core.Domain.Reporting
{
    public class PlaceOrderReport
    {
        public PlaceOrderReport()
        {
            Id = Guid.NewGuid();
        }

        public virtual Guid Id { get; set; }

        public virtual Guid AggregateRootId { get; set; }

        public virtual Guid CustomerId { get; set; }

        public virtual CustomerStatus CustomerStatus { get; set; }

        public virtual string CustomerName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Address1 { get; set; }

        public virtual string Address2 { get; set; }

        public virtual string Postcode { get; set; }

        public virtual Address.AddressType AddressType { get; set; }

        public virtual Guid ProductId { get; set; }

        public virtual string ProductName { get; set; }

        public virtual int Quantity { get; set; }

        public virtual decimal Price { get; set; }

        public virtual string Code { get; set; }

        public virtual decimal Discount { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual decimal TotalAmount { get; set; }
    }
}
