using Epidi.Models.ViewModel.MobileUserTracking;
using Epidi.Repository.MobileUserTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.MobileUserTrackingService
{
    public class MobileUserTrackingService : IMobileUserTrackingService
    {
        private readonly IMobileUserTrackingRepository _mobileUserTrackingRepository;
        public MobileUserTrackingService(IMobileUserTrackingRepository mobileUserTrackingRepository)
        {
            _mobileUserTrackingRepository = mobileUserTrackingRepository;
        }
        public Task<MobileUserTrackingViewModel> Upsert(MobileUserTrackingViewModel mobileUserTrackingViewModel)
        {
            return _mobileUserTrackingRepository.Upsert(mobileUserTrackingViewModel);
        }
    }
}
