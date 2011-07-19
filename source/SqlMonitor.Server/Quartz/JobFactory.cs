using System;
using Castle.Windsor;
using Quartz;
using Quartz.Spi;

namespace SqlMonitor.Server.Quartz
{
    public class JobFactory : IJobFactory
    {
        private readonly IWindsorContainer container;

        public JobFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle)
        {
            try
            {
                JobDetail jobDetail = bundle.JobDetail;
                Type jobType = jobDetail.JobType;
                // Return job that is registrated in container
                return (IJob) container.Resolve(jobType);
            }
            catch (Exception e)
            {
                throw new SchedulerException("Problem instantiating class", e);
            }
        }
    }
}
