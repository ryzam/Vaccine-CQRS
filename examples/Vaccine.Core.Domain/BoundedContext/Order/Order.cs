using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Aggregate;
using Vaccine.Events;

namespace VaccineExample.Core.Domain.BoundedContext.Order
{
    public class Order : AggregateRootEs<Order>
    {
        public Order()
        {

        }

        public Order(IEnumerable<DomainEvent> events)
        {
            this.ReplayEvent(events);
        }

        public Order(CustomerOrder customerOrder,ProductOrder productOrder)
        {
            PlaceOrder(customerOrder, productOrder);
        }

        private void PlaceOrder(CustomerOrder customerOrder, ProductOrder productOrder)
        {
            var orderItem = new OrderItem(productOrder, customerOrder);

            var _totalAmount = orderItem._amount;

            this.ApplyEvent<OrderPlacedEvent>(new OrderPlacedEvent(customerOrder, orderItem, _totalAmount));
        }

        public Order(CustomerOrder customerOrder, IList<ProductOrder> productOrders)
        {
            foreach (var productOrder in productOrders)
            {
                PlaceOrder(customerOrder, productOrder);
            }

        }

        public IList<OrderItem> _orderItems;

        public CustomerOrder _customerOrder;

        public decimal _totalAmount;

        
        public void OnOrderPlacedEvent(OrderPlacedEvent e)
        {
            if (_orderItems == null)
                _orderItems = new List<OrderItem>();
            _customerOrder = e.customerOrder;
            _totalAmount += e.totalAmount;

            _orderItems.Add(new OrderItem(e.productOrder,e.customerOrder));
        }


        #region Snapshot
        public override void CreateSnapshot()
        {
            throw new NotImplementedException();
        }

        public override void OnSnapshotCreated(SnapshotCreatedEvent @event)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
