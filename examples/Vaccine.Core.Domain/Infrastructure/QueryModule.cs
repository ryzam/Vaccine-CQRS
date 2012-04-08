using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using NHibernate;
using Vaccine.Core.Domain.Queries;
using NHibernate.Linq;
 
namespace Vaccine.Core.Domain.Infrastructure
{
    public class QueryModule : Autofac.Module
    {
        public static IContainer _container;

        protected override void Load(Autofac.ContainerBuilder builder)
        {
            //builder.Register(c=> new CommandBus()).As<CommandBus>();
            //builder.Register(c => new AcademicApplicationQuery(c.Resolve<ISessionFactory>().GetCurrentSession())).As<AcademicApplicationQuery>();
            //builder.Register(c => new EventStore(c.Resolve<CommandBus>(),c.Resolve<ISessionFactory>())).As<EventStore>();
            builder.Register(c => new QueryImpl(c.Resolve<ISessionFactory>())).As<IQueryRepo>();
        }

        public static IQueryable<T> Query<T>()
        {
            return _container.Resolve<IQueryRepo>().Session.Query<T>();
        }
    }

    
}
