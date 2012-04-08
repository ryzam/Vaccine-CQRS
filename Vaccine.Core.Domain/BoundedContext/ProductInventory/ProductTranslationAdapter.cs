using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Domain.BoundedContext.Order;

namespace Vaccine.Core.Domain.BoundedContext.ProductInventory
{
    public static class ProductTranslationAdapter
    {
        public static ProductOrder AsProductOrder(this Product self,int quantity)
        {
            return new ProductOrder(self.AggregateRootId,self._name, self._price, self._code,quantity);
        }
    }
}
