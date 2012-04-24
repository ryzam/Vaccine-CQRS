using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VaccineExample.Core.Domain.BoundedContext.ProductInventory;
using Vaccine.Commands;
using Vaccine.Events;
using NHibernate.Context;

namespace VaccineExample.Core.Domain.Context.ProductInventory
{
    public class StockNewProductContext : ICommandHandler<CreateProductCommand>
    {
        IEventStore _eventStore;

        public StockNewProductContext(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(CreateProductCommand cmd)
        {
            using (var s = _eventStore.Start())
            {
                var product = new Product(cmd.name, cmd.code, cmd.stock, cmd.price);
                _eventStore.Save<Product>(product);
            }
        }
    }
}
