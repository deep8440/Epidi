using Epidi.Models.ViewModel.DeviceTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.DeviceTokensService
{
    public interface IDeviceTokensService
    {
        Task<DeviceTokenViewModel> Upsert(DeviceTokenViewModel model);
        Task<DeviceTokenViewModel> Delete(int Id);
    }
}
