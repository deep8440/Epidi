using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Users;
using Epidi.Service.UsersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<UsersViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] UsersViewModel req)
        {
            BaseResponse<UsersViewModel> response = new BaseResponse<UsersViewModel>();
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _usersService.Upsert(req);
            response.ResponseMessage = string.Format(General.AccountCreated, "Users");
            return StatusCode(response.ResponseCode, response);
        }
        [AllowAnonymous]
        [HttpPatch]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<UsersViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Patch(int Id, [FromBody] UsersViewModel req)
        {
            BaseResponse<UsersViewModel> response = new BaseResponse<UsersViewModel>();
            req.Id = Id;
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _usersService.Upsert(req);
            response.ResponseMessage = string.Format(General.AccountUpdated, "Users");
            return StatusCode(response.ResponseCode, response);
        }
        [AllowAnonymous]
        [HttpGet("{MasterInstrumentId},{LPId}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<UsersFavourite>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UsersFavourite_ByMasterInstrumentIdLPId(int MasterInstrumentId, int LPId)
        {
            BaseResponse<List<UsersFavourite>> response = new BaseResponse<List<UsersFavourite>>();
            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _usersService.UsersFavourite_GetByMasterInstrumentIdLPId(MasterInstrumentId, LPId);
            response.ResponseMessage = string.Format(General.Success, "UsersFavourite");
            return StatusCode(response.ResponseCode, response);
        }
        
        [AllowAnonymous]
        [HttpGet("UsersFavouriteGetByUserId/{UserId}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<UsersFavourite>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UsersFavourite_ByUserId(int UserId)
        {
            BaseResponse<List<UsersFavourite>> response = new BaseResponse<List<UsersFavourite>>();
            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _usersService.UsersFavourite_GetByUserId(UserId);
            response.ResponseMessage = string.Format(General.Success, "UsersFavourite");
            response.TotalRecords = response.ResponseData.Count();
            return StatusCode(response.ResponseCode, response);
        }

        [AllowAnonymous]
        [HttpGet("PayoutRequestByUserId/{UserId}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<PayoutRequestData>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PayoutRequest_ByUserId(int UserId)
        {
            BaseResponse<List<PayoutRequestData>> response = new BaseResponse<List<PayoutRequestData>>();
            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _usersService.PayoutRequest_GetByUserId(UserId);
            response.ResponseMessage = string.Format(General.Success, "UsersFavourite");
            response.TotalRecords = response.ResponseData.Count();
            return StatusCode(response.ResponseCode, response);
        }

        [AllowAnonymous]
        [HttpPost("UserFavouriteInsert")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<UsersViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UserFavourite_Insert([FromBody] UsersFavouriteInsert req)
        {
            BaseResponse<UsersFavouriteInsert> response = new BaseResponse<UsersFavouriteInsert>();
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _usersService.UsersFavourite_Insert(req);
            response.ResponseMessage = string.Format(General.AccountCreated, "UserFavourite");
            return StatusCode(response.ResponseCode, response);
        }

        [AllowAnonymous]
        [HttpPost("PayoutRequestInsert")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<UsersViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PayoutRequest_Insert([FromBody] PayoutRequest req)
        {
            BaseResponse<PayoutRequest> response = new BaseResponse<PayoutRequest>();
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _usersService.PayoutRequests_Insert(req);

            if (response.ResponseData == null)
            {
                response.ResponseCode = (int)HttpStatusCode.OK;
                response.ResponseMessage = "You have insufficient balance for payout";
            }
            else
            {
                response.ResponseMessage = string.Format(General.AccountCreated, "Payout Request");
            }

            return StatusCode(response.ResponseCode, response);
        }

        [AllowAnonymous]
        [HttpPost("UserFavouriteRemove")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<UsersViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UserFavourite_Remove(int MasterInstrumentId, int UserId)
        {
            BaseResponse<UsersFavouriteInsert> response = new BaseResponse<UsersFavouriteInsert>();
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _usersService.UsersFavourite_Remove(MasterInstrumentId, UserId);
            response.ResponseMessage = string.Format(General.AccountCreated, "UserFavouriteRemove");
            return StatusCode(response.ResponseCode, response);
        }

		[AllowAnonymous]
		[HttpPost("UserStepCompleted")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.Forbidden)]
		[ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[ProducesResponseType(typeof(BaseResponse<UsersViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Users_StepCompleted(int UserId)
		{
			BaseResponse<UsersViewModel> response = new BaseResponse<UsersViewModel>();
			response.ResponseCode = (int)HttpStatusCode.Created;
			response.ResponseData = await _usersService.UserStepCompleted(UserId);
			response.ResponseMessage = string.Format(General.AccountCreated, "UserStepCompleted");
			return StatusCode(response.ResponseCode, response);
		}


	}
}
