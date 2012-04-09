using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using Vaccine.Commands;
using Vaccine.Office.Web.Infrastructure;
using Vaccine.Core.Domain.BoundedContext.ProductInventory;
using Vaccine.Core.Domain.Reporting;

namespace Vaccine.Office.Web.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
         protected CommandBus _cmdBus;
        //
        // GET: /BankAccount/

        protected ISessionFactory sf;


        public ProductController(ISessionFactory sf)
        {
            this.sf = sf;
            this._cmdBus = ServiceLocator.Bus;
        }

        public ActionResult Stock()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Stock(string name, string code,int stock,decimal price)
        {
            var createProductCommand = new CreateProductCommand(name, code, stock, price);

            _cmdBus.Send(createProductCommand);

            return RedirectToAction("Success");
            
        }

        public ActionResult List()
        {
            using (var s = sf.OpenSession())
            {
                var productStockReport = s.Query<ProductStockReport>().ToList<ProductStockReport>();

                s.Close();

                return View(productStockReport);
            }
        }

        public ActionResult Success()
        {
            return View();
        }

    }
}
