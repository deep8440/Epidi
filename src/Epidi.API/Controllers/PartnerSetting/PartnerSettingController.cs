using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.PartnerSetting;
using Epidi.Service.PartnerSetting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.PartnerSetting
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerSettingController : ControllerBase
    {
        private readonly IPartnerSettingService _partnerSettingService;

        public PartnerSettingController(IPartnerSettingService partnerSettingService)
        {
            _partnerSettingService = partnerSettingService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<PartnerSettingViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] PartnerSettingViewModel req)
        {
            BaseResponse<ResponseViewModel> response = new BaseResponse<ResponseViewModel>();
            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _partnerSettingService.Upsert(req);
            response.ResponseMessage = string.Format(General.AccountCreated, "PartnerSetting");
            return StatusCode(response.ResponseCode, response);
        }
    }
}
