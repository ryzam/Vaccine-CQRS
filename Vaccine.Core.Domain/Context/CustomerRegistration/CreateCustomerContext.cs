using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Commands;
using Vaccine.Core.Domain.BoundedContext.CustomerRegistration;
using Vaccine.Core.Cqrs.Events;
using NHibernate.Context;
using Vaccine.Core.Domain.Infrastructure;
using Vaccine.Core.Domain.Reporting;

namespace Vaccine.Core.Domain.Context.CustomerRegistration
{
    public class CreateCustomerContext : ICommandHandler<CreateCustomerCommand>
    {
        EventStore _eventStore;

        public CreateCustomerContext(EventStore eventStore)
        {
            _eventStore = eventStore;
            
        }

        public void Handle(CreateCustomerCommand command)
        {
            using (var s = _eventStore.OpenSession())
            {
                WebSessionContext.Bind(s);

                Customer customer;

                var customers = QueryModule.Query<CustomerReport>().Where(c => c.Email == command.email);

                if (customers.Count() == 0)
                {
                    customer = new Customer(command.name, command.email,CustomerStatus.Bronze);

                    _eventStore.Save<Customer>(customer);

                    WebSessionContext.Unbind(_eventStore._sf);

                    s.Close();
                }
                else
                {
                    WebSessionContext.Unbind(_eventStore._sf);

                    s.Close();

                    throw new Exception("Email already exist!");
                }

                

            }
        }
    }
}
