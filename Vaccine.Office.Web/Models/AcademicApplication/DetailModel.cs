using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediuCms.Core.Domain.Queries;

namespace MediuCms.Office.Web.Models.AcademicApplication
{
    public class DetailModel :IModel
    {
        public DetailModel(string refNumber)
        {
            var q = this.ResolveQuery<AcademicApplicationQuery>();

            var r = q.GetApplicantRefNumber(refNumber).FirstOrDefault();

            Name = r.FirstName;

            OtherName = r.OtherName;

            RefNumber = r.ReferenceNumber;
        }

        public string Name { get; set; }

        public string OtherName { get; set; }

        public string RefNumber { get; set; }
    }
}