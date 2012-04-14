﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;

namespace Vaccine.Office.Web.Controllers
{
    public class AbstractController : Controller
    {
        protected ISessionFactory sf;

        public AbstractController()
        {

        }
        public AbstractController(ISessionFactory sf)
        {
            this.sf = sf;
        }

    }
}
