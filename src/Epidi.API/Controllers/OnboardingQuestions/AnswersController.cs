using Epidi.API.Common;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Question;
using Epidi.Service.AnswerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Epidi.API.Controllers.OnboardingQuestions
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        public AnswersController(IAnswerService answerService)
        {
            _answerService = answerService;
        }
        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<AnswerViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(long QuestionId)
        {
            BaseResponse<List<AnswerViewModel>> response = new BaseResponse<List<AnswerViewModel>>();

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _answerService.GetAnswerByQuestionId(QuestionId);
            response.TotalRecords = response.ResponseData.Count;
            response.ResponseMessage = string.Format(General.Success);
            return StatusCode(response.ResponseCode, response);
        }
    }
}
