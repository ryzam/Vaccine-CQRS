using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaccineExample.Core.Domain.BoundedContext.Order
{
    public interface ICustomerDiscountStrategy
    {
        decimal GetDiscount(int quantity);
    }

    public class CustomerDiscount
    {
        ICustomerDiscountStrategy iCustomerDiscountStrategy;

        public CustomerDiscount(ICustomerDiscountStrategy iCustomerDiscountStrategy)
        {
            this.iCustomerDiscountStrategy = iCustomerDiscountStrategy;
        }

        public decimal GetDiscount(int quantity)
        {
            return this.iCustomerDiscountStrategy.GetDiscount(quantity);
        }
    }
}
