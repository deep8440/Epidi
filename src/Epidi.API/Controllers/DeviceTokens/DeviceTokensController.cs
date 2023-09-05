using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.DeviceTokens;
using Epidi.Service.DeviceTokensService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.DeviceTokens
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeviceTokensController : ControllerBase
    {
        private readonly IDeviceTokensService _deviceTokensService;
        public DeviceTokensController(IDeviceTokensService deviceTokensService)
        {
            _deviceTokensService = deviceTokensService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<DeviceTokenViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] DeviceTokenViewModel req)
        {
            BaseResponse<DeviceTokenViewModel> response = new BaseResponse<DeviceTokenViewModel>();
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _deviceTokensService.Upsert(req);
            response.ResponseMessage = string.Format(General.AccountCreated, "DeviceTokens");
            return StatusCode(response.ResponseCode, response);
        }
        [AllowAnonymous]
        [HttpPatch]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<DeviceTokenViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int Id)
        {
            BaseResponse<DeviceTokenViewModel> response = new BaseResponse<DeviceTokenViewModel>();
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _deviceTokensService.Delete(Id);
            response.ResponseMessage = string.Format(General.AccountDeleted, "DeviceTokens");
            return StatusCode(response.ResponseCode, response);
        }
    }
}
