using Bootstrap.WindsorExtension;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SqlMonitor.Server.Quartz.Jobs;

namespace SqlMonitor.Server.Quartz
{
    public class QuartzBootstrapper : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.Register(Component.For<ISchedulerFactory>().ImplementedBy<StdSchedulerFactory>());
            container.Register(Component.For<IScheduler>()
                                   .UsingFactory((ISchedulerFactory factory) => factory.GetScheduler()));
            container.Register(Component.For<IJobFactory>().ImplementedBy<JobFactory>());
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            container.Register(Component.For<QueryRunner>());

            var scheduler = container.Resolve<IScheduler>();
            scheduler.JobFactory = container.Resolve<IJobFactory>();
            scheduler.Start();
        }
    }
}
