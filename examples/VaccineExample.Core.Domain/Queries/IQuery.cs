using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using VaccineExample.Core.Domain.Infrastructure;
 
namespace VaccineExample.Core.Domain.Queries
{
    public interface IQueryRepo
    {
        ISession Session { get; }
    }

    public class QueryImpl : IQueryRepo
    {
        public QueryImpl(ISessionFactory s)
        {
            this.Session = s.GetCurrentSession();
        }

        public ISession Session { get; private set; }
    }
}
