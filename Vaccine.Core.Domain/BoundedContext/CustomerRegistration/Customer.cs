using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Aggregate;
using Vaccine.Core.Cqrs.Events;

namespace Vaccine.Core.Domain.BoundedContext.CustomerRegistration
{
    public class Customer : AggregateRootEs<Customer>
    {
        
        public Customer(IEnumerable<DomainEvent> events)
        {
            this.ReplayEvent(events);

        }

        public string _name;

        public string _email;

        //Create New Customer
        public Customer(string name, string email)
        {
            //this.ApplyEvent<CustomerCreatedEvent>(new CustomerCreatedEvent(name, email));
        }

        public void OnCustomerCreated(CustomerCreatedEvent e)
        {
        }

        public override void CreateSnapshot()
        {
            throw new NotImplementedException();
        }

        public override void OnSnapshotCreated(SnapshotCreatedEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
