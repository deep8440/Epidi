using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.User;
using Epidi.Service.ApplicationUserService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;
        public RegisterController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

       
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<RegisterUserDataViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Put([FromBody] RegisterUserDataViewModel req)
        {
            BaseResponse<RegisterUserDataViewModel> response = new BaseResponse<RegisterUserDataViewModel>();

            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _applicationUserService.Update(req);
            response.ResponseMessage = string.Format(General.AccountUpdated, req.Name);
            return StatusCode(response.ResponseCode, response);
        }
    }
}
