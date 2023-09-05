using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.PartnerSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.PartnerSettingRepository
{
    public interface IPartnerSettingRepository
    {
        Task<ResponseViewModel> Upsert(PartnerSettingViewModel model);
    }
}
