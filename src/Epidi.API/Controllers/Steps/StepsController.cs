using Epidi.API.Common;
using Epidi.Models.Paging;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.DeviceTokens;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.Steps;
using Epidi.Models.ViewModel.User;
using Epidi.Models.ViewModel.UsersDocuments;
using Epidi.Service.StepsService;
using Epidi.Service.UsersDocumentsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.Steps
{
    [Route("api/[controller]")]
    [ApiController]
    public class StepsController : ControllerBase
    {

        private readonly IStepsService _stepsService;

        public StepsController(IStepsService stepsService)
        {
            _stepsService = stepsService;
        }
        [AllowAnonymous]
        [HttpGet("{UserId}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<UsersDocumentViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStepsById(int Id)
        {
            BaseResponse<StepsViewModel> response = new()
            {
                ResponseCode = (int)HttpStatusCode.Created,
                ResponseData = await _stepsService.GetStepsById(Id)
            };
            //response.ResponseMessage = string.Format(General.AccountUpdated, "UsersDocuments");
            return StatusCode(response.ResponseCode, response);
        }

        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveState(StepsViewModel model)
        {
            BaseResponse<StepsViewModel> response = new()
            {
                ResponseCode = (int)HttpStatusCode.Created,
                ResponseData = await _stepsService.SaveSteps(model)
            };

            return StatusCode(response.ResponseCode, response);
        }


        [AllowAnonymous]
        [HttpPatch]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<DeviceTokenViewModel>), (int)HttpStatusCode.OK)]
        public IActionResult DeleteSteps(int Id)
        {
            BaseResponse<List<ResponseViewModel>> response = new()
            {
                ResponseCode = (int)HttpStatusCode.Created,
                ResponseData = _stepsService.DeleteSteps(Id),
                ResponseMessage = string.Format(General.AccountDeleted, "DeviceTokens")
            };
            return StatusCode(response.ResponseCode, response);
        }
        [AllowAnonymous]
        [HttpGet("GetStepByCountryId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<List<StepsViewModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStepByCountryId(int CountryId, int UserId)
        {
            PageParam pageParam = new()
            {
                Offset = 0,
                Limit = 100
            };
            var res = await _stepsService.Steps_GetByCountryId(CountryId, UserId);
            BaseResponse<List<StepsViewModel>> response = new()
            {
                ResponseCode = (int)HttpStatusCode.OK,
                ResponseData = res,
                ResponseMessage = General.Success
            };
            response.TotalRecords = res.Count();


            return StatusCode(response.ResponseCode, response);
        }

        [AllowAnonymous]
        [HttpGet("GetStepAndPageNumberStatusInfo")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<List<StepAndPageNumberStatusViewModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStepAndPageNumberStatusInfo(int CountryId = 0)
        {
            BaseResponse<List<StepAndPageNumberStatusViewModel>> response = new()
            {
                ResponseCode = (int)HttpStatusCode.Created,
                ResponseData = await _stepsService.GetStepAndPageNumberStatusInfo(CountryId)
            };
            return StatusCode(response.ResponseCode, response);
        }
    }
}
