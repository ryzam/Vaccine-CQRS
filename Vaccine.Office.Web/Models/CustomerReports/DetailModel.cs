using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vaccine.Core.Domain.Reporting;
using System.Web.Mvc;
using Add = Vaccine.Core.Domain.BoundedContext.CustomerRegistration.Address;

namespace Vaccine.Office.Web.Models.CustomerReports
{
    public class DetailModel
    {
        public DetailModel(CustomerReport customerReport)
        {
            AddressModels = new List<AddressModel>();

            this.CustomerReport = customerReport;

            this.AggregateRootId = customerReport.AggregateRootId;

            AddressTypes = new SelectList(new List<string>{ Add.AddressType.Home.ToString(),Add.AddressType.Billing.ToString()});

            if (customerReport.Address1G1 != null)
                AddressModels.Add(new AddressModel { Address1 = customerReport.Address1G1, Address2 = customerReport.Address2G1, AddressType = customerReport.AddressTypeG1, PostCode = customerReport.PostCodeG1 });

            if (customerReport.Address1G2 != null)
                AddressModels.Add(new AddressModel { Address1 = customerReport.Address1G2, Address2 = customerReport.Address2G2, AddressType = customerReport.AddressTypeG2, PostCode = customerReport.PostCodeG2 });
        
        }

        public Guid AggregateRootId { get; set; }

        public CustomerReport CustomerReport { get; set; }

        public IEnumerable<SelectListItem> AddressTypes { get; set; }

        public IList<AddressModel> AddressModels { get; set; }
    }

    public class AddressModel
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string PostCode { get; set; }

        public Vaccine.Core.Domain.BoundedContext.CustomerRegistration.Address.AddressType AddressType { get; set; }
    }
}