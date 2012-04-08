using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Domain.BoundedContext.Order;

namespace Vaccine.Core.Domain.BoundedContext.CustomerRegistration
{
    public static class CustomerTranslationAdapter
    {
        public static CustomerOrder AsCustomerOrder(this Customer self)
        {
            var address = self._addresses.Where(c=>c._addressType == Address.AddressType.Billing).FirstOrDefault();

            return new CustomerOrder
            {
                _customerId = self.AggregateRootId,
                _name = self._name,
                _email = self._email,
                _customerStatus = self._customerStatus,
                _address1 = address._address1,
                _address2 = address._address2,
                _addressType = address._addressType,
                _postcode = address._postCode
            };
        }
    }
}
