﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using MediuCms.Core.Domain.Queries;
using Dynamic.Role;
using NHibernate.Context;
using MediuCms.Office.Web.Models.AcademicApplication;

namespace MediuCms.Office.Web.Controllers
{
    public class AcademicApplicationController : AbstractController
    {
        //
        // GET: /Applicant/
        public AcademicApplicationController(ISessionFactory sf):base(sf)
        {

        }

        public ActionResult Index(int page = 0,int rows = 10)
        {
            using (var s = sf.OpenSession())
            {
                WebSessionContext.Bind(s);

                var query = this.ResolveQuery<AcademicApplicationQuery>();

                var count = query.CountRecords();

                var total = (int)Math.Ceiling((float)count / (float)rows);

                if (page == 0)
                    page = 1;
                int pageIndex = Convert.ToInt32(page) - 1;

                var results = query.GetApplicants().Take(rows).Skip(pageIndex * rows)
                    .Select(c => new AcademicApplicationRecords
                    {
                        FirstName = c.FirstName,
                        LatestStatus = c.LatestStatus,
                        ReferenceNumber = c.ReferenceNumber,
                        OtherName = c.OtherName,
                        SessionCode = c.SessionCode
                    }).ToList();

                var indexModel = new IndexModel
                {
                    Page = page,
                    Total = total,
                    Rows = count,
                    Records = results
                };
                indexModel.CalculatePaging();

                WebSessionContext.Unbind(sf);

                s.Close();

                return View(indexModel);
            }
        }

        public ActionResult Detail(string refNumber)
        {
            using (var s = sf.OpenSession())
            {
                WebSessionContext.Bind(s);

                var detailModel = new DetailModel(refNumber);

                WebSessionContext.Unbind(sf);

                s.Close();

                return View(detailModel);
            }
        }

    }
}
