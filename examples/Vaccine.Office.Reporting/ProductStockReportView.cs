using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using VaccineExample.Core.Domain.BoundedContext.ProductInventory;
using Vaccine.Commands;
using VaccineExample.Core.Domain.Reporting;

namespace VaccineExample.Office.Reporting
{
    public class ProductStockReportView: IReportViewHandler<ProductCreatedEvent>
    {
        ISessionFactory _sf;

        public ProductStockReportView(ISessionFactory sf)
        {
            this._sf = sf;
        }

        public void Handle(ProductCreatedEvent r)
        {
            using (var s = _sf.OpenSession())
            {
                var productStockReport = new ProductStockReport { AggregateRootId = r.AggregateRootId, Name = r.name, Code = r.code, Price = r.price, Stock = r.stock };
                s.Save(productStockReport);
                s.Flush();
                s.Close();
            }
        }
    }
}
