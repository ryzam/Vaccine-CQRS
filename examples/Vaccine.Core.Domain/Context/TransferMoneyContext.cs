﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Dynamic.Role;
using NHibernate.Context;
using Vaccine.Commands;
using Vaccine.Events;
using Vaccine.Core.Domain.Infrastructure;
using NHibernate.Linq;
using Vaccine.Core.Domain.Reporting;
using Vaccine.Core.Domain.BoundedContext;

namespace Vaccine.Core.Domain.Context
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

        static TransferMoneyCommand _cmd;

        public TransferMoneyContext(IEventStore eventStore)
        {
            _eventStore = eventStore;
            
        }

        public void Handle(TransferMoneyCommand command)
        {
            using (var s = _eventStore.Start())
            {
                _cmd = command;

                var sourceAccount = _eventStore.GetById<Account>(_cmd.SourceAccountId);
                var sinkAccount = _eventStore.GetById<Account>(_cmd.SinkAccountId);

                sourceAccount.Compose<TransferMoney>()
                    .Transfer(sinkAccount);

                _eventStore.Save(sourceAccount);

                _eventStore.Save(sinkAccount);
            }

        }

        class TransferMoney : PlayedBy<Account>
        {
            public void Transfer(Account sinkAccount)
            {
                var bankAccountReport = QueryModule.Query<BankAccountReport>().Where(c => c.AggregateRootId == self.AggregateRootId);

                if (bankAccountReport.FirstOrDefault().Balance > _cmd.Amount)
                {
                    self.DecreaseBalance(_cmd.Amount);

                    sinkAccount.IncreaseBalance(_cmd.Amount);
                }
               
            }
        }
    }
}
