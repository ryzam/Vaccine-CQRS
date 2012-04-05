using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vaccine.Core.Domain.Context;
using Vaccine.Office.Web.Infrastructure;
using NHibernate;
using NHibernate.Linq;
using Vaccine.Office.Web.Models.BankAccount;
using Vaccine.Office.Reporting;
using Vaccine.Core.Cqrs.Commands;
using Vaccine.Core.Domain.Reporting;

namespace Vaccine.Office.Web.Controllers
{
    public class BankAccountController : Controller
    {
        //protected RabbitMQPublisher _cmdBus;
        protected CommandBus _cmdBus;
        //
        // GET: /BankAccount/

        protected ISessionFactory sf;

        public BankAccountController(ISessionFactory sf)
        {
            this.sf = sf;
            this._cmdBus = ServiceLocator.Bus;
        }
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CreateAccount(string accountName,decimal? amount)
        {
            var openAccountCommand = new OpenAccountCommand { AccountName = accountName, Balance = amount.Value };

            _cmdBus.Send(openAccountCommand);

            return RedirectToAction("Success");
 
        }

        public ActionResult BankAccountReport()
        {
            using (var s = sf.OpenSession())
            {
                
                var report = s.Query<BankAccountReport>()
                    
                    .ToList();

                s.Close();

                return View(report);
            }
        }

        public ActionResult Detail(Guid id)
        {
            using (var s = sf.OpenSession())
            {
                var report = s.Query<BankAccountReport>().Where(c => c.AggregateRootId == id).FirstOrDefault();

                ViewBag.ObjectId = report.AggregateRootId;

                s.Close();

                return View(report);
            }
        }

        public ActionResult Edit(Guid id,string accountName,decimal amount)
        {
            var changeAccountNameAndBalanceCommand = new ChangeAccountNameAndBalanceCommand { AccountId = id, AccountName = accountName, Balance = amount };
            
            _cmdBus.Send(changeAccountNameAndBalanceCommand);
            
            return RedirectToAction("Detail", new { id = id });
        }

        public ActionResult Transfer()
        {
            using (var s = sf.OpenSession())
            {
                var transferModel = new TransferModel(s);

                return View(transferModel);
            }
        }

        [HttpPost]
        public ActionResult Transfer(Guid? SourceAccount, Guid? SinkAccount, decimal? amount)
        {
            var transferMoneyCommand = new TransferMoneyCommand { SourceAccountId = SourceAccount.Value, SinkAccountId = SinkAccount.Value, Amount = amount.Value };

            _cmdBus.Send(transferMoneyCommand);

            return RedirectToAction("BankAccountReport");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}
