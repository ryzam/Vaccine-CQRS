using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Commands;
using Vaccine.Core.Cqrs.Events;
using Vaccine.Core.Domain.BoundedContext.CustomerRegistration;
using NHibernate.Context;

namespace Vaccine.Core.Domain.Context.CustomerRegistration
{
    public class AddCustomerAddressContext : ICommandHandler<AddCustomerAddressCommand>
    {
        EventStore _eventStore;

        public AddCustomerAddressContext(EventStore eventStore)
        {
            this._eventStore = eventStore;
        }

        public void Handle(AddCustomerAddressCommand cmd)
        {
            using (var s = _eventStore.OpenSession())
            {
                WebSessionContext.Bind(s);

                Customer customer = _eventStore.GetById<Customer>(cmd.aggregateRootId);

                customer.AddCustomerAddress(cmd.address1, cmd.address2, cmd.postCode, cmd.addressType);

                _eventStore.Save<Customer>(customer);

                s.Close();
            }

        }
    }
}
