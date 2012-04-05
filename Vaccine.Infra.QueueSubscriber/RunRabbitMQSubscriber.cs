using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Commands;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;
using Vaccine.Core.Cqrs.Infrastructure;
using Vaccine.Core.Cqrs.Events;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Cfg;
using NHibernate;
using NHibernate.Dialect;
using Vaccine.Core.Domain.Context;
using NHibernate.Type;
using Vaccine.Core.Cqrs.Aggregate;
using System.Threading;
using System.Reflection;
using Vaccine.Office.Reporting;
using Vaccine.Core.Domain.BoundedContext;

namespace Vaccine.Infra.QueueSubsriber
{
    public class RabbitMQSubscriber
    {
        public static void Main(string[] args)
        {
            var commandBus = new CommandBus();

            var sessionFactory = DbConfig();

            var accountReportView = new AccountReportView(sessionFactory);

            commandBus.RegisterHandlerEvent<AccountCreatedEvent>(accountReportView.Handle);

            commandBus.RegisterHandlerEvent<AccountNameAndBalanceChangedEvent>(accountReportView.Handle);

            commandBus.RegisterHandlerEvent<BalanceDecreasedEvent>(accountReportView.Handle);

            commandBus.RegisterHandlerEvent<BalanceIncreasedEvent>(accountReportView.Handle);

            var r = new RunRabbitMQSubscriber(commandBus);
        }

        public static ISessionFactory DbConfig()
        {
            var configuration = new Configuration();

            var map = GetMappings();

            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<MsSql2008Dialect>();
                c.ConnectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=MediuCms;Integrated Security=True;Pooling=False";
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

            HbmMapping domainMapping = mapper.CompileMappingFor(entities.Where(c => c.Name.EndsWith("Report")));

            return domainMapping;
        }
    }

    public class RunRabbitMQSubscriber
    {
        ConnectionFactory factory;
        IConnection conn;
        CommandBus bus;

        public RunRabbitMQSubscriber(CommandBus bus)
        {
            this.bus = bus;
            Console.WriteLine("Creating factory...");
            factory = new ConnectionFactory();

            Console.WriteLine("Creating connection...");
            factory.Protocol = Protocols.FromEnvironment();
            factory.HostName = "localhost";
            conn = factory.CreateConnection();

            Console.WriteLine("Creating channel...");
            try
            {
                QueChannel(bus);
            }
            catch
            {
                Thread.Sleep(100000);
                QueChannel(bus);
            }
        }

        private void QueChannel(CommandBus bus)
        {
            using (IModel model = conn.CreateModel())
            {
                var subscription = new Subscription(model, "queue", false);

                while (true)
                {
                    BasicDeliverEventArgs basicDeliveryEventArgs =
                        subscription.Next();
                    var @event = StreamExtension.Deserialize<Vaccine.Core.Cqrs.Events.DomainEvent>(basicDeliveryEventArgs.Body);

                    //Encoding.UTF8.GetString(basicDeliveryEventArgs.Body);
                    Console.WriteLine(@event.GetType());

                    bus.Publish(@event);

                    subscription.Ack(basicDeliveryEventArgs);
                }
            }
        }
    }
}

