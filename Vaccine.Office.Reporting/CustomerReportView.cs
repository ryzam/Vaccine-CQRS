using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Domain.BoundedContext.CustomerRegistration;
using Vaccine.Core.Cqrs.Commands;
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

        public void Handle(CustomerCreatedEvent report)
        {
            using (var s = _sf.OpenSession())
            {
                var customerReport = new CustomerReport {Id= Guid.NewGuid(), AggregateRootId = report.AggregateRootId, Name = report.name, Email = report.email, CustomerStatus = report.customerStatus };
                s.Save(customerReport);
                s.Flush();
                s.Close();
            }
        }

        public void Handle(CustomerAddressAddedEvent report)
        {
            using (var s = _sf.OpenSession())
            {
                var customerReport = s.Query<CustomerReport>().Where(c => c.AggregateRootId == report.AggregateRootId).FirstOrDefault();

                if (report.addressType == Address.AddressType.Home)
                {
                    customerReport.Address1G1 = report.address1;
                    customerReport.Address2G1 = report.address2;
                    customerReport.PostCodeG1 = report.postCode;
                    customerReport.AddressTypeG1 = report.addressType;
                }
                else
                {
                    customerReport.Address1G2 = report.address1;
                    customerReport.Address2G2 = report.address2;
                    customerReport.PostCodeG2 = report.postCode;
                    customerReport.AddressTypeG2 = report.addressType;
                }

                s.Flush();
            }
        }
    }
}
