using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Domain.BoundedContext.CustomerRegistration;
using Address = Vaccine.Core.Domain.BoundedContext.CustomerRegistration.Address;

namespace Vaccine.Core.Domain.BoundedContext.Order
{
    [Serializable]
    public class CustomerOrder
    {
        public Guid _customerId { get; set; }

        public CustomerStatus _customerStatus { get; set; }

        public string _name { get; set; }

        public string _email { get; set; }

        public string _address1 { get; set; }

        public string _address2 { get; set; }

        public string _postcode { get; set; }

        public Address.AddressType _addressType { get; set; }

    }
}
