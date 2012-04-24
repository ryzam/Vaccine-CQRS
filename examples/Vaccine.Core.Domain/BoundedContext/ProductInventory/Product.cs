using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Aggregate;
using Vaccine.Events;

namespace VaccineExample.Core.Domain.BoundedContext.ProductInventory
{
    public class Product : AggregateRootEs<Product>
    {
        public Product()
        {

        }

        public Product(IEnumerable<DomainEvent> events)
        {
            this.ReplayEvent(events);
        }

        public Product(string name, string code, int stock, decimal price)
        {
            this.ApplyEvent<ProductCreatedEvent>(new ProductCreatedEvent(name, code, stock, price));
        }

        public void OnProductCreated(ProductCreatedEvent e)
        {
            this._name = e.name;
            this._code = e.code;
            this._stock = e.stock;
            this._price = e.price;
        }

        public string _name;

        public string _code;

        public int _stock;

        public decimal _price;

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
