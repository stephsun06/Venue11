using log4net;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venue11.MerchantCollection.Commands;
using Venue11.MerchantCollection.Service.Api;

namespace Venue11.MerchantCollection.Service.MessageHandler
{
    public class MerchantMessageHandler : IMessageHandler<GetCatalog>
    {

        private readonly ILog _log = LogManager.GetLogger(typeof(MerchantMessageHandler));

        private ICatalogCollectionProcess _collectionProcess;

        public MerchantMessageHandler(ICatalogCollectionProcess catalogCollectionProcess)
        {
            if (catalogCollectionProcess == null) throw new ArgumentNullException("catalogCollectionProcess");

            _collectionProcess = catalogCollectionProcess;
        }

        public void Handle(GetCatalog message)
        {
            _log.DebugFormat("Received GetCatalog : {0} {1} ", message.MerchantName, message.GroupKey);

            _collectionProcess.Run(message);
        }
    }
}
