using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Domain.BoundedContext.Order;
using Vaccine.Commands;
using NHibernate;
using Vaccine.Core.Domain.Reporting;

namespace Vaccine.Office.Reporting
{
    public class PlaceOrderView : IReportViewHandler<OrderPlacedEvent>
    {
        ISessionFactory _sf;

        public PlaceOrderView(ISessionFactory sf)
        {
            this._sf = sf;
        }

        public void Handle(OrderPlacedEvent r)
        {
            using (var s = _sf.OpenSession())
            {
                var placeOrderReport = new PlaceOrderReport()
                {
                    AggregateRootId = r.AggregateRootId,
                    Address1 = r.customerOrder._address1,
                    Address2 = r.customerOrder._address2,
                    Postcode = r.customerOrder._postcode,
                    Email = r.customerOrder._email,
                    CustomerId = r.customerOrder._customerId,
                    CustomerName = r.customerOrder._name,
                    CustomerStatus = r.customerOrder._customerStatus,
                    AddressType = r.customerOrder._addressType,
                    Amount = r.amount,
                    Code = r.productOrder._code,
                    Discount = r.discount,
                    Price = r.productOrder._price,
                    ProductId = r.productOrder._productId,
                    ProductName = r.productOrder._name,
                    Quantity = r.productOrder._quantity,
                    TotalAmount = r.totalAmount
                };

                s.Save(placeOrderReport);
                s.Flush();
                s.Close();
            }
        }
    }
}
