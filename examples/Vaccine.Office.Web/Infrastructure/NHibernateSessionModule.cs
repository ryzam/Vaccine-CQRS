using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using System.Reflection;
using Vaccine.Core.Domain;
//using MediuCms.Core.Domain.Model.Admission;
using NHibernate.Mapping.ByCode.Conformist;
using Vaccine.Events;
using Vaccine.Aggregate;

namespace Vaccine.Office.Web.Infrastructure
{
    public class NHibernateSessionModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configuration = new Configuration();

            var map = GetMappings();

            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<MsSql2008Dialect>();
                c.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"];

                c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                c.SchemaAction = SchemaAutoAction.Update;

            }).SetProperty("current_session_context_class", "web");

            configuration.AddDeserializedMapping(map, "NHSchemaTest");

            
            var sessionFactory = configuration.BuildSessionFactory();

            builder.RegisterInstance(sessionFactory).As<ISessionFactory>().SingleInstance();
        }

        private HbmMapping GetMappings()
        {
            var mapper = new ConventionModelMapper();

            //var baseEntityType = typeof(Entity);
            //mapper.IsEntity((t, declared) => baseEntityType.IsAssignableFrom(t) && baseEntityType != t && !t.IsInterface);
            //mapper.IsRootEntity((t, declared) => baseEntityType.Equals(t.BaseType));
 
            
            mapper.BeforeMapClass += (mi, type, map) =>
            {
                map.Id(g => g.Generator(Generators.Assigned));
            };

            mapper.Class<StoredEvent>(m => m.Property("PayLoadEvent", c => 
                { 
                    c.Type<NHibernate.Type.BinaryBlobType>();
                    c.Length(1048576);
                }));
            //mapper.Class<StoredEvent>(m => m.Property("Command", c => c.Type<NHibernate.Type.BinaryBlobType>()));

            

            //mapper.BeforeMapBag += (mi, type, map) =>
            //{
            //    map.Inverse(true);
            //    map.Lazy(CollectionLazy.Lazy);
            //};

            //AcademicApplication
            //mapper.Class<AcademicApplication>(m =>
            //    {
            //        m.Bag(x => x.AdmissionStatuses, cm =>
            //        {
            //            cm.Table("AdmissionStatus");
            //            cm.Cascade(Cascade.All);
            //            cm.Key(k => k.Column("AcademicApplication"));
            //        });
            //    }
            //);

            //mapper.Class<AdmissionWorkFlow>(m =>
            //    {
            //        m.Bag(x => x.AdmissionWorkFlows, cm =>
            //        {

            //            cm.Table("AdmissionWorkFlow");
            //            cm.Cascade(Cascade.All);
            //            cm.Key(k => k.Column("ParentAdmissionWorkFlow"));
            //            cm.Inverse(true);
            //        });
                    
            //    }
            //);


            var entities = new List<Type> { typeof(AggregateVersion), typeof(StoredEvent) };// Assembly.Load("MediuCms.Core.Cqrs").GetTypes();

            var reportingEntities = Assembly.Load("Vaccine.Core.Domain").GetTypes();

            foreach (var e in reportingEntities.Where(c=>c.Name.EndsWith("Report")))
            {
                entities.Add(e);
            }
            HbmMapping domainMapping = mapper.CompileMappingFor(entities);

            return domainMapping;
        }

    }

    
}