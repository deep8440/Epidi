using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Country;
using Epidi.Service.CountryWiseDocumentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.CountryWiseDocument
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CountryWiseDocumentController : ControllerBase
    {
        private readonly ICountryWiseDocumentService _countryWiseDocumentService;

        public CountryWiseDocumentController(ICountryWiseDocumentService countryWiseDocumentService)
        {
            _countryWiseDocumentService = countryWiseDocumentService;
        }

        [AllowAnonymous]
        [HttpGet("{CountryId}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<CountryWiseDocumentViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CountryWiseDocument(int CountryId)
        {
            BaseResponse<CountryWiseDocumentViewModel> response = new BaseResponse<CountryWiseDocumentViewModel>();
            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _countryWiseDocumentService.GetCountryWiseDocumentByCountryId(CountryId);            
            return StatusCode(response.ResponseCode, response);
        }
    }
}
