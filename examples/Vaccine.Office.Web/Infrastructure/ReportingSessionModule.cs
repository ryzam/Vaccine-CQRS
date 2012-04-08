using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Vaccine.Queue;
using Vaccine.Infra.QueueSubsriber;
using NHibernate;
using Vaccine.Office.Reporting;
using NHibernate.Cfg;
using Vaccine.Core.Domain;
using NHibernate.Dialect;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using System.Reflection;
using Vaccine.Core.Domain.Infrastructure;

namespace Vaccine.Office.Web.Infrastructure
{
    public class ReportingSessionModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var sessionFactory = DbConfig();

            builder.RegisterInstance(sessionFactory).As<IReportingSessionFactory>().SingleInstance();
        }

        private ISessionFactory DbConfig()
        {
            var configuration = new Configuration();

            var map = GetMappings();

            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<MsSql2008Dialect>();
                c.ConnectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=MediuCmsReporting;Integrated Security=True;Pooling=False";
                c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                c.SchemaAction = SchemaAutoAction.Update;

            }).SetProperty("current_session_context_class", "web");

            configuration.AddDeserializedMapping(map, "NHSchemaTest");

            var sessionFactory = configuration.BuildSessionFactory();

            return sessionFactory;

            //builder.RegisterInstance(sessionFactory).As<ISessionFactory>().SingleInstance();
        }

        private static HbmMapping GetMappings()
        {
            var mapper = new ConventionModelMapper();

            mapper.BeforeMapClass += (mi, type, map) =>
            {
                map.Id(g => g.Generator(Generators.Assigned));
            };

            var entities = Assembly.Load("Vaccine.Core.Domain").GetTypes();

            HbmMapping domainMapping = mapper.CompileMappingFor(entities.Where(c=>c.Name.EndsWith("Report")));

            return domainMapping;
        }
    }
}