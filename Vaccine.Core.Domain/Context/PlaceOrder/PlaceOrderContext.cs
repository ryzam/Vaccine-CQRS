using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Domain.BoundedContext.Order;
using Vaccine.Core.Cqrs.Commands;
using Vaccine.Core.Cqrs.Events;
using Vaccine.Core.Domain.BoundedContext.CustomerRegistration;
using Vaccine.Core.Domain.BoundedContext.ProductInventory;

namespace Vaccine.Core.Domain.Context.PlaceOrder
{
    public class PlaceOrderContext : ICommandHandler<PlaceOrderCommand>
    {
        EventStore _eventStore;

        public PlaceOrderContext(EventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public void Handle(PlaceOrderCommand cmd)
        {
            using (var s = _eventStore.OpenSession())
            {
                Customer customer = _eventStore.GetById<Customer>(cmd.CustomerId);
                var customerOrder = customer.AsCustomerOrder();

                IList<ProductOrder> productOrders = new List<ProductOrder>();

                foreach (var item in cmd.ProductOrders)
                {
                    Product product = _eventStore.GetById<Product>(item._productId);
                    var productOrder = product.AsProductOrder(item._quantity);
                    productOrders.Add(productOrder);
                }

                var order = new Order(customerOrder, productOrders);

                _eventStore.Save<Order>(order);

                s.Close();
            }
        }
    }
}
