using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.IBLimit;
using Epidi.Service.IBService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.IB
{
    [Route("api/[controller]")]
    [ApiController]
    public class IBController : ControllerBase
    {
        private readonly IIBService _IIBService;
        public IBController(IIBService iBService)
        {
            _IIBService = iBService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<IBViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] IBViewModel req)
        {
            BaseResponse<IBViewModel> response = new BaseResponse<IBViewModel>();
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _IIBService.Upsert(req);
            response.ResponseMessage = string.Format(General.AccountCreated, "IB");
            return StatusCode(response.ResponseCode, response);
        }

        [HttpPost("ValidateIBLimit")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<IBLimitValidateViewModel>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> ValidateIBLimit([FromBody] IBLimitValidateViewModel req)
        {
            try
            {
                BaseResponse<IBLimitValidateRespViewModel> response = new BaseResponse<IBLimitValidateRespViewModel>();

                response.ResponseCode = (int)HttpStatusCode.OK;
                response.ResponseData = await _IIBService.ValidateIBLimit(req);
                response.ResponseData.ResponseMessage = string.Format(response.ResponseData.ResponseMessage, "ValidateIBLimit");
                return StatusCode(response.ResponseData.ResponseCode, response);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
