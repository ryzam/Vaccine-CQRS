using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Events;

namespace Vaccine.Core.Domain.BoundedContext.Order
{
    [Serializable]
    public class OrderPlacedEvent : DomainEvent
    {
        public CustomerOrder customerOrder;

        public ProductOrder productOrder;

        public decimal totalAmount;

        public decimal amount;

        public decimal discount;

        public OrderPlacedEvent(CustomerOrder customerOrder, OrderItem orderItem, decimal totalAmount)
        {
            // TODO: Complete member initialization
            this.customerOrder = customerOrder;
            this.productOrder = orderItem._productOrder;
            this.totalAmount = totalAmount;
            this.amount = orderItem._amount;
            this.discount = orderItem._discount;
        }
    }
}
