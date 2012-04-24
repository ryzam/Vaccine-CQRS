using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using NHibernate;
using Vaccine.Commands;
using VaccineExample.Core.Domain.Reporting;
using VaccineExample.Core.Domain.BoundedContext;

namespace VaccineExample.Office.Reporting
{
    
  
    public class AccountReportView : IReportViewHandler<AccountCreatedEvent>, IReportViewHandler<AccountNameAndBalanceChangedEvent>, IReportViewHandler<BalanceIncreasedEvent>, IReportViewHandler<BalanceDecreasedEvent>
    {
        ISessionFactory _sf;

        public AccountReportView(ISessionFactory sf)
        {
            this._sf = sf;
        }

        public void Handle(AccountCreatedEvent report)
        {
            using (var s = _sf.OpenSession())
            {

                var Id = Guid.NewGuid();

                var bankAccountReport = new BankAccountReport { Id = Id, AccountName = report.AccountName, Balance = report.OpeningBalance, AggregateRootId = report.AggregateRootId };

                s.Save(bankAccountReport);

                s.Flush();

            }

        }

        public void Handle(AccountNameAndBalanceChangedEvent report)
        {
            using (var s = _sf.OpenSession())
            {
                var bankAccountReport1 = s.Query<BankAccountReport>().Where(c => c.AggregateRootId == report.AggregateRootId).FirstOrDefault();
               

                bankAccountReport1.AccountName = report.AccountName;
                bankAccountReport1.Balance = report.Balance;

              
                s.Flush();

                s.Close();
            }
        }

        public void Handle(BalanceDecreasedEvent report)
        {
            using (var s = _sf.OpenSession())
            {
                var bankAccountReport1 = s.Query<BankAccountReport>().Where(c => c.AggregateRootId == report.AggregateRootId).FirstOrDefault();

                bankAccountReport1.Balance = report.Balance;
                Console.WriteLine(report.Balance);

                s.Flush();
            }
        }

        public void Handle(BalanceIncreasedEvent report)
        {
            using (var s = _sf.OpenSession())
            {
                var bankAccountReport1 = s.Query<BankAccountReport>().Where(c => c.AggregateRootId == report.AggregateRootId).FirstOrDefault();

                bankAccountReport1.Balance = report.Balance;


                s.Flush();
            }
        }
    }

}