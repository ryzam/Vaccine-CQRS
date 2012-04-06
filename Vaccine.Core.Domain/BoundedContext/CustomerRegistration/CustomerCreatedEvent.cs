using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Events;

namespace Vaccine.Core.Domain.BoundedContext.CustomerRegistration
{
    public class CustomerCreatedEvent : DomainEvent
    {
        private string name;
        private string email;

        public CustomerCreatedEvent(Guid aggregateRootId,string name, string email):base(aggregateRootId)
        {
            // TODO: Complete member initialization
            this.name = name;
            this.email = email;
        }
    }
}
