using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Context;
using NHibernate.Linq;
using Vaccine.Commands;
using Vaccine.Events;
using Vaccine.Core.Domain.BoundedContext;

namespace Vaccine.Core.Domain.Context
{
    [Serializable]
    public class OpenAccountCommand : DomainCommand
    {
        public decimal Balance { get; set; }

        public string AccountName { get; set; }
    }

    public class CreateBankAccountContext : ICommandHandler<OpenAccountCommand>
    {
        IEventStore _eventStore;

        Account _account;

        public CreateBankAccountContext(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(OpenAccountCommand command)
        {
            using (var s = _eventStore.Start())
            {
                _account = new Account(Guid.NewGuid(), command.Balance, command.AccountName);
                _eventStore.Save<Account>(_account);
            }
        }
    }
}
