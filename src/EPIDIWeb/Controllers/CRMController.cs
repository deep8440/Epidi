using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Models.ViewModel.Users;
using Epidi.Models.ViewModel.UsersDocuments;
using Epidi.Repository.ApplicationUserRepository;
using Epidi.Service.ApplicationUserService;
using Epidi.Service.BDM;
using Epidi.Service.CountryService;
using Epidi.Service.CRMService;
using Epidi.Service.IBService;
using Epidi.Service.LegalEntityService;
using Epidi.Service.UsersDocumentsService;
using Epidi.Service.PayInoutRequestService;
using Epidi.Service.UsersService;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Ocsp;
using System.Web.WebPages.Html;
using NPOI.HPSF;
using Org.BouncyCastle.Ocsp;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.AspNetCore.Http;
using Epidi.Service.Helpers;
using Epidi.Models.ViewModel.Question;
using Epidi.Service.AnswerService;

namespace EPIDIWeb.Controllers
{
    public class CRMController : Controller
    {
        private readonly ICRMService _crmService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IBDMService _BDMService;
        private readonly ILegalEntityService _LegalEntityService;
        private readonly ICountryService _countryService;
        private readonly IIBService _IBServices;
        private readonly IUsersService _usersService;
        private readonly IPayInoutRequestService _payInoutRequestService;
		private readonly IUsersDocumentsService _usersDocumentsService;
        private readonly IAnswerService _answerService;
        private IConfiguration Configuration;
		public CRMController(ICRMService crmService, 
			IApplicationUserService applicationUserService, 
			IIBService iBServices, IBDMService bDMService, 
			ILegalEntityService legalEntityService, 
			ICountryService countryService, 
			IUsersService usersService, 
			IPayInoutRequestService payInoutRequestService,
			IUsersDocumentsService usersDocumentsService,
            IAnswerService answerService,
            IConfiguration configuration)
        {
            _crmService = crmService;
            _applicationUserService = applicationUserService;
            _IBServices = iBServices;
            _BDMService = bDMService;
            _LegalEntityService = legalEntityService;
            _countryService = countryService;
            _usersService = usersService;
            _payInoutRequestService = payInoutRequestService;
			_usersDocumentsService = usersDocumentsService;
            _answerService = answerService;
            Configuration = configuration;
		}
        public IActionResult Index()
        {

            ViewBag.Title = "CRM List";
            ViewBag.Method = "CRM";
            return View();
        }

        [HttpPost]
        public JsonResult Users_All()
        {

            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();

            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _crmService.Users_GetAll(pageParam, search);

            //totalRecord = response.Item2;
            //filteredRecord = response.Item2;

            totalRecord = response.Item1.Count;
            filteredRecord = response.Item1.Count;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });

        }
        public async Task<IActionResult> EditCRM(int Id)
        {
            var response = await _applicationUserService.GetUserProfile(Id);
            var res = await _usersDocumentsService.GetByUserId(Id);
            response.UsersDocumentViewModels = res;
			//response.FieldName =
			GetBDMList();
            GetLegalEntityListALL();
            GetCountryListAll();
            GetIBList();
            return PartialView("EditCRM", response);
        }
        public void GetBDMList()
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 1;
            pageParam.Limit = 100;
            var response = _BDMService.GetAll(pageParam, "");

            var FieldsList = response.Item1.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });

            ViewBag.BDMDropDown = FieldsList;
        }
        public void GetIBList()
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _IBServices.GetAll(pageParam, "");
            var FieldsList = response.Item1.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });
            ViewBag.IBDropDown = FieldsList;
        }
        public void GetLegalEntityListALL()
        {
            var DropdownlistLegalEntity = _LegalEntityService.GetLegalEntityListALL().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.DropdownlistLegalEntity = DropdownlistLegalEntity;
        }

        public void GetCountryListAll()
        {
            var DropdownlistCountry = _countryService.GetCountryListAll().Result.Select(p => new SelectListItem() { Value = p.CountryId.ToString(), Text = p.CountryName });

            ViewBag.DropdownlistCountry = DropdownlistCountry;
        }

		[HttpPost]
		public async Task<IActionResult> UploadDocument(IFormFile FileUpload, int UserId)
		{
			List<UsersDocumentViewModel> FileList = new List<UsersDocumentViewModel>();
			//FileList = _termsConditionService.GetTermsConditionFilesById(Id);
			if (FileUpload != null)
			{
				UsersDocumentViewModel tCMultipleFilViewModel = new UsersDocumentViewModel();

				//var supportedTypes = new[] { "pdf" };
				//var fileExt = Path.GetExtension(FileUpload.FileName);
				//fileExt = fileExt.Replace(".", "");
				////string newFilename = FileUpload.FileName + "." + fileExt;
				//var folderPath = this.Configuration.GetSection("Path")["DocumentFilePath"];
				//var filePath = Path.Combine(folderPath, FileUpload.FileName.ToString());
				//if (!Directory.Exists(folderPath))
				//{
				//	Directory.CreateDirectory(folderPath);
				//}


				var accountName = this.Configuration.GetSection("DocumentUploadCred")["BlobAccountName"];
				var accountKey = this.Configuration.GetSection("DocumentUploadCred")["BlobAccountKey"];
				var containerName = this.Configuration.GetSection("DocumentUploadCred")["Blobcontainername"];
				var fileUrl = this.Configuration.GetSection("DocumentUploadCred")["FileUrl"];
				var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
				var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
				var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
				var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(FileUpload.FileName.ToString());
				
				await using var memoryStream = new MemoryStream();
				await FileUpload.CopyToAsync(memoryStream);
				var streamBytes = memoryStream.ToArray();

				using (Stream stream = new MemoryStream(streamBytes))
				{
					await cloudBlockBlob.UploadFromStreamAsync(stream);
				}


				//using (var stream = System.IO.File.Create(filePath))
				//{
				//	FileUpload.CopyTo(stream);
				//}
				tCMultipleFilViewModel.DocumentName = FileUpload.FileName.ToString();
				tCMultipleFilViewModel.UserId = UserId;
				tCMultipleFilViewModel.DocumentPath = fileUrl+ FileUpload.FileName.ToString();
				tCMultipleFilViewModel.DocumentType = "Front";
				tCMultipleFilViewModel.CreatedBy = 1;


				var res = _usersDocumentsService.Upsert(tCMultipleFilViewModel).Result;
				if (res != null)
				{
					FileList.Add(res);
					return PartialView("CRMMultiFile", FileList);
				}
				else
				{
					return PartialView("CRMMultiFile", new List<UsersDocumentViewModel>());
				}

			}
			else
			{
				return PartialView("CRMMultiFile", FileList);
			}
		}


		public ActionResult DownloadDocument(string FileName)
		{
			var folderPath = this.Configuration.GetSection("Path")["DocumentFilePath"];
			var filePath = Path.Combine(folderPath, FileName.ToString());

			byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

			return File(fileBytes, "application/force-download", FileName);

		}

		[HttpPost]
		public IActionResult RemoveDocument(string FileName, int Id, int UserId)
		{
			if (FileName != null)
			{
				//var folderPath = this.Configuration.GetSection("Path")["DocumentFilePath"];
				//var filePath = Path.Combine(folderPath, FileName.ToString());
				//if (System.IO.File.Exists(filePath))
				//{
				//	System.IO.File.Delete(filePath);
				//}

				var accountName = this.Configuration.GetSection("DocumentUploadCred")["BlobAccountName"];
				var accountKey = this.Configuration.GetSection("DocumentUploadCred")["BlobAccountKey"];
				var containerName = this.Configuration.GetSection("DocumentUploadCred")["Blobcontainername"];
				var fileUrl = this.Configuration.GetSection("DocumentUploadCred")["FileUrl"];
				var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
				var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
				var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
				var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(FileName);

                cloudBlockBlob.DeleteIfExistsAsync();

			}
			if (Id > 0)
			{
				var res = _usersDocumentsService.DeleteDocument(Id, UserId).Result;
			}
			return Json(new { Data = "", StatusCode = 200, Message = "File removed successfully." });
		}


		[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveUser(UsersViewModel model)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();
            if (model != null)
            {
                var res = await _usersService.Upsert(model);
                if (res != null)
                {
                    responseViewModel.Code = 200;
                    responseViewModel.Message = "User Successfully";
                }
                else
                {
                    responseViewModel.Code = 400;
                    responseViewModel.Message = "User Saved";
                }
            }
            return Json(responseViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> ViewPaymentRequestListByUser(int Id)
        {
            ViewBag.PageTitle = "View Payment Request List By User";
            ViewBag.Method = "CRM";

			ViewBag.Id = Id;
			return View("PaymentRequestList");
        }

        [HttpPost]
        public async Task<JsonResult> GetPaymentRequestListByUserId(int Id)
        {

            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();

            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = await _usersService.GetPaymentRequestListByUserId(pageParam, search,Id);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });

        }

        [HttpGet]
        public async Task<IActionResult> ViewQuestionAnswerListByUser(int UserId)
        {
            ViewBag.PageTitle = "View Question Answer List By User";
            ViewBag.Method = "CRM";

            var response = await _usersService.GetQuestionAnswerListByUserId(UserId);



            if (response != null)
            {
                ViewBag.Id = UserId;
                return View("QuestionAnswerList", response);
            }
            else
            {
                return View();
            }


        }


        public IActionResult AddDepositMoney(int Id, string name)
		{
            GetRequestTypeListALL();
            GetRequestStatusListALL();

			PayoutRequest payoutRequest = new PayoutRequest();
            if(name == "Deposite")
            {
				payoutRequest.RequestBy = Id;
				payoutRequest.IsDelete = false;
				payoutRequest.RequestType = "1";
				payoutRequest.Id = 0;
                ViewBag.MethodName = "Add " + name + " Money";
				return View(payoutRequest);
			}
            if(name == "Withdrawal")
            {
				payoutRequest.RequestBy = Id;
				payoutRequest.IsDelete = false;
				payoutRequest.RequestType = "2";
				payoutRequest.Id = 0;
				ViewBag.MethodName = "Add " + name + " Money";
				return View(payoutRequest);
			}
			return View(payoutRequest);
		}

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddDepositMoney(PayoutRequest model)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();
            if (model != null)
            {
                var res = await _usersService.DepositMoneyInsert(model);
                if (res != null)
                {
					return RedirectToAction("ViewPaymentRequestListByUser", "CRM", new { @id = res.RequestBy });
					responseViewModel.Code = 200;
                    responseViewModel.Message = "Deposite Add Successfully";
                }
                else
                {
                    responseViewModel.Code = 400;
                    responseViewModel.Message = "Deposite Not Save Please try again....";
                }
            }
            return Json(responseViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UpdateAnswer(QuestionAnswerList questionAnswerReq)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();
            if (questionAnswerReq != null)
            {
                var res = await _usersService.UpdateAnswer(questionAnswerReq);
                if (res != null && res.Id==1)
                {
                    responseViewModel.Code = 200;
                    responseViewModel.Message = "Answer Updated Successfully";
                }
                else
                {
                    responseViewModel.Code = 400;
                    responseViewModel.Message = "Error in Saving";
                }
            }
            return Json(responseViewModel);
        }
        public void GetRequestStatusListALL()
        {
            var DropdownlistRequestStatus = _payInoutRequestService.GetRequestStatusListALL().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Status });

            ViewBag.DropdownlistRequestStatus = DropdownlistRequestStatus;
        }

        public void GetRequestTypeListALL()
        {
            var DropdownlistRequestType = _payInoutRequestService.GetRequestTypeListALL().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.DropdownlistRequestType = DropdownlistRequestType;
        }

        public async Task<IActionResult> GetAnswerByQuestionId(int QuestionId)
        {
            var resp = await _answerService.GetAnswerByQuestionId(QuestionId);

            return Json(resp);
        }

        public async Task<IActionResult> GetAnswerByAnswerId(string answerIds)
        {
            List<AnswerViewModel> answerViewModels = new List<AnswerViewModel>();
            if (answerIds != null)
            {
                if (answerIds.Contains("|"))
                {
                    string[] answerIdArr = answerIds.Split("|");

                    foreach (var answerId in answerIdArr)
                    {
                        var resp = await _answerService.GetAnswerByAnswerId(Convert.ToInt64(answerId));
                        answerViewModels.Add(resp);
                    }
                }
                else
                {
                    var resp = await _answerService.GetAnswerByAnswerId(Convert.ToInt64(answerIds));
                    answerViewModels.Add(resp);
                }
            }
            return Json(answerViewModels);
        }

        //public void GetCountry()
        //{
        //	var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName });

        //	ViewBag.CountryList = country;
        //}


        //[HttpGet]
        //public async Task<IActionResult> Edit(int UserId)
        //{
        //	//var questions = await _questionService.GetQuestionByQuestionId(UserId);
        //	var answers = await _usersService.GetQuestionAnswerListByUserId(UserId);

        //	if (answers.Count > 0)
        //		questions.Answers = answers;
        //	else
        //		questions.Answers.Add(new AnswerViewModel() { AnswerDesc = "" });

        //	GetCountryListAll();
        //	GetAnsType();
        //	Steps_GetByCountryId(Convert.ToInt32(questions.CountryId));
        //	questions.StepId = Convert.ToInt32(questions.StepId);   
        //	if (questions != null)
        //	{
        //		return View("AddQuestion");
        //	}
        //	else
        //	{
        //		return View(new QuestionViewModel());
        //	}
        //}
    }

}
