using Epidi.API.Common;
using Epidi.Models;
using Epidi.Models.Extensions;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.TokenResult;
using Epidi.Models.ViewModel.User;
using Epidi.Service.ApplicationUserService;
using Epidi.Service.Helpers;
//using Epidi.Repository.Provider;
//using Epidi.Repository.UserStrategy;
//using Epidi.Repository.UserStrategyContext;
using Epidi.Service.TokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Epidi.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }


        [HttpPost("login")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<AuthenticateResponse>), (int)HttpStatusCode.OK)]
        //[Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> GetTokenAsync(LoginViewModel login)
        {
            BaseResponse<AuthenticateResponse> response = new BaseResponse<AuthenticateResponse>();            
            response.ResponseData = await _tokenService.Authenticate(login);
            response.ResponseCode = (int)response.ResponseData.statusCode;
            if (response.ResponseData.statusCode == HttpStatusCode.NotFound)
            {
                response.ResponseMessage = string.Format(General.NoAccountFound);
            }
            else if (response.ResponseData.statusCode == HttpStatusCode.Forbidden)
            {
                response.ResponseCode = (int)HttpStatusCode.OK;
            }
            else if (response.ResponseData.statusCode == HttpStatusCode.Accepted)
            {
                response.ResponseCode = (int)HttpStatusCode.OK;
            }
            else
            {
                response.ResponseMessage = string.Format(General.Success);
            }
           
            return StatusCode(response.ResponseCode, response);
        }


        //[AllowAnonymous]
        //[HttpPost("refresh-token")]
        //public IActionResult RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];
        //    var response = _userService.RefreshToken(refreshToken, ipAddress());
        //    setTokenCookie(response.RefreshToken);
        //    return Ok(response);
        //}
    }
}
