using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
//using MediuCms.Core.Domain.Model.Admission;

namespace Vaccine.Office.Web.Controllers
{
    public class SetupController : AbstractController
    {
        //
        // GET: /Setup/
        public SetupController(ISessionFactory sf):base(sf)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RunDirect()
        {
            using (var s = sf.OpenSession())
            {
                

                return View("Index");
            }
        }

    }
}
