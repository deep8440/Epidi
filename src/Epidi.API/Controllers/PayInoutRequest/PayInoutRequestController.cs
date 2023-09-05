using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.PayInoutRequest;
using Epidi.Service.PayInoutRequestService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.PayInoutRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayInoutRequestController : ControllerBase
    {
        private readonly IPayInoutRequestService _payInoutRequestService;

        public PayInoutRequestController(IPayInoutRequestService payInoutRequestService)
        {
            _payInoutRequestService = payInoutRequestService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<PayInoutRequestViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] PayInoutRequestViewModel req)
        {
            BaseResponse<PayInoutRequestViewModel> response = new BaseResponse<PayInoutRequestViewModel>();
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _payInoutRequestService.Upsert(req);
            response.ResponseMessage = string.Format(General.AccountCreated, "PayInoutRequest");
            return StatusCode(response.ResponseCode, response);
        }
    }
}
