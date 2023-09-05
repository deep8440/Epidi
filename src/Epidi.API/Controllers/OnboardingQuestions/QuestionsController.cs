using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Question;
using Epidi.Service.QuestionService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Epidi.API.Controllers.OnboardingQuestions
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }    

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<QuestionViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Get(long CountryId, int StepId,int PageNumber)
        {
            BaseResponse<List<QuestionViewModel>> response = new BaseResponse<List<QuestionViewModel>>();

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _questionService.GetQuestionsByCountryIdStepId(CountryId, StepId, true,PageNumber);
            response.TotalRecords = response.ResponseData.Count;
            response.ResponseMessage = string.Format(General.Success);
            return StatusCode(response.ResponseCode, response);
        }

        [HttpGet("QuestionCountByCountryId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<QuestionViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> QuestionCountByCountryId(long CountryId)
        {
            APIResponseModel response = new APIResponseModel();

            if (CountryId == 0)
            {
                response.ResponseCode = (int)HttpStatusCode.BadRequest;
                response.ResponseData = null;
                response.ResponseMessage = string.Format(General.GreaterthenZero);
                return StatusCode(response.ResponseCode, response);
            }
            else
            {
                response.ResponseCode = (int)HttpStatusCode.OK;
                response.ResponseData = await _questionService.GetQuestionCountByCountryId(CountryId);
                response.ResponseMessage = string.Format(General.Success);
                return StatusCode(response.ResponseCode, response);
            }
            
        }
    }
}
