using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Context;

namespace Vaccine.Office.Web.Controllers
{
    public class HomeController : Controller
    {
        protected ISessionFactory sf;



        public HomeController(ISessionFactory sf)
        {
            this.sf = sf;
        }

        public ActionResult Index()
        {
            using (var s = sf.OpenSession())
            {
                WebSessionContext.Bind(s);

                ViewBag.Message = "Welcome to ASP.NET MVC!";

                s.Close();

                WebSessionContext.Unbind(sf);

                return View();
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Layout()
        {
            return View();
        }
    }
}
