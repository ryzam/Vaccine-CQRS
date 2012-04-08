using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vaccine.Core.Domain.Queries;
using System.Web.Mvc;
using Autofac;
using Vaccine.Office.Web.Models.AcademicApplication;

namespace Vaccine.Office.Web
{
    public static class ControllerQueryExtension
    {
        public static T ResolveQuery<T>(this Controller self)
        {
            return MvcApplication._container.Resolve<T>();
        }
    }

    public static class HelperQueryExtension
    {
        public static T ResolveQuery<T>(this IModel self)
        {
            return MvcApplication._container.Resolve<T>();
        }
    }
}