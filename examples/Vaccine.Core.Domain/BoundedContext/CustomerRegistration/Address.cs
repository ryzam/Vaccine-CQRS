using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaccine.Core.Domain.BoundedContext.CustomerRegistration
{
    public class Address : ValueObject
    {
        public enum AddressType
        {
            Home,Billing
        }

        public Address(string address1,string address2,string postCode,AddressType addressType)
        {
            this._address1 = address1;
            this._address2 = address2;
            this._postCode = postCode;
            this._addressType = addressType;
        }

        public string _address1;

        public string _address2;

        public string _postCode;

        public AddressType _addressType;
    }
}
