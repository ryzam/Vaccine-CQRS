using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaccineExample.Core.Domain.BoundedContext.CustomerRegistration;
using NHibernate;
using Vaccine.Commands;
using VaccineExample.Office.Web.Infrastructure;
using Address = VaccineExample.Core.Domain.BoundedContext.CustomerRegistration.Address;
using VaccineExample.Office.Web.Models;

namespace VaccineExample.Office.Web.Controllers
{
    public class CustomerRegistrationController : Controller
    {
        //
        // GET: /CustomerRegistration/

        protected CommandBus _cmdBus;
        //
        // GET: /BankAccount/

        protected ISessionFactory sf;

       
        public CustomerRegistrationController(ISessionFactory sf)
        {
            this.sf = sf;
            this._cmdBus = ServiceLocator.Bus;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string name,string email)
        {
            var createCustomerCommand = new CreateCustomerCommand(name, email);

            _cmdBus.Send(createCustomerCommand);

            return RedirectToAction("Success");
        }

        [HttpPost]
        public ActionResult RegisterAddress(Guid? aggregateRootId,string address1,string address2,string postCode,Address.AddressType addressType)
        {
            try
            {
                var addCustomerAddressCommand = new AddCustomerAddressCommand(aggregateRootId.Value, address1, address2, postCode, addressType);

                _cmdBus.Send(addCustomerAddressCommand);

                return RedirectToAction("Success");
            }
            catch (Exception err)
            {

                return RedirectToAction("Error", new { err = err.Message});
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Error(string err)
        {
            var error = new ErrorModel { ErrorMessage = err };
            
            return View(error);
        }

    }
}
