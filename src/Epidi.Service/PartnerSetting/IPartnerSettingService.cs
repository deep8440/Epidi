using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.PartnerSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.PartnerSetting
{
    public interface IPartnerSettingService
    {
        Task<ResponseViewModel> Upsert(PartnerSettingViewModel model);
    }
}
