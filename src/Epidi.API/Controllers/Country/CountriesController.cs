using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Country;
using Epidi.Service.CountryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.Country
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<CountryDDl>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            BaseResponse<List<CountryDDl>> response = new BaseResponse<List<CountryDDl>>();

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _countryService.GetAllCountries();
            response.TotalRecords = response.ResponseData.Count;
            response.ResponseMessage = string.Format(General.Success);
            return StatusCode(response.ResponseCode, response);
        }
    }
}
