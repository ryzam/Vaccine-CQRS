using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Dynamic.Role;
using NHibernate.Context;
using Vaccine.Commands;
using Vaccine.Events;
using VaccineExample.Core.Domain.Infrastructure;
using NHibernate.Linq;
using VaccineExample.Core.Domain.Reporting;
using VaccineExample.Core.Domain.BoundedContext;

namespace VaccineExample.Core.Domain.Context
{
    [Serializable]
    public class TransferMoneyCommand : DomainCommand
    {
        public Guid SourceAccountId { get; set; }

        public Guid SinkAccountId { get; set; }

        public decimal Amount { get; set; }

    }

    public class TransferMoneyContext : ICommandHandler<TransferMoneyCommand>
    {
       
        IEventStore _eventStore;

       
        public TransferMoneyContext(IEventStore eventStore)
        {
            _eventStore = eventStore;
            
        }

        public void Handle(TransferMoneyCommand cmd)
        {
            using (var s = _eventStore.Start())
            {
                var sourceAccount = _eventStore.GetById<Account>(cmd.SourceAccountId);

                var sinkAccount = _eventStore.GetById<Account>(cmd.SinkAccountId);

                sourceAccount.Compose<TransferMoney>()
                    .Transfer(sinkAccount,cmd.Amount);

                _eventStore.Save(sourceAccount);

                _eventStore.Save(sinkAccount);
            }

        }

        class TransferMoney : PlayedBy<Account>, IRolePlayer
        {
            public void Transfer(Account sinkAccount,decimal amount)
            {
                var bankAccountReport = QueryModule.Query<BankAccountReport>().Where(c => c.AggregateRootId == self.AggregateRootId);

                if (bankAccountReport.FirstOrDefault().Balance > amount)
                {
                    self.DecreaseBalance(amount);

                    sinkAccount.IncreaseBalance(amount);
                }
               
            }
        }
    }
}
