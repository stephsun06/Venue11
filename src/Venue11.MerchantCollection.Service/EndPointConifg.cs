using NServiceBus;
using StructureMap;
using Venue11.Domain.Mongo;
using Venue11.Domain.Mongo.Repositories;

namespace Venue11.MerchantCollection.Service
{
    public class EndPointConfig : IWantCustomInitialization, IConfigureThisEndpoint, AsA_Server
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
                x.For<IMongoDbConnection>().Singleton().Use<MongoDbConnection>();
                x.For<IMongoUnitOfWork>().Use<MongoUnitOfWork>();
                x.For<ICatalogRepository>().Use<CatalogRepository>();

            });
        }
    }
}
