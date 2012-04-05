using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Vaccine.Office.Web.Infrastructure;
using System.Reflection;
using Vaccine.Core.Domain.Infrastructure;
using Vaccine.Core.Cqrs.Commands;
using Vaccine.Core.Cqrs.Events;
using Vaccine.Core.Domain.Context;
using NHibernate;
using Vaccine.Core.Cqrs.Queue;

namespace Vaccine.Office.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static IContainer _container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "BankAccountApplication", // Route name
                "BankAccount/{action}/{id}", // URL with parameters
                new { controller = "BankAccount", action = "Detail", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "AcademicApplication", // Route name
                "AcademicApplication/{action}/{refNumber}", // URL with parameters
                new { controller = "AcademicApplication", action = "Detail", refNumber = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            builder.RegisterModule(new NHibernateSessionModule());

            //builder.RegisterModule(new ReportingSessionModule());

            builder.RegisterModule(new QueryModule());

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();

            _container = container;

            QueryModule._container = _container;

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var rabbitMQPublisher = new RabbitMQPublisher(System.Configuration.ConfigurationManager.AppSettings["CLOUDAMQP_URL"]);

            var commandBus = new CommandBus(rabbitMQPublisher);

            var eventStore = new EventStore(commandBus, container.Resolve<ISessionFactory>());

            //Context
            var transferMoneyContext = new TransferMoneyContext(eventStore);
            var createBankAccountContext = new CreateBankAccountContext(eventStore);
            var changeAccountAndBalanceContext = new ChangeAccountNameAndBalanceContext(eventStore);

            //Register Command
            commandBus.RegisterHandlerCommand<OpenAccountCommand>(createBankAccountContext.Handle);
            commandBus.RegisterHandlerCommand<TransferMoneyCommand>(transferMoneyContext.Handle);
            commandBus.RegisterHandlerCommand<ChangeAccountNameAndBalanceCommand>(changeAccountAndBalanceContext.Handle);

            //Report View
            //var accountReportView = new AccountReportView(eventStore);

            ////Register Event
            //commandBus.RegisterHandlerEvent<AccountCreatedEvent>(accountReportView.Handle);
            //commandBus.RegisterHandlerEvent<AccountNameAndBalanceChangedEvent>(accountReportView.Handle);


            ServiceLocator.Pub = rabbitMQPublisher;
            ServiceLocator.Bus = commandBus;
            //ServiceLocator.Sub = rabbitMQSubsriber;
        }

        
    }
}