using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Context;
using Vaccine.Commands;
using Vaccine.Events;
using Vaccine.Core.Domain.BoundedContext;

namespace Vaccine.Core.Domain.Context
{
    [Serializable]
    public class ChangeAccountNameAndBalanceCommand : DomainCommand
    {
        public Guid AccountId { get; set; }

        public string AccountName { get; set; }

        public decimal Balance { get; set; }
    }

    public class ChangeAccountNameAndBalanceContext : ICommandHandler<ChangeAccountNameAndBalanceCommand>
    {
        IEventStore _eventStore;

       
        public ChangeAccountNameAndBalanceContext(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public void Handle(ChangeAccountNameAndBalanceCommand command)
        {
            using (var s = _eventStore.Start())
            {
                var account = _eventStore.GetById<Account>(command.AccountId);

                account.ChangeAccountNameAndBalance(command.AccountName, command.Balance);

                _eventStore.Save<Account>(account);
            }

        }
    }
}
