using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Domain.BoundedContext.CustomerRegistration;

namespace Vaccine.Core.Domain.BoundedContext.Order
{
    public class OrderItem
    {
        public ProductOrder _productOrder;

        public CustomerOrder _customerOrder;

        public decimal _discount;

        public decimal _amount;

        public OrderItem(ProductOrder productOrder, CustomerOrder _customerOrder)
        {
            // TODO: Complete member initialization
            this._productOrder = productOrder;
            this._customerOrder = _customerOrder;
            this._discount = CalculateDiscount();
            this._amount = CalculateAmount();
            
        }

        private decimal CalculateAmount()
        {
            return _productOrder._price * _productOrder._quantity * _discount;
        }

        private decimal CalculateDiscount()
        {
            CustomerDiscount _customerDiscount;

            if (_customerOrder._customerStatus == CustomerStatus.Gold)
            {
                _customerDiscount = new CustomerDiscount(new CustomerGoldDiscount());
                return _customerDiscount.GetDiscount(_productOrder._quantity);
            }
            else if (_customerOrder._customerStatus == CustomerStatus.Silver)
            {
                _customerDiscount = new CustomerDiscount(new CustomerSilverDiscount());
                return _customerDiscount.GetDiscount(_productOrder._quantity);
            }
            else if (_customerOrder._customerStatus == CustomerStatus.Bronze)
            {
                _customerDiscount = new CustomerDiscount(new CustomerBrownDiscount());
                return _customerDiscount.GetDiscount(_productOrder._quantity);
            }
            return 0m;
        }

        
        
    }
}
