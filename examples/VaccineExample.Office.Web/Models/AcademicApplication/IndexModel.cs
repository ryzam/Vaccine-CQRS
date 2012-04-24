using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediuCms.Office.Web.Models.AcademicApplication
{
    public class IndexModel :IModel
    {
        public int Rows { get; set; }

        public int Total { get; set; }

        public int Page { get; set; }

        public IList<AcademicApplicationRecords> Records { get; set; }

        public int StartPgNum { get; set; }

        public int EndPgNum { get; set; }

        public void CalculatePaging()
        {
            if (Page > 3)
            {
                StartPgNum = Page - 2;
                EndPgNum = Page + 2;
            }
            else
            {
                StartPgNum = 1;
                EndPgNum = 5;
            }
        }
    }

    public class AcademicApplicationRecords
    {
        public virtual string ReferenceNumber { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string OtherName { get; set; }

        public virtual string LatestStatus { get; set; }

        public virtual string SessionCode { get; set; }
    }
}