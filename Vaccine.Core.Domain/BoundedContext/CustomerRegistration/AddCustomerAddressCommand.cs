using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Commands;
using Address = Vaccine.Core.Domain.BoundedContext.CustomerRegistration.Address;

namespace Vaccine.Core.Domain.BoundedContext.CustomerRegistration
{
    public class AddCustomerAddressCommand : DomainCommand
    {
        public Guid aggregateRootId;
        public string address1;
        public string address2;
        public string postCode;
        public Address.AddressType addressType;

        public AddCustomerAddressCommand(Guid aggregateRootId, string address1, string address2, string postCode, Core.Domain.BoundedContext.CustomerRegistration.Address.AddressType addressType)
        {
            // TODO: Complete member initialization
            this.aggregateRootId = aggregateRootId;
            this.address1 = address1;
            this.address2 = address2;
            this.postCode = postCode;
            this.addressType = addressType;
        }
    }
}
