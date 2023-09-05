using Epidi.API.Common;
using Epidi.Models.Paging;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Service.InstrumentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.Instrument
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InstrumentController : ControllerBase
    {
        private readonly IInstrumentService _instrumentService;
        public InstrumentController(IInstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
        }
        //public async Task<IActionResult> FavouriteInstrument(FavouriteInstrument favouriteInstrument)
        //{
        //    var response = await _instrumentService.AddFavourite(favouriteInstrument);
        //    return Ok();
        //}
        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<InstrumentMaster>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Get(string SearchKeyword,int UserId)
        {
            BaseResponse<List<InstrumentMaster>> response = new BaseResponse<List<InstrumentMaster>>();
            PageParam pageParam = new()
            {
                Offset = 0,
                Limit = 100
            };
            var res = await _instrumentService.GetAllInstrumentByUserId(pageParam, SearchKeyword, UserId);
            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = res.Item1;
            response.TotalRecords = response.ResponseData.Count;
            response.ResponseMessage = string.Format(General.Success);
            return StatusCode(response.ResponseCode, response);
        }
        
        [AllowAnonymous]
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<InstrumentMaster>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> InstrumentPopular()
        {
            BaseResponse<List<InstrumentMaster>> response = new BaseResponse<List<InstrumentMaster>>();
           
            var res = await _instrumentService.GetAllInstrumentPopular();
            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = res;
            response.TotalRecords = response.ResponseData.Count;
            response.ResponseMessage = string.Format(General.Success);
            return StatusCode(response.ResponseCode, response);
        }

        [AllowAnonymous]
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<CommissionRuleInstrument>), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> GetInstrumentsWithCommission()
        //{
        //    try
        //    {
        //        BaseResponse<List<CommissionRuleInstrument>> response = new BaseResponse<List<CommissionRuleInstrument>>();

        //        var res = await _instrumentService.GetInstrumentsWithCommission();
        //        response.ResponseCode = (int)HttpStatusCode.OK;
        //        response.ResponseData = res;
        //        response.TotalRecords = response.ResponseData.Count;
        //        response.ResponseMessage = string.Format(General.Success);
        //        return StatusCode(response.ResponseCode, response);
        //    }
        //    catch (Exception ex)
        //    { 
        //        return StatusCode((int)HttpStatusCode.InternalServerError);
        //    }
        //}
        public async Task<ActionResult> GetInstrumentsWithCommission()
        {
            try
            {
                var res = await _instrumentService.GetInstrumentsWithCommission();

                var response = new BaseResponse<List<CommissionRuleInstrument>>
                {
                    ResponseCode = (int)HttpStatusCode.OK,
                    ResponseData = res,
                    TotalRecords = res.Count,
                    ResponseMessage = General.Success
                };

                return StatusCode(response.ResponseCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            //catch (Exception ex)
            //{
            //    return StatusCode((int)HttpStatusCode.InternalServerError);
            //}
        }

        [AllowAnonymous]
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<List<InstrumentWithIBCommision>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInstrumentsWithIBCommission([FromQuery] int IbId)
        {
            try
            {
                var res = await _instrumentService.GetInstrumentsWithIBCommission(IbId);

                var response = new BaseResponse<List<InstrumentWithIBCommision>>
                {
                    ResponseCode = (int)HttpStatusCode.OK,
                    ResponseData = res,
                    TotalRecords = res.Count,
                    ResponseMessage = General.Success
                };

                return StatusCode(response.ResponseCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            //catch (Exception ex)
            //{
            //    return StatusCode((int)HttpStatusCode.InternalServerError);
            //}
        }
    }
}
