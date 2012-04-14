using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using Vaccine.Commands;
using Vaccine.Office.Web.Infrastructure;
using NHibernate.Linq;
using Vaccine.Core.Domain.Reporting;
using Vaccine.Office.Web.Models.CustomerReports;

namespace Vaccine.Office.Web.Controllers
{
    public class CustomerReportController : Controller
    {
        //
        // GET: /CustomerReport/
        protected CommandBus _cmdBus;
        //
        // GET: /BankAccount/

        protected ISessionFactory sf;

        public CustomerReportController(ISessionFactory sf)
        {
            this.sf = sf;
            this._cmdBus = ServiceLocator.Bus;
        }

        public ActionResult List()
        {
            using (var s = sf.OpenSession())
            {
                var customerReports = s.Query<CustomerReport>().ToList<CustomerReport>();

                s.Close();

                return View(customerReports);
            }
        }

        public ActionResult Detail(Guid Id)
        {
            using (var s = sf.OpenSession())
            {
                var customerReport = s.Query<CustomerReport>().Where(c => c.AggregateRootId == Id).FirstOrDefault();

                s.Close();

                return View(new DetailModel(customerReport));
            }
        }

    }
}
