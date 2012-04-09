using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Domain.BoundedContext.ProductInventory;
using Vaccine.Core.Cqrs.Commands;
using Vaccine.Core.Cqrs.Events;
using NHibernate.Context;

namespace Vaccine.Core.Domain.Context.ProductInventory
{
    public class StockNewProductContext : ICommandHandler<CreateProductCommand>
    {
        EventStore _eventStore;

        public StockNewProductContext(EventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(CreateProductCommand cmd)
        {
            using (var s = _eventStore.OpenSession())
            {
                WebSessionContext.Bind(s);

                var product = new Product(cmd.name, cmd.code, cmd.stock, cmd.price);

                _eventStore.Save<Product>(product);

                WebSessionContext.Unbind(_eventStore._sf);

                s.Close();
            }
        }
    }
}
