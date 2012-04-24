using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaccineExample.Core.Domain.BoundedContext.Order
{
    public class CustomerBrownDiscount : ICustomerDiscountStrategy
    {
        public decimal GetDiscount(int quantity)
        {
            if (quantity <= 5)
                return 0.5m;
            else if (quantity > 5)
                return 0.8m;
            return 0m;
        }
    }
}
