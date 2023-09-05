using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.UsersTransactions;
using Epidi.Service.UsersTransactionsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.UsersTransactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersTransactionsController : ControllerBase
    {
        private readonly IUsersTransactionsService _usersTransactionsService;

        public UsersTransactionsController(IUsersTransactionsService usersTransactionsService)
        {
            _usersTransactionsService = usersTransactionsService;   
        }
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<UsersTransactionViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] UsersTransactionViewModel req)
        {
            BaseResponse<UsersTransactionViewModel> response = new BaseResponse<UsersTransactionViewModel>();
            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _usersTransactionsService.Upsert(req);
            response.ResponseMessage = string.Format(General.AccountCreated, "UsersTransactions");
            return StatusCode(response.ResponseCode, response);
        }
    }
}
