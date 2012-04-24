using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;
using System.Web.Mvc;
using VaccineExample.Office.Reporting;
using VaccineExample.Core.Domain.Reporting;

namespace VaccineExample.Office.Web.Models.BankAccount
{
    public class TransferModel
    {

        public TransferModel(ISession s)
        {

           var accountReports = s.Query<BankAccountReport>()
                .Select(c => new AccountReport { AccountName = c.AccountName, ObjectId = c.AggregateRootId })
                .ToList<AccountReport>();

           SourceAccounts = new SelectList(accountReports, "ObjectId", "AccountName");
           SinkAccounts = new SelectList(accountReports, "ObjectId", "AccountName");
        }

        public IEnumerable<SelectListItem> SourceAccounts { get; set; }

        public IEnumerable<SelectListItem> SinkAccounts { get; set; }
    }

    public class AccountReport
    {
        public string AccountName { get; set; }

        public Guid ObjectId { get; set; }
    }
}