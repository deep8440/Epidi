using Epidi.Models.ViewModel.DeviceTokens;
using Epidi.Repository.DeviceTokensRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.DeviceTokensService
{
    public class DeviceTokensService : IDeviceTokensService
    {
        private readonly IDeviceTokensRepository _deviceTokensRepository;

        public DeviceTokensService(IDeviceTokensRepository deviceTokensRepository)
        {
            _deviceTokensRepository = deviceTokensRepository;
        }

        public Task<DeviceTokenViewModel> Delete(int Id)
        {
            return _deviceTokensRepository.Delete(Id);
        }

        public Task<DeviceTokenViewModel> Upsert(DeviceTokenViewModel model)
        {
            return _deviceTokensRepository.Upsert(model);
        }
    }
}
