using Epidi.API.Common;
using Epidi.Models.Model;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Models.ViewModel.User;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.ApplicationUserRepository;
using Epidi.Service.ApplicationUserService;
using Epidi.Service.TermsConditionService;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using static IdentityModel.OidcConstants;
using Epidi.Repository;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Epidi.Notifications.Interface;
using Epidi.Models.ExceptionHelper;
using System.Text.RegularExpressions;
using Epidi.Service.Helpers;

namespace Epidi.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;

        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMessages _messages;
        private StraalConfigModel StraalConfigModel { get; set; }

        public TelemetryClient client = new TelemetryClient();

        public AccountController(IApplicationUserService applicationUserService, IOptions<StraalConfigModel> StraalConfigOption, IApplicationUserRepository applicationUserRepository, IMessages messages)
        {
            _applicationUserService = applicationUserService;

            this.StraalConfigModel = StraalConfigOption.Value;
            _applicationUserRepository = applicationUserRepository;
            _messages = messages;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Post([FromBody] RegisterUserViewModel req)
        {
            //try
            //{
            client.TrackTrace(JsonConvert.SerializeObject(req));

            BaseResponse<ApplicationUserDetail> response = new BaseResponse<ApplicationUserDetail>();

            string url = StraalConfigModel.BaseUrl + "customers";
            CreateCustomerViewModel createCustomerViewModel = new CreateCustomerViewModel();
            createCustomerViewModel.email = req.Email;
            createCustomerViewModel.reference = "EPIDI_" + new Random().Next(1000, 9999).ToString();
            var stringContent = new StringContent(JsonConvert.SerializeObject(createCustomerViewModel), Encoding.UTF8, "application/json");
            var resp = await APIService.PostAPIWithToken(url, stringContent, StraalConfigModel.Token);
            client.TrackTrace(JsonConvert.SerializeObject(resp));

            if (resp.ResponseData.id != null)
            {
                string CustomerId = resp.ResponseData.id.ToString();
                string EmailId = resp.ResponseData.email.ToString();
                client.TrackTrace("CustomerId: " + CustomerId);
                if (req.Email == null || req.Email == null)
                {
                    response.ResponseCode = (int)HttpStatusCode.Created;
                    response.ResponseMessage = "Please enter email";
                    response.ErrorCode = (int)HttpStatusCode.Forbidden;
                    return StatusCode(response.ResponseCode, response);
                }
                if (req.CountryId == 0 || req.CountryId == null)
                {
                    response.ResponseCode = (int)HttpStatusCode.Created;
                    response.ResponseMessage = "Please select country";
                    response.ErrorCode = (int)HttpStatusCode.Forbidden;
                    return StatusCode(response.ResponseCode, response);
                }
                if (req.Role == 0 || req.Role == null)
                {
                    response.ResponseCode = (int)HttpStatusCode.Created;
                    response.ResponseMessage = "Please enter Role";
                    response.ErrorCode = (int)HttpStatusCode.Forbidden;
                    return StatusCode(response.ResponseCode, response);
                }
                // Promo code
                //if (req.PromoCode == "" || req.PromoCode == null || req.PromoCode == "string")
                //{
                //    response.ResponseCode = (int)HttpStatusCode.BadRequest;
                //    response.ResponseMessage = "Please Enter PromoCode";
                //    return StatusCode(response.ResponseCode, response);
                //}
                // Password
                if (req.SocialId == null || req.SocialId == "" || req.SocialType == null || req.SocialType == "")
                {

                    if (req.Password == null || req.Password == "" || req.ReEnterPassword == null || req.ReEnterPassword == "")
                    {
                        response.ResponseCode = (int)HttpStatusCode.Created;
                        response.ResponseMessage = "Please Enter Password and Confirm Password";
                        response.ErrorCode = (int)HttpStatusCode.Forbidden;
                        response.ErrorType = 2;
                        return StatusCode(response.ResponseCode, response);
                    }
                    else if (req.Password != req.ReEnterPassword)
                    {
                        response.ResponseCode = (int)HttpStatusCode.Created;
                        response.ResponseMessage = "Your ReEnter password must be different";
                        response.ErrorCode = (int)HttpStatusCode.Forbidden;
                        response.ErrorType = 2;
                        return StatusCode(response.ResponseCode, response);
                    }
                    else
                    {

                        string errorMessagePassword = "";
                        //string errorMessagePromoCode = "";
                        bool Validationresult = ValidatePassword(req.Password, out errorMessagePassword);
                        //bool Validationresultpromo = ValidatePromoCode(req.PromoCode, out errorMessagePromoCode);

                        if (Validationresult == false)
                        {
                            response.ResponseCode = (int)HttpStatusCode.Created;
                            response.ResponseMessage = errorMessagePassword;
                            response.ErrorCode = (int)HttpStatusCode.Forbidden;
                            response.ErrorType = 2;
                            return StatusCode(response.ResponseCode, response);
                        }
                        //else if(Validationresultpromo == false)
                        //{
                        //    response.ResponseCode = (int)HttpStatusCode.BadRequest;
                        //    response.ResponseMessage = errorMessagePromoCode;
                        //    return StatusCode(response.ResponseCode, response);
                        //}
                    }
                }
                bool duplicateUser = await _applicationUserService.DuplicateUser(req.Email);
                if (duplicateUser)
                {
                    response.ResponseCode = (int)HttpStatusCode.Created;
                    response.ResponseMessage = "Great News, The account Already exists! If you do not rember the password reset it from the Sign-In section";
                    response.ErrorCode = (int)HttpStatusCode.Forbidden;
                    response.ErrorType = 1;
                    return StatusCode(response.ResponseCode, response);
                }
                response.ResponseData = await _applicationUserService.Create(req, CustomerId);
                string strBody = GetRegistrationEmailBody(EmailId);
                string result = await _messages.SendEmailViaSendGrid(EmailId, "Registration", strBody);
                client.TrackTrace("Created Response: " + JsonConvert.SerializeObject(resp.ResponseData));
                response.ResponseCode = (int)HttpStatusCode.Created;
                response.ResponseMessage = string.Format(General.AccountCreated, req.Email);
                client.TrackTrace(response.ResponseCode.ToString() + "  " + response.ResponseMessage);
                return StatusCode(response.ResponseCode, response);
            }
            else
            {
                response.ResponseCode = (int)HttpStatusCode.Created;
                response.ResponseMessage = "Please Enter Email";
                response.ErrorCode = (int)HttpStatusCode.Forbidden;
                response.ErrorType = 1;
                return StatusCode(response.ResponseCode, response);
            }
            //}
            //catch(Exception ex)
            //{
            //    client.TrackException(ex);
            //    return StatusCode(500, "Server Error");

            //}

        }


        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Put([FromBody] RegisterUserViewModel req)
        {
            BaseResponse<ApplicationUserDetail> response = new BaseResponse<ApplicationUserDetail>();

            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _applicationUserService.Create(req, "");
            response.ResponseMessage = string.Format(General.AccountCreated, req.Email);
            return StatusCode(response.ResponseCode, response);
        }


        //public async Task<ActionResult> SendEmail(string Email, string verificationCode)
        //{

        //    BaseResponse<ApplicationUserDetail> response = new BaseResponse<ApplicationUserDetail>();
        //    try
        //    {
        //        response.ResponseCode = (int)HttpStatusCode.Created;
        //        response.ResponseData = await _applicationUserService.ConfirmUser(Email, verificationCode);
        //        response.ResponseMessage = string.Format(General.Success, Email);
        //        if (response.ResponseData.Id == 0)
        //        {
        //            response.ResponseData = null;
        //            response.ResponseCode = (int)HttpStatusCode.Forbidden;
        //            response.ResponseMessage = General.OTPNotMatched;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return StatusCode(response.ResponseCode, response);
        //}

        [HttpGet("confirm")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SendEmail(string Email, string verificationCode)
        {
            BaseResponse<ApplicationUserDetail> response = new BaseResponse<ApplicationUserDetail>();
            response.ResponseData = await _applicationUserService.ConfirmUser(Email, verificationCode);
            response.ResponseMessage = string.Format(General.Success, Email);
            response.ResponseCode = (int)HttpStatusCode.Created;
            if (response.ResponseData.Id == 0)
            {
                response.ErrorCode = (int)HttpStatusCode.Forbidden;
                response.ResponseMessage = string.Format(General.OTPNotMatched);
            }
            else
            {
                response.ResponseMessage = string.Format(General.Success);
            }
            return StatusCode(response.ResponseCode, response);
        }



        [HttpPost("uploaddocument")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UploadDoc(long UserId, [FromForm] string Document)
        {
            BaseResponse<ApplicationUserDetail> response = new BaseResponse<ApplicationUserDetail>();

            response.ResponseCode = (int)HttpStatusCode.Created;
            //response.ResponseData = await _applicationUserService.ConfirmUser(Email, verificationCode);
            response.ResponseMessage = string.Format(General.Success, "");
            return StatusCode(response.ResponseCode, response);
        }


        [HttpPost("createcard")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateCard([FromBody] CreateCardViewModel req)
        {
            APIResponseModel responseModel = new APIResponseModel();
            var customerId = await _applicationUserService.GetUsersCustomerId(req.userId);
            if (customerId != null)
            {
                string url = StraalConfigModel.BaseUrl + "customers/" + customerId + "/cards";
                var stringContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                responseModel = await APIService.PostAPIWithToken(url, stringContent, StraalConfigModel.Token);
                return StatusCode(responseModel.ResponseCode, responseModel);
            }
            else
            {
                responseModel.ResponseCode = (int)HttpStatusCode.NotFound;
                responseModel.ResponseMessage = "Customer is not found";
                return StatusCode(responseModel.ResponseCode, responseModel);
            }

        }

        [Common.Authorize]
        [HttpGet("getallcard")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllCard(int userId)
        {
            APIResponseModel responseModel = new APIResponseModel();
            var customerId = await _applicationUserService.GetUsersCustomerId(userId);
            if (customerId != null)
            {
                string url = StraalConfigModel.BaseUrl + "customers/" + customerId + "/cards";
                responseModel = await APIService.GetAPIWithToken(url, StraalConfigModel.Token);
                return StatusCode(responseModel.ResponseCode, responseModel);
            }
            else
            {
                responseModel.ResponseCode = (int)HttpStatusCode.NotFound;
                responseModel.ResponseMessage = "Customer is not found";
                return StatusCode(responseModel.ResponseCode, responseModel);
            }
        }

        [HttpPost("disablecard")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DesableCard([FromBody] DisableCardViewModel req)
        {
            APIResponseModel responseModel = new APIResponseModel();

            string url = StraalConfigModel.BaseUrl + "cards/" + req.cardid + "/disable";
            var stringContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
            responseModel = await APIService.PostAPIWithToken(url, stringContent, StraalConfigModel.Token);
            return StatusCode(responseModel.ResponseCode, responseModel);

        }

        [HttpPost("createtransaction")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateTransaction([FromBody] CreateTransactionViewModel req)
        {
            APIResponseModel responseModel = new APIResponseModel();
            string url = StraalConfigModel.BaseUrl + "cards/" + req.cardid + "/transactions";
            var stringContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
            responseModel = await APIService.PostAPIWithToken(url, stringContent, StraalConfigModel.Token);
            return StatusCode(responseModel.ResponseCode, responseModel);
        }

        [HttpGet("getalltransaction")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllTransaction(string cardId)
        {
            APIResponseModel responseModel = new APIResponseModel();
            string url = StraalConfigModel.BaseUrl + "cards/" + cardId + "/transactions";
            responseModel = await APIService.GetAPIWithToken(url, StraalConfigModel.Token);
            return StatusCode(responseModel.ResponseCode, responseModel);
        }

        [HttpPost("createpaymentlink")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreatePaymentLink([FromBody] CreatePaymentLinkViewModel req)
        {
            APIResponseModel responseModel = new APIResponseModel();
            var customerId = await _applicationUserService.GetUsersCustomerId(req.userId);
            if (customerId != null)
            {
                string url = StraalConfigModel.BaseUrl + "customers/" + customerId + "/pay_by_link_payments";
                var stringContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                responseModel = await APIService.PostAPIWithToken(url, stringContent, StraalConfigModel.Token);
                return StatusCode(responseModel.ResponseCode, responseModel.ResponseData);
            }
            else
            {
                responseModel.ResponseCode = (int)HttpStatusCode.NotFound;
                responseModel.ResponseMessage = "Customer is not found";
                return StatusCode(responseModel.ResponseCode, responseModel);
            }

        }

        [HttpGet("getcards")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCards()
        {
            APIResponseModel responseModel = new APIResponseModel();
            string url = StraalConfigModel.BaseUrl + "cards";
            responseModel = await APIService.GetAPIWithToken(url, StraalConfigModel.Token);
            return StatusCode(responseModel.ResponseCode, responseModel);
        }

        [HttpPost("refund")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Refund([FromBody] TransactionReFundViewModel req)
        {
            APIResponseModel responseModel = new APIResponseModel();
            string url = StraalConfigModel.BaseUrl + "transactions/" + req.extra_data.cardid + "/refund";
            var stringContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
            responseModel = await APIService.PostAPIWithToken(url, stringContent, StraalConfigModel.Token);
            return StatusCode(responseModel.ResponseCode, responseModel);
        }


        [HttpPost("capturepayment")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CapturePayment([FromBody] TransactionReFundViewModel req)
        {
            APIResponseModel responseModel = new APIResponseModel();
            string url = StraalConfigModel.BaseUrl + "transactions/" + req.extra_data.cardid + "/capture";
            var stringContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
            responseModel = await APIService.PostAPIWithToken(url, stringContent, StraalConfigModel.Token);
            return StatusCode(responseModel.ResponseCode, responseModel);
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] AppUser model)
        {
            var response = await _applicationUserService.Login(model);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpPost("termsconditionagreed")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> TermsConditionAgreed(int userId)
        {
            APIResponseModel responseModel = new APIResponseModel();

            responseModel.ResponseCode = (int)HttpStatusCode.Created;
            responseModel.ResponseData = await _applicationUserService.UpdateTermsCondition(userId);
            responseModel.ResponseMessage = string.Format(General.Success, userId);
            return StatusCode(responseModel.ResponseCode, responseModel);
        }

        [HttpPost("termsconditionstepagreed")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ApplicationUserDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> TermsConditionStepAgreed(int userId)
        {
            APIResponseModel responseModel = new APIResponseModel();

            responseModel.ResponseCode = (int)HttpStatusCode.Created;
            responseModel.ResponseData = await _applicationUserService.UpdateTermsStepCondition(userId);
            responseModel.ResponseMessage = string.Format(General.Success, userId);
            return StatusCode(responseModel.ResponseCode, responseModel);
        }

        [HttpPost("forgotpassword")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ForgotPasswordViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> ForgotPassword(string Email)
        {
            APIResponseModel responseModel = new APIResponseModel();
            if (Email != "")
            {
                responseModel.ResponseCode = (int)HttpStatusCode.OK;
                responseModel.ResponseData = await _applicationUserService.ForgotPassword(Email);
                if (responseModel.ResponseData != null && responseModel.ResponseData != "")
                {
                    //send email of verification code
                    string strBody = GetForgotPasswordEmailBody(Email, responseModel.ResponseData);
                    string result = await _messages.SendEmailViaSendGrid(Email, "Forgot password Otp", strBody);
                    responseModel.ResponseData = result;
                }
                else
                {
                    responseModel.ResponseData = "Email not found.";
                }
                responseModel.ResponseMessage = string.Format(General.Success);
            }
            else
            {
                responseModel.ResponseMessage = string.Format(General.PasswordMismatch);
            }

            return Ok(responseModel);
        }
        [HttpPost("Changepassword")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ForgotPasswordViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> ChangePassword([FromBody] ForgotPasswordViewModel req)
        {
            APIResponseModel responseModel = new APIResponseModel();
            if (req.Password == req.ReEnterPassword)
            {
                responseModel.ResponseData = "";
                responseModel.ResponseCode = (int)HttpStatusCode.OK;
                responseModel.ResponseMessage = await _applicationUserService.ResetPassword(req.UserId, CommonFunction.EncodePasswordToBase64(req.Password), CommonFunction.EncodePasswordToBase64(req.OldPassword));
            }
            else
            {
                throw new EntityNotFoundException(General.PasswordMismatch, "", HttpStatusCode.Unauthorized.ToString(), General.Invalid);
            }

            return Ok(responseModel);
        }

        // by niti ChangePasswordByEmailId
        [HttpPost("ChangePasswordByEmailId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<ChangePasswordByEmailIdViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> ChangePasswordByEmailId(ChangePasswordByEmailIdViewModel changepass)
        {
            APIResponseModel responseModel = new APIResponseModel();
            try
            {
                if (changepass.Email != "")
                {
                    if (changepass.Password != changepass.ReEnterPassword)
                    {
                        responseModel.ResponseCode = (int)HttpStatusCode.BadRequest;
                        responseModel.ResponseMessage = "Please Enter Password and Confirm Password same";
                        return StatusCode(responseModel.ResponseCode, responseModel);
                    }

                    string errorMessagePassword = "";
                    bool Validationresult = ValidatePassword(changepass.Password, out errorMessagePassword);
                    if (Validationresult == false)
                    {
                        responseModel.ResponseCode = (int)HttpStatusCode.BadRequest;
                        responseModel.ResponseMessage = errorMessagePassword;
                        return StatusCode(responseModel.ResponseCode, responseModel);
                    }

                    responseModel.ResponseData = await _applicationUserService.ChangePasswordByEmail(changepass.Email, changepass.Password);
                    if (responseModel.ResponseData != null)
                    {
                        //send email of verification code
                        string strBody = GetChangePasswordByEmailBody(changepass.Email);
                        string result = await _messages.SendEmailViaSendGrid(changepass.Email, "Change Password Successfully", strBody);
                        responseModel.ResponseData = result;
                        responseModel.ResponseCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        responseModel.ResponseData = "Email not found.";
                    }
                    responseModel.ResponseMessage = string.Format(General.Success);
                }
                else
                {
                    responseModel.ResponseMessage = string.Format(General.PasswordMismatch);
                }
            }
            catch (Exception ex)
            {
                responseModel.ResponseCode = (int)HttpStatusCode.InternalServerError;
                responseModel.ResponseMessage = ex.Message;
                return StatusCode(responseModel.ResponseCode, responseModel);
            }

            return Ok(responseModel);
        }


        [HttpPost("savepersonalinformation")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<PersonalInfomation>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SavePersonalInformation([FromBody] PersonalInfomation req)
        {
            APIResponseModel responseModel = new APIResponseModel();
            if (req.Id == 0)
            {
                responseModel.ResponseCode = (int)HttpStatusCode.OK;
                responseModel.ResponseData = await _applicationUserService.PersonalInformationCreate(req);
                responseModel.ResponseMessage = string.Format(General.Inserted);
                return StatusCode(responseModel.ResponseCode, responseModel);
            }
            else
            {
                responseModel.ResponseCode = (int)HttpStatusCode.OK;
                responseModel.ResponseData = await _applicationUserService.PersonalInformationUpdate(req);
                responseModel.ResponseMessage = string.Format(General.Updated);
                return StatusCode(responseModel.ResponseCode, responseModel);
            }
        }

        private string GetRegistrationEmailBody(string ToEmail)
        {
            string strHTML = "";
            try
            {

                string filename = @"WelComeEmailTemplate.html";
                strHTML = System.IO.File.ReadAllText(filename);
                strHTML = strHTML.Replace("[First Name]", ToEmail);
            }
            catch (Exception ex)
            {

                throw;
            }

            return strHTML;
        }
        private string GetForgotPasswordEmailBody(string ToEmail, string VerificationCode)
        {
            string strHTML = "";
            string filename = @"ForgotPassword.html";
            strHTML = System.IO.File.ReadAllText(filename);
            strHTML = strHTML.Replace("[First Name]", ToEmail);
            strHTML = strHTML.Replace("[code]", VerificationCode);
            return strHTML;
        }

        private string GetChangePasswordByEmailBody(string ToEmail)
        {
            string strHTML = "";
            string filename = @"ChangePassword.html";
            strHTML = System.IO.File.ReadAllText(filename);
            strHTML = strHTML.Replace("[First Name]", ToEmail);
            return strHTML;
        }

        // Get UserProfile (Code by NITI) strat
        [AllowAnonymous]
        [HttpGet("GetUserProfile/{UserId}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<UsersViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserProfile(int UserId)
        {
            BaseResponse<UsersViewModel> response = new BaseResponse<UsersViewModel>();
            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _applicationUserService.GetUserProfile(UserId);
            response.ResponseMessage = string.Format(General.Success, "UsersFavourite");
            return StatusCode(response.ResponseCode, response);
        }


        // Get UserPromocodeByUserId (Code by NITI) strat
        [AllowAnonymous]
        [HttpGet("GetUserPromocodeByUserId/{UserId}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<Userpromocodemodel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserPromocodeByUserId(int UserId)
        {
            BaseResponse<Userpromocodemodel> response = new BaseResponse<Userpromocodemodel>();
            response.ResponseCode = (int)HttpStatusCode.OK;
            //response.ResponseData = await _applicationUserService.GetUserProfile(UserId);
            response.ResponseData = await _applicationUserService.GetUserPromocodeByUserId(UserId);
            response.ResponseMessage = string.Format(General.Success, "UsersFavourite");
            return StatusCode(response.ResponseCode, response);
        }



        private bool ValidatePassword(string password, out string ErrorMessage)
        {
            bool restulval = true;
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (input.Length >= 8 && input.Length <= 15)
            {
                restulval = true;
            }
            else
            {
                ErrorMessage = "Password is minimum is 8 and maximum 15 characters.";
                restulval = false;
            }


            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one lower case letter.";
                restulval = false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one upper case letter.";
                restulval = false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password should not be lesser than 8 or greater than 15 characters.";
                restulval = false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one numeric value.";
                restulval = false;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one special case character.";
                restulval = false;
            }
            return restulval;
        }

        private bool ValidatePromoCode(string promocode, out string ErrorMessage)
            {
            var proinput = promocode;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(proinput))
            {
                throw new Exception("Promocode should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasChars = new Regex(@"[A-Za-z]");
            var hasMiniMaxChars = new Regex(@"[?=.{6,8}$]");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");


            if (hasNumber.IsMatch(proinput) || hasSymbols.IsMatch(proinput))
            {
                ErrorMessage = "Promocode should only characters.";
                return false;
            }
            
            if (promocode.Length >= 6 && promocode.Length <= 8)
            {
                return true;
                
            }
            else
            {
                ErrorMessage = "Promocode is minimum is 6 and maximum 8 characters allow.";
                return false;
            }
        }
    }
}
