using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Domain.BoundedContext.CustomerRegistration;
using Vaccine.Commands;
using NHibernate;
using NHibernate.Linq;
using Vaccine.Core.Domain.Reporting;

namespace Vaccine.Office.Reporting
{
    public class CustomerReportView : IReportViewHandler<CustomerCreatedEvent>, IReportViewHandler<CustomerAddressAddedEvent>
    {
        ISessionFactory _sf;

        public CustomerReportView(ISessionFactory sf)
        {
            this._sf = sf;
        }

        public void Handle(CustomerCreatedEvent r)
        {
            using (var s = _sf.OpenSession())
            {
                var customerReport = new CustomerReport {AggregateRootId = r.AggregateRootId, Name = r.name, Email = r.email, CustomerStatus = r.customerStatus };
                s.Save(customerReport);
                s.Flush();
                s.Close();
            }
        }

        public void Handle(CustomerAddressAddedEvent r)
        {
            using (var s = _sf.OpenSession())
            {
                var customerReport = s.Query<CustomerReport>().Where(c => c.AggregateRootId == r.AggregateRootId).FirstOrDefault();

                if (r.addressType == Address.AddressType.Home)
                {
                    customerReport.Address1G1 = r.address1;
                    customerReport.Address2G1 = r.address2;
                    customerReport.PostCodeG1 = r.postCode;
                    customerReport.AddressTypeG1 = r.addressType;
                }
                else
                {
                    customerReport.Address1G2 = r.address1;
                    customerReport.Address2G2 = r.address2;
                    customerReport.PostCodeG2 = r.postCode;
                    customerReport.AddressTypeG2 = r.addressType;
                }

                s.Flush();
            }
        }
    }
}
