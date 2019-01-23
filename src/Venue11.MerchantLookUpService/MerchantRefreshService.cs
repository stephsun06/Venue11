using log4net;
using NServiceBus;
using System;
using System.Timers;
using System.Linq;
using Venue11.Domain.Repositories;
using Venue11.MerchantCollection.Commands;

namespace Venue11.MerchantLookUpService
{
    public class MerchantRefreshService : IWantToRunAtStartup
    {
        private readonly ILog log = LogManager.GetLogger(typeof(MerchantRefreshService));

        private int _lookUpInterval;
        private const int _firstRunLookUpInterval = 10000;
        private readonly Timer _timer = new Timer();

        private bool _firstRun;
        private int _recurHour;
        private IBus _bus;
        private ISystemVariableRepository _systemVariableRepository;
        private IMerchantRepository _merchantRepository;


        public MerchantRefreshService(IMerchantRepository merchantRepository, ISystemVariableRepository systemVariableRepository, IBus bus)
        {
            if (merchantRepository == null) throw new ArgumentNullException("merchantRepository");
            if (systemVariableRepository == null) throw new ArgumentNullException("systemVariableRepository");
            if (bus == null) throw new ArgumentNullException("bus");


            _merchantRepository = merchantRepository;
            _systemVariableRepository = systemVariableRepository;
            _bus = bus;

            Init();





        }

        private void Init()
        {
            log.Debug("");
            var config = _systemVariableRepository.GetSystemVariable();

            _lookUpInterval = Int32.Parse(config.FirstOrDefault(x => x.Variable == "LookupInterval").Value);
            _recurHour = Int32.Parse(config.FirstOrDefault(x => x.Variable == "RecurHour").Value);
            _timer.Interval = 10000;
            _timer.Elapsed += OnTimerElapsed;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;

            MerchantDataColleciton();

            if (_firstRun)
            {
                _timer.Interval = _lookUpInterval;
                log.DebugFormat("FirstRun Completed. Update LookupInterval to {0}", _lookUpInterval);
                _firstRun = false;
            }
            _timer.Enabled = true;
        }

        private void MerchantDataColleciton()
        {
            var list = _merchantRepository.GetMerchants();

            foreach (var item in list)
            {
                if ((DateTime.Now - item.requested).TotalHours > 24)
                {
                    _bus.Send(new GetCatalog()
                    {
                        MerchantName = item.merchant_name,
                        Id = item.id,
                        Url = item.url,
                        RequestedDate = DateTime.Now,
                        GroupKey = new Guid()
                    });
                }
            }
        }

        public void Run()
        {
            _firstRun = true;
            log.DebugFormat("FirstRunLookUpInterval={0}; LookupInterval={1}", _firstRunLookUpInterval, _lookUpInterval);
            _timer.Start();
        }

        public void Stop()
        {
            log.DebugFormat("Service is gracefully stopped");
        }
    }
}
