using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Commands;
using VaccineExample.Core.Domain.BoundedContext.CustomerRegistration;
using Vaccine.Events;
using NHibernate.Context;
using VaccineExample.Core.Domain.Infrastructure;
using VaccineExample.Core.Domain.Reporting;

namespace VaccineExample.Core.Domain.Context.CustomerRegistration
{
    public class CreateCustomerContext : ICommandHandler<CreateCustomerCommand>
    {
        IEventStore _eventStore;

        public CreateCustomerContext(IEventStore eventStore)
        {
            _eventStore = eventStore;
            
        }

        public void Handle(CreateCustomerCommand command)
        {
            using (var s = _eventStore.Start())
            {
                Customer customer;


                var customers = QueryModule.Query<CustomerReport>().Where(c => c.Email == command.email);

                if (customers.Count() == 0)
                {
                    customer = new Customer(command.name, command.email,CustomerStatus.Bronze);

                    _eventStore.Save<Customer>(customer);

                }
                else
                {
                    throw new Exception("Email already exist!");
                }
            }
        }
    }
}
