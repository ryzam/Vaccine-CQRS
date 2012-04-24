using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Events;

namespace VaccineExample.Core.Domain.BoundedContext.ProductInventory
{
    [Serializable]
    public class ProductCreatedEvent : DomainEvent
    {
        public string name;
        public string code;
        public int stock;
        public decimal price;

        public ProductCreatedEvent(string name, string code, int stock, decimal price)
        {
            // TODO: Complete member initialization
            this.name = name;
            this.code = code;
            this.stock = stock;
            this.price = price;
        }
    }
}
