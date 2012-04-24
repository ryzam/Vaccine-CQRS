using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaccineExample.Core.Domain.BoundedContext.Order
{
    [Serializable]
    public class ProductOrder
    {
        public ProductOrder()
        {

        }

        public ProductOrder(Guid productId,string name,decimal price,string code, int quantity)
        {
            this._productId = productId;
            this._name = name;
            this._price = price;
            this._code = code;
            this._quantity = quantity;
        }

        public Guid _productId { get; set; }

        public string _name { get; set; }

        public int _quantity { get; set; }

        public decimal _price { get; set; }

        public string _code { get; set; }
    }
}
