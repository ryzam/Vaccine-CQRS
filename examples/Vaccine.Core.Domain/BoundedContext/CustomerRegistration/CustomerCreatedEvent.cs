using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Events;

namespace VaccineExample.Core.Domain.BoundedContext.CustomerRegistration
{
    [Serializable]
    public class CustomerCreatedEvent : DomainEvent
    {
        public string name;

        public string email;

        public CustomerStatus customerStatus;

        public CustomerCreatedEvent(string name, string email,CustomerStatus customerStatus)
        {
            this.EventState = EventState.New;
            this.name = name;
            this.email = email;
            this.customerStatus = customerStatus;
        }
    }
}
