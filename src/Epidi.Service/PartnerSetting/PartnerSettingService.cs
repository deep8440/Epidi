using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.PartnerSetting;
using Epidi.Repository.PartnerSettingRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.PartnerSetting
{
    public class PartnerSettingService : IPartnerSettingService
    {
        private readonly IPartnerSettingRepository _partnerSettingRepository;

        public PartnerSettingService(IPartnerSettingRepository partnerSettingRepository)
        {
            _partnerSettingRepository = partnerSettingRepository;
        }
        public async Task<ResponseViewModel> Upsert(PartnerSettingViewModel model)
        {
            return await _partnerSettingRepository.Upsert(model);
        }
    }
}
