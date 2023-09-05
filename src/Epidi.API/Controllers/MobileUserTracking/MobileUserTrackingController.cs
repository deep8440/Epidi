using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.MobileUserTracking;
using Epidi.Service.MobileUserTrackingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.MobileUserTracking
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileUserTrackingController : ControllerBase
    {
        private readonly IMobileUserTrackingService _mobileUserTrackingService;

        public MobileUserTrackingController(IMobileUserTrackingService mobileUserTrackingService)
        {
            _mobileUserTrackingService = mobileUserTrackingService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<MobileUserTrackingViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] MobileUserTrackingViewModel req)
        {
            BaseResponse<MobileUserTrackingViewModel> response = new BaseResponse<MobileUserTrackingViewModel>();
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _mobileUserTrackingService.Upsert(req);
            response.ResponseMessage = string.Format(General.AccountCreated, "MobileUserTracking");
            return StatusCode(response.ResponseCode, response);
        }

    }
}
