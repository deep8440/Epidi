using Epidi.Models.ViewModel.MobileUserTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.MobileUserTrackingService
{
    public interface IMobileUserTrackingService
    {
        Task<MobileUserTrackingViewModel> Upsert(MobileUserTrackingViewModel mobileUserTrackingViewModel);
    }
}
