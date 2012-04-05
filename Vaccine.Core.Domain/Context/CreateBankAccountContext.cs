using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Context;
using NHibernate.Linq;
using Vaccine.Core.Cqrs.Commands;
using Vaccine.Core.Cqrs.Events;
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
        EventStore _eventStore;

        Account _account;

        public CreateBankAccountContext(EventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(OpenAccountCommand command)
        {
            using (var s = _eventStore.OpenSession())
            {
                WebSessionContext.Bind(s);

                //ThreadStaticSessionContext.Bind(s);


                _account = new Account(Guid.NewGuid(), command.Balance, command.AccountName);

                _eventStore.Save<Account>(_account);

                WebSessionContext.Unbind(_eventStore._sf);

                //ThreadStaticSessionContext.Unbind(_eventStore._sf);

                s.Close();
            }
        }

        //public Account GetAggregate()
        //{
        //    return _account;
        //}


    }
}
