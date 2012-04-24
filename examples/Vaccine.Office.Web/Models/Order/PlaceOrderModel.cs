using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaccineExample.Core.Domain.Reporting;
using NHibernate;
using NHibernate.Linq;

namespace VaccineExample.Office.Web.Models.Order
{
    public class PlaceOrderModel
    {
        public PlaceOrderModel(ISession s)
        {
            OrderItemModels = new List<OrderItemModel>();

            var customerReports = s.Query<CustomerReport>().ToList<CustomerReport>();

            var productReports = s.Query<ProductStockReport>()
                .Select(c => new { AggregateRootId = c.AggregateRootId, Name = c.Name + " " + c.Code })
                .ToList();

            Customers = new SelectList(customerReports, "AggregateRootId", "Name");

            Products = new SelectList(productReports, "AggregateRootId", "Name");
        }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }

        public IList<OrderItemModel> OrderItemModels { get; set; }
    }

    public class OrderItemModel
    {
        public Guid ProductId { get; set; }

        public Guid CustomerId { get; set; }

        public int Quantity { get; set; }

        public string CustomerName { get; set; }

        public string ProductName { get; set; }
    }
}