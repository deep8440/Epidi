using Epidi.API.Common;
using Epidi.API.Extensions;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.User;
using Epidi.Service.Helpers;
using Epidi.Service.QuestionService;
using Epidi.Service.UserQuestionAnswerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using System.Data;
using System.Net;
using System.Xml.Linq;
using static Epidi.API.Common.BaseResponse;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Epidi.API.Controllers.OnboardingQuestions
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserQuestionAnswersController : ControllerBase
    {
        private readonly IUserQuestionAnswerService _questionAnswerService;
        private IConfiguration Configuration;
        public UserQuestionAnswersController(IUserQuestionAnswerService questionService, IConfiguration configuration)
        {
            _questionAnswerService = questionService;
            Configuration = configuration;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<UserQuestionAnswerViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Post([FromBody] List<UserQuestionAnswerViewModel> req)
        {
            ListToDataTableConverter converter = new ListToDataTableConverter();
            System.Data.DataTable dt = converter.ToDataTable(req);

            BaseResponse<List<UserQuestionAnswerViewModel>> response = new BaseResponse<List<UserQuestionAnswerViewModel>>();

            int StepId = Convert.ToInt32(dt.Rows[0]["StepId"].ToString());
            int countryId = Convert.ToInt32(dt.Rows[0]["countryId"].ToString());
            dt.Columns.Remove("countryId");
            var val = await _questionAnswerService.Create(dt, StepId, countryId);
            if (val > 0)
            {
                response.ResponseCode = (int)HttpStatusCode.OK;
                response.ResponseData = req;
                response.ResponseMessage = string.Format("UserQuestionAnswers created successfully");
                response.TotalRecords = req.Count();
                return StatusCode(response.ResponseCode, response);
            }
            else if (val == 0)
            {
                response.ResponseCode = 400;
                response.ResponseData = req;
                response.ResponseMessage = string.Format("Please give All Questions answares.", General.OperationNotDone);
                return StatusCode(response.ResponseCode, response);
            }
            else
            {
                response.ResponseCode = (int)HttpStatusCode.OK;
                response.ResponseData = req;
                response.ResponseMessage = string.Format("UserQuestionAnswers", General.OperationNotDone);
                return StatusCode(response.ResponseCode, response);
            }
        }
        [HttpPost("UploadFile")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<UserQuestionAnswerViewModel>), (int)HttpStatusCode.OK)]
        
        public async Task<ActionResult> UploadFile([FromForm] UserQuestionAnswerFileRequest req)
        {
            BaseResponse<Response> response = new BaseResponse<Response>();
            
            var item = req.uPloadFiles;
            if(item.files.Length > 0)
            {
				var accountName = this.Configuration.GetSection("DocumentUploadCred")["BlobAccountName"];
				var accountKey = this.Configuration.GetSection("DocumentUploadCred")["BlobAccountKey"];
				var containerName = this.Configuration.GetSection("DocumentUploadCred")["Blobcontainername"];
				var fileUrl = this.Configuration.GetSection("DocumentUploadCred")["FileUrl"];
				var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
				var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
				var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
				var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Convert.ToString(item.files.FileName));

				await using var memoryStream = new MemoryStream();
				await item.files.CopyToAsync(memoryStream);
				var streamBytes = memoryStream.ToArray();

				using (Stream stream = new MemoryStream(streamBytes))
				{
					await cloudBlockBlob.UploadFromStreamAsync(stream);
				}
                string filePath_front = fileUrl + Convert.ToString(item.files.FileName);
				response.ResponseData = await _questionAnswerService.Response(Convert.ToString(item.files.FileName), item.UserId, Convert.ToString(filePath_front));

				if (response.ResponseData.UserId > 0)
				{
					response.ResponseCode = (int)HttpStatusCode.OK;
					response.ResponseMessage = "File Uploaded successfully";
					response.TotalRecords = 1;
					return StatusCode(response.ResponseCode, response);
				}
				else if (response.ResponseData.UserId == 0) 
				{
					response.ResponseCode = 400;
					response.ResponseMessage = "Please give all required answers.";
					return StatusCode(response.ResponseCode, response);
				}
				else
				{
					response.ResponseCode = (int)HttpStatusCode.OK;
					response.ResponseMessage = "File Upload operation not done.";
					return StatusCode(response.ResponseCode, response);
				}

			}
            else
            {
				response.ResponseCode = 400;
				response.ResponseMessage = "Please select file";
				return StatusCode(response.ResponseCode, response);
			}
		
        }

    }
}
