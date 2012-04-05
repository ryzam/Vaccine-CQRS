using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Vaccine.Core.Domain.Infrastructure;
 
namespace Vaccine.Core.Domain.Queries
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
