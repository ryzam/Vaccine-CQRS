using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Context;

namespace Vaccine.Events
{
    public class NHibernateEventStoreTransaction : IEventStoreTransaction
    {
        public ISessionFactory SessionFactory { get; set; }
        public ISession Session { get; set; }

        public NHibernateEventStoreTransaction(ISessionFactory sf)
        {
            this.SessionFactory = sf;
            this.Session = sf.OpenSession();

            WebSessionContext.Bind(this.Session);;
        }

        public void Commit()
        {
            WebSessionContext.Unbind(this.SessionFactory);
            this.Session.Close();
        }

        public void Dispose()
        {
            Commit();
        }
    }
}
