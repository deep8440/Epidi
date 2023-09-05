using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Service.IBCommisionMarkUpSettingInstrumentWise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.IBCommisionMarkUpSettingInstrumentWise
{
    [Route("api/[controller]")]
    [ApiController]
    public class IBCommisionMarkUpSettingInstrumentWiseController : ControllerBase
    {
        private readonly IIBCommisionMarkUpSettingInstWiseService _IIBCommisionMarkUpSettingInstWiseService;

        public IBCommisionMarkUpSettingInstrumentWiseController(IIBCommisionMarkUpSettingInstWiseService iIBCommisionMarkUpSettingInstWiseService)
        {
            _IIBCommisionMarkUpSettingInstWiseService = iIBCommisionMarkUpSettingInstWiseService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<IBCommisionMarkUpSettingInstrumentWiseViewModel>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> Post([FromBody] IBCommisionMarkUpSettingInstrumentWiseViewModel req)
        //{
        //    try
        //    {
        //        BaseResponse<IBCommisionMarkUpSettingInstrumentWiseViewModel> response = new BaseResponse<IBCommisionMarkUpSettingInstrumentWiseViewModel>();
        //        response.ResponseCode = (int)HttpStatusCode.Created;
        //        response.ResponseData = await _IIBCommisionMarkUpSettingInstWiseService.Upsert(req);
        //        response.ResponseMessage = string.Format(General.AccountCreated, "IBCommisionMarkUpSettingInstrumentWise");
        //        return StatusCode(response.ResponseCode, response);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}




        //public async Task<IActionResult> Post([FromBody] List<IBCommisionMarkUpSettingInstrumentWiseViewModel> reqList)
        //{
        //    try
        //    {
        //        BaseResponse<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>> response = new BaseResponse<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>>();
        //        response.ResponseCode = (int)HttpStatusCode.Created;

        //        List<IBCommisionMarkUpSettingInstrumentWiseViewModel> responseData = new List<IBCommisionMarkUpSettingInstrumentWiseViewModel>();

        //        foreach (var req in reqList)
        //        {
        //            var result = await _IIBCommisionMarkUpSettingInstWiseService.Upsert(req);
        //            responseData.Add(result);
        //        }

        //        response.ResponseData = responseData;
        //        response.ResponseMessage = string.Format(General.AccountCreated, "IBCommisionMarkUpSettingInstrumentWise");

        //        return StatusCode(response.ResponseCode, response);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception appropriately
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        public async Task<IActionResult> Post([FromBody] List<IBCommisionMarkUpSettingInstrumentWiseViewModel> reqList)
        {
            try
            {
                BaseResponse<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>> response = new BaseResponse<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>>();
                response.ResponseCode = (int)HttpStatusCode.Created;

                List<IBCommisionMarkUpSettingInstrumentWiseViewModel> responseData = new List<IBCommisionMarkUpSettingInstrumentWiseViewModel>();

                foreach (var req in reqList)
                {
                    var result = await _IIBCommisionMarkUpSettingInstWiseService.Upsert(req);
                    if (result.Code != 200)
                    {
                        throw new InvalidDataException(result.Message);
                    }
                    else
                    {
                        IBCommisionMarkUpSettingInstrumentWiseViewModel data = new()
                        {
                            Id = result.Id
                        };
                        responseData.Add(data);
                    }

                }

                response.ResponseData = responseData;
                response.ResponseMessage = string.Format(General.AccountCreated, "IBCommisionMarkUpSettingInstrumentWise");

                return StatusCode(response.ResponseCode, response);
            }
            catch (InvalidDataException e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}
