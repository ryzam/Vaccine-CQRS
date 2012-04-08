using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using Vaccine.Office.Web.Infrastructure;
using Vaccine.Core.Cqrs.Commands;
using Vaccine.Office.Web.Models.Order;
using Vaccine.Core.Domain.Reporting;
using Vaccine.Core.Domain.BoundedContext.Order;
using Vaccine.Office.Web.Models;

namespace Vaccine.Office.Web.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/
         protected CommandBus _cmdBus;
        //
        // GET: /BankAccount/

        protected ISessionFactory sf;


        public OrderController(ISessionFactory sf)
        {
            this.sf = sf;
            this._cmdBus = ServiceLocator.Bus;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PlaceOrder()
        {
            using (var s = sf.OpenSession())
            {
                var placeOrderModel = new PlaceOrderModel(s);

                TempData["PlaceOrder"] = placeOrderModel;

                return View(placeOrderModel);
            }
        }

        [HttpPost]
        public ActionResult PlaceOrder(string add,Guid customer,Guid product,int quantity=0)
        {
            CustomerReport customerReport = null;

            ProductStockReport productStockReport = null;
            
            PlaceOrderModel placeOrderModel = null;

            using (var s = sf.OpenSession())
            {
                if (!string.IsNullOrEmpty(add))
                {
                    return AddOrderInMemory(customer, product, quantity, ref customerReport, ref productStockReport, ref placeOrderModel, s);

                }
                else
                {
                    IList<ProductOrder> productOrders = new List<ProductOrder>();

                    if (TempData["PlaceOrder"] != null)
                    {
                        placeOrderModel = (PlaceOrderModel)TempData["PlaceOrder"];

                        foreach(var placeOrder in placeOrderModel.OrderItemModels)
                        {
                            productOrders.Add(new ProductOrder() { _productId = placeOrder.ProductId, _quantity = placeOrder.Quantity});
                        }
                    }

                    var placeOrderCommand = new PlaceOrderCommand { CustomerId = customer, ProductId = product, ProductOrders = productOrders };
                    try
                    {
                        _cmdBus.Send(placeOrderCommand);

                        return View("Success");
                    }
                    catch(Exception err)
                    {
                        return RedirectToAction("Error", new { err = err.Message });
                    }
                }
                
            }
        }

        public ActionResult Error(string err)
        {
            var error = new ErrorModel { ErrorMessage = err };

            return View(error);
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult ViewOrder(Guid Id)
        {
            using (var s = sf.OpenSession())
            {
                var placeOrderReport = s.Query<PlaceOrderReport>().Where(c => c.CustomerId == Id).ToList<PlaceOrderReport>();

                s.Close();

                return View(placeOrderReport);
            }
        }

        private ActionResult AddOrderInMemory(Guid customer, Guid product, int quantity, ref CustomerReport customerReport, ref ProductStockReport productStockReport, ref PlaceOrderModel placeOrderModel, ISession s)
        {
            var _customerReports = s.Query<CustomerReport>().Where(c => c.AggregateRootId == customer);

            var _productReports = s.Query<ProductStockReport>().Where(c => c.AggregateRootId == product);

            if (_customerReports.Count() == 1)
                customerReport = _customerReports.FirstOrDefault();

            if (_productReports.Count() == 1)
                productStockReport = _productReports.FirstOrDefault();

            if (customerReport != null && productStockReport != null)
            {
                var orderItemModel = new OrderItemModel { CustomerName = customerReport.Name, CustomerId = customer, ProductId = product, ProductName = productStockReport.Name + " " + productStockReport.Code, Quantity = quantity };

                if (TempData["PlaceOrder"] != null)
                {
                    placeOrderModel = (PlaceOrderModel)TempData["PlaceOrder"];

                    placeOrderModel.OrderItemModels.Add(orderItemModel);

                    TempData["PlaceOrder"] = placeOrderModel;

                    return View(placeOrderModel);
                }
                else
                {
                    placeOrderModel = new PlaceOrderModel(s);

                    placeOrderModel.OrderItemModels.Add(orderItemModel);

                    TempData["PlaceOrder"] = placeOrderModel;

                    return View(placeOrderModel);
                }


            }
            return View();
        }

    }
}
