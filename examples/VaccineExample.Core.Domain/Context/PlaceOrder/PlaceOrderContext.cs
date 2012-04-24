using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VaccineExample.Core.Domain.BoundedContext.Order;
using Vaccine.Commands;
using Vaccine.Events;
using VaccineExample.Core.Domain.BoundedContext.CustomerRegistration;
using VaccineExample.Core.Domain.BoundedContext.ProductInventory;

namespace VaccineExample.Core.Domain.Context.PlaceOrder
{
    public class PlaceOrderContext : ICommandHandler<PlaceOrderCommand>
    {
        IEventStore _eventStore;

        public PlaceOrderContext(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public void Handle(PlaceOrderCommand cmd)
        {
            using (var s = _eventStore.Start())
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
            }
        }
    }
}
