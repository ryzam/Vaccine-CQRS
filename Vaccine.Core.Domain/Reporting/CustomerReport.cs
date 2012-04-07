using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Address = Vaccine.Core.Domain.BoundedContext.CustomerRegistration.Address;
using Vaccine.Core.Domain.BoundedContext.CustomerRegistration;

namespace Vaccine.Core.Domain.Reporting
{
    public class CustomerReport
    {
        public CustomerReport()
        {

        }

        public virtual Guid Id { get; set; }

        public virtual Guid AggregateRootId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Email { get; set; }

        public virtual string Address1G1 { get; set; }

        public virtual string Address2G1 { get; set; }

        public virtual string PostCodeG1 { get; set; }

        public virtual Address.AddressType AddressTypeG1 { get; set; }

        public virtual string Address1G2 { get; set; }

        public virtual string Address2G2 { get; set; }

        public virtual string PostCodeG2 { get; set; }

        public virtual Address.AddressType AddressTypeG2 { get; set; }

        public virtual CustomerStatus CustomerStatus { get; set; }


    }
}
