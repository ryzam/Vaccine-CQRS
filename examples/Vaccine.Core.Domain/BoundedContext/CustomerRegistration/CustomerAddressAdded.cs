using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Events;

namespace VaccineExample.Core.Domain.BoundedContext.CustomerRegistration
{
    [Serializable]
    public class CustomerAddressAddedEvent : DomainEvent
    {
        public string address1;
        public string address2;
        public string postCode;
        public Address.AddressType addressType;

        public CustomerAddressAddedEvent(string address1, string address2, string postCode, Address.AddressType addressType)
        {
            // TODO: Complete member initialization
            this.address1 = address1;
            this.address2 = address2;
            this.postCode = postCode;
            this.addressType = addressType;
        }
    }
}
