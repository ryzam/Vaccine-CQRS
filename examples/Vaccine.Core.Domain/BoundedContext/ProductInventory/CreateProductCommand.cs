using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Commands;

namespace Vaccine.Core.Domain.BoundedContext.ProductInventory
{
    public class CreateProductCommand : DomainCommand
    {
        public string name;
        public string code;
        public int stock;
        public decimal price;

        public CreateProductCommand(string name, string code, int stock, decimal price)
        {
            // TODO: Complete member initialization
            this.name = name;
            this.code = code;
            this.stock = stock;
            this.price = price;
        }

    }
}
