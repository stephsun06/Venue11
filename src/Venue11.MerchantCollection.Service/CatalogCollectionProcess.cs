using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Venue11.Domain.Commands.MerchantCollection;
using Venue11.Domain.Mongo.Entities;
using Venue11.Domain.Mongo.Repositories;
using Venue11.MerchantCollection.Service.Api;

namespace Venue11.MerchantCollection.Service
{
    public class CatalogCollectionProcess : ICatalogCollectionProcess
    {

        private string _apikey;
        private Guid _groupkey;
        private string _merchantName;
        private ICatalogApiService _catalogApiService;
        private ICatalogRepository _catalogRepository;

        private Queue<string> _queue = new Queue<string>();

        private readonly ILog _log = LogManager.GetLogger(typeof(CatalogCollectionProcess));

        public bool IsBusy { get; set; }
        public bool IsRunning { get; set; }
       
        public CatalogCollectionProcess(ICatalogApiService apiService , ICatalogRepository repository)
        {
            if (apiService == null) throw new ArgumentNullException("apiService");
            if (repository == null) throw new ArgumentNullException("repository");

            _catalogApiService = apiService;
            _catalogRepository = repository;
        }

        public void Run(GetCatalog getCatalog)
        {
            _apikey = getCatalog.ApiKey;
            _merchantName = getCatalog.MerchantName;
            _groupkey = getCatalog.GroupKey;

            _queue.Enqueue(getCatalog.Url);

            IsRunning = true;

            while(IsRunning)
            {
                if(_queue.Count > 0 && !IsBusy)
                {
                    Downloading(_queue.Dequeue());
                }

                Thread.Sleep(5000);
            }
        }

        private void Downloading(string url)
        {
            var task = _catalogApiService.GetCatalog(url, _apikey);
            task.ContinueWith(x =>
            {
                _log.DebugFormat("Dowload Failed");

                IsBusy = false;
                IsRunning = false;

            }, TaskContinuationOptions.OnlyOnFaulted);



            task.ContinueWith(t =>
            {
                var result = t.Result;
                if (result != null)
                {
                    var item = _catalogRepository.InsertLog(
                       new ProductLog()
                       {
                           MerchantName = _merchantName,
                           ReceivdDate = DateTime.Now,
                           Catalog = result,

                       });
                    
                    _log.DebugFormat("Next Url {0}", result.navigation.next_page_url);

                    if (!string.IsNullOrEmpty(result.navigation.next_page_url))
                    {
                        _queue.Enqueue(result.navigation.next_page_url);
                        
                    }
                    else
                    {
                        ///Complete Downloading Looping 
                        IsRunning = false;
                    }

                    return;
                }


                IsBusy = false;


            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
