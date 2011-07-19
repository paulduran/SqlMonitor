using System.Net.Mail;
using Bootstrap.WindsorExtension;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;

namespace SqlMonitor.Server.BootStrappers
{
    public class ServiceBusBootstrapper : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            Configure.With(AllAssemblies.Except("sybdrvado20.dll"))
                .CastleWindsor250Builder(container)
                .XmlSerializer();

            container.Register(
            AllTypes.FromAssemblyNamed("SqlMonitor.DataServices").Pick()
            .WithService.FirstInterface());

            container.Register(Component.For<SmtpClient>().Instance(new SmtpClient()));
        }
    }
}
