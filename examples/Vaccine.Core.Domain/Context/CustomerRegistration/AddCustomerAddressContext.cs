using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Commands;
using Vaccine.Events;
using VaccineExample.Core.Domain.BoundedContext.CustomerRegistration;
using NHibernate.Context;

namespace VaccineExample.Core.Domain.Context.CustomerRegistration
{
    public class AddCustomerAddressContext : ICommandHandler<AddCustomerAddressCommand>
    {
        IEventStore _eventStore;

        public AddCustomerAddressContext(IEventStore eventStore)
        {
            this._eventStore = eventStore;
        }

        public void Handle(AddCustomerAddressCommand cmd)
        {
            using (var s = _eventStore.Start())
            {
                Customer customer = _eventStore.GetById<Customer>(cmd.aggregateRootId);

                customer.AddCustomerAddress(cmd.address1, cmd.address2, cmd.postCode, cmd.addressType);

                _eventStore.Save<Customer>(customer);
            }

        }
    }
}
