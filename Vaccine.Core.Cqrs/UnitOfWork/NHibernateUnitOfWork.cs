using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Context;
using Vaccine.Core.Cqrs.Aggregate;

namespace Vaccine.Core.Cqrs.UnitOfWork
{
    public class NHibernateUnitOfWork : IUnitOfWorkEs
    {
        protected ISessionFactory sf;

        protected ISession s;

        protected bool IsWeb;

        public NHibernateUnitOfWork(ISessionFactory sf, bool isWeb = true)
        {
            this.sf = sf;
            this.IsWeb = isWeb;
        }

        public IUnitOfWorkEs StartUnitOfWork()
        {
            if (s != null && s.IsOpen)
            {
                s = this.sf.GetCurrentSession();
            }
            else
            {
                s = this.sf.OpenSession();

                if (!IsWeb)
                    ThreadLocalSessionContext.Bind(s);
                else
                    WebSessionContext.Bind(s);
            }

            return this;
        }

        public void Save<T>(T aggregateRoot) where T : AggregateRootEs<T>
        {
            s.Save(aggregateRoot);
            s.Flush();
            var events = aggregateRoot.GetUncommitedEvents();

        }

        public T GetById<T>(Guid Id)
        {
            return s.Get<T>(Id);
        }

        public IQueryable<T> Gets<T>()
        {
            return null;//s.Query<T>();
        }


        public void Commit()
        {
            s.Flush();
        }

        public void Dispose()
        {
            WebSessionContext.Unbind(sf);
            s.Close();
        }
    }
}
