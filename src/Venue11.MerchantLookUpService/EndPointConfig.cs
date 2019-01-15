using System.Configuration;
using NServiceBus;
using StructureMap;
using log4net;
using Venue11.Domain.Repositories;
using Venue11.Domain.Dapper;

namespace Venue11.MerchantLookUpService
{
    public class EndPointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public void Init()
        {
            var container = ConfigureStructureMap();

            log4net.Config.XmlConfigurator.Configure();

            Configure.With()
                .DefiningCommandsAs(t => t.Namespace != null && (t.Namespace.StartsWith("Venue11.Domain.Commands")))
                .StructureMapBuilder(container)
                .XmlSerializer()
                .DisableSecondLevelRetries()
                .DisableTimeoutManager()
                .MsmqSubscriptionStorage();
        }

        private static IContainer ConfigureStructureMap()
        {
            return new Container(x =>
            {

                x.For<IDbConnectionFactory>().Singleton().Use<DbConnectionFactory>();
                x.For<IUnitOfWork>().Use<DapperUnitOfWork>();
                x.For<ISystemVariableRepository>().Use<SystemVariableRepository>();
                x.For<IMerchantRepository>().Use<MerchantRepository>();
            });
        }
    }
}
