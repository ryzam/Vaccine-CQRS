using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Aggregate;
using Vaccine.Events;

namespace VaccineExample.Core.Domain.BoundedContext.CustomerRegistration
{
    public class Customer : AggregateRootEs<Customer>
    {
        public Customer()
        {

        }

        public Customer(IEnumerable<DomainEvent> events)
        {
            this.ReplayEvent(events);
        }

        public string _name;

        public string _email;

        public CustomerStatus _customerStatus;

        public IList<Address> _addresses;

        //Create New Customer
        public Customer(string name, string email,CustomerStatus customerStatus)
        {
            
            this.ApplyEvent<CustomerCreatedEvent>(new CustomerCreatedEvent(name, email,customerStatus));
        }

        public void OnCustomerCreated(CustomerCreatedEvent e)
        {
            this._name = e.name;
            this._email = e.email;
            this._customerStatus = e.customerStatus;
            _addresses = new List<Address>();
        }

        public void AddCustomerAddress(string address1,string address2,string postCode,VaccineExample.Core.Domain.BoundedContext.CustomerRegistration.Address.AddressType addressType)
        {
            var _filterAddress = _addresses.Where(c => c._addressType == addressType);
            if (_filterAddress!=null && _filterAddress.Count() == 1)
                throw new Exception("Duplicate address type!");
            else
                this.ApplyEvent<CustomerAddressAddedEvent>(new CustomerAddressAddedEvent(address1, address2, postCode, addressType));
        }

        public void OnCustomerAddressAdded(CustomerAddressAddedEvent e)
        {
            if (_addresses == null)
                _addresses = new List<Address>();
            var address = new Address(e.address1, e.address2, e.postCode, e.addressType);
            _addresses.Add(address);
        }

        #region Snapshot
        public override void CreateSnapshot()
        {
            throw new NotImplementedException();
        }

        public override void OnSnapshotCreated(SnapshotCreatedEvent @event)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
