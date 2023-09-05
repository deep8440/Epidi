using Epidi.API.Common;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Service.TermsConditionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Epidi.Models;
using Epidi.Models.Model;
using Microsoft.Extensions.Configuration;

namespace Epidi.API.Controllers.TermsCondition
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermsConditionController : ControllerBase
    {
        private readonly ITermsConditionService _termsConditionService;
        private readonly IConfiguration _configuration;
        public TermsConditionController(ITermsConditionService termsConditionService,IConfiguration iConfig)
        {
            _termsConditionService = termsConditionService;
            _configuration = iConfig;
        }

        [AllowAnonymous]
        [HttpGet("TermsConditionByCountryId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<TermsConditionViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int CountryId, int UserId)
        {
            BaseResponse<List<TermsConditionViewModel>> response = new BaseResponse<List<TermsConditionViewModel>>();

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _termsConditionService.GetTermsCondition_ByCountryId(CountryId);
            foreach (var item in response.ResponseData)
            {
                item.FileUrl = _configuration.GetSection("FileGet").GetSection("BaseUrlFileGet").Value + item.FileUrl + "&" + "UserId=" + UserId;
            }
            response.TotalRecords = response.ResponseData.Count();
            return StatusCode(response.ResponseCode, response);
        }

   



        [AllowAnonymous]
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<TermsConditionViewModelSelect>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTermsConditionBy_Id()
        {
            BaseResponse<TermsConditionViewModelSelect> response = new BaseResponse<TermsConditionViewModelSelect>();

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _termsConditionService.GetTermsConditionBy_Id();
            return StatusCode(response.ResponseCode, response);
        }


        [AllowAnonymous]
        [HttpGet("CountryId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<TermsConditionViewModelTitle>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTermsConditionByCountry(int CountryId)
        {
            BaseResponse<List<TermsConditionViewModelTitle>> response = new BaseResponse<List<TermsConditionViewModelTitle>>();

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _termsConditionService.GetTermsConditionByCountry(CountryId);
            return StatusCode(response.ResponseCode, response);
        }
    }
}
