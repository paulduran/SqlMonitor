using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Bootstrap.WindsorExtension;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;
using SqlMonitor.DataServices;

namespace SqlMonitor.Web.Windsor
{
    public class ComponentRegistration : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            RegisterServiceBus(container);
            RegisterControllers(container);

            container.Register(
              AllTypes.FromAssemblyNamed("SqlMonitor.DataServices").Pick()
              .WithService.FirstInterface());
        }

        private static void RegisterServiceBus(IWindsorContainer container)
        {
            var bus = Configure.WithWeb()
                .CastleWindsor250Builder(container)
//                .CastleWindsorBuilder(container)
                .DefaultBuilder()
                .Log4Net()
                .XmlSerializer()
                .MsmqTransport()
                    .IsTransactional(false)
                    .PurgeOnStartup(true)
                .MsmqSubscriptionStorage()
                .UnicastBus()
                .CreateBus()
                .Start();

            container.Register(Component.For<IBus>().Instance(bus));
            container.Register(Component.For<QueryContext>().ImplementedBy<QueryContext>().LifeStyle.PerWebRequest);
        }

        private static void RegisterControllers(IWindsorContainer container)
        {
            container.Register(AllTypes.FromAssembly(Assembly.GetExecutingAssembly())
                .BasedOn<Controller>()
                .Configure(c => c.LifeStyle.Transient.Named(c.Implementation.Name.ToLower()))); 
        }
    }
}