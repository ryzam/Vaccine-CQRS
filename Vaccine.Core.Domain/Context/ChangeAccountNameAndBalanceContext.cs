using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Context;
using Vaccine.Core.Cqrs.Commands;
using Vaccine.Core.Cqrs.Events;
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
        EventStore _eventStore;

       
        public ChangeAccountNameAndBalanceContext(EventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public void Handle(ChangeAccountNameAndBalanceCommand command)
        {
            using (var s = _eventStore.OpenSession())
            {
                WebSessionContext.Bind(s);

                var account = _eventStore.GetById<Account>(command.AccountId);

                account.ChangeAccountNameAndBalance(command.AccountName, command.Balance);

                _eventStore.Save<Account>(account);

                WebSessionContext.Unbind(_eventStore._sf);

                s.Close();
            }

        }
    }
}
