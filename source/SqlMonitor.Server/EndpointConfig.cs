using AutoMapper;
using Bootstrap;
using Bootstrap.WindsorExtension;
using NServiceBus;

namespace SqlMonitor.Server
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization, IWantCustomLogging
    {
        public void Init()
        {
            Bootstrapper.With.Container(new WindsorContainerExtension())
                .Start();
            SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);
//            Mapper.AssertConfigurationIsValid();
        }
    }
}
