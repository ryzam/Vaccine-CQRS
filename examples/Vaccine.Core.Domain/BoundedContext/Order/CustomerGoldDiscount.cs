using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaccine.Core.Domain.BoundedContext.Order
{
    public class CustomerGoldDiscount : ICustomerDiscountStrategy
    {
        public decimal GetDiscount(int quantity)
        {
            if (quantity <= 5)
                return 2m;
            else if (quantity > 5)
                return 4m;
            return 0m;
        }
    }
}
