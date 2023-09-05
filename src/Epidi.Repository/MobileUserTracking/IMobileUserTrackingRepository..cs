using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidi.Models.ViewModel.MobileUserTracking;

namespace Epidi.Repository.MobileUserTracking
{
    public interface IMobileUserTrackingRepository
    {
        Task<MobileUserTrackingViewModel> Upsert(MobileUserTrackingViewModel mobileUserTrackingViewModel);
    }
}
