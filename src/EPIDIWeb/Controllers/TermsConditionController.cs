using ClosedXML.Excel;
using DataTables.AspNet.AspNetCore;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Service.ApplicationUserService;
using Epidi.Service.CountryService;
using Epidi.Service.TermsConditionService;
using Epidi.Service.UsersService;
using EPIDIWeb.Extensions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using NPOI.OpenXmlFormats.Dml.Diagram;
using System.Linq;
using NPOI.HPSF;
using Epidi.Models.Extensions;

namespace EPIDIWeb.Controllers
{
    public class TermsConditionController : Controller
    {
        private readonly ITermsConditionService _termsConditionService;
        private readonly ICountryService _countryService;
        private readonly IApplicationUserService _applicationUserService;
        private IConfiguration _Configuration;

        public TermsConditionController(ITermsConditionService termsConditionService,
            ICountryService countryService,
            IApplicationUserService applicationUserService,
            IConfiguration Configuration)
        {
            _termsConditionService = termsConditionService;
            _countryService = countryService;
            _applicationUserService = applicationUserService;
            _Configuration = Configuration;

        }

        public IActionResult Index()
        {
            ViewBag.Title = "Terms And Condition  List";
            return View();
        }
        [HttpPost]
        //public JsonResult GetTermsCondition()
        //{
        //	{
        //		long totalRecord = 0;
        //		long filteredRecord = 0;				
        //		//PageParam pageParam = new PageParam();
        //		//pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
        //		//pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
        //		//string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
        //		var response = _termsConditionService.Get();

        //		return Json(new { data = response });
        //	}
        //}

        [HttpPost]
        public async Task<JsonResult> TermsCondition_All()
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();

                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = await _termsConditionService.Get(pageParam, search);
                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveTermsCondition(TermsConditionViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {
                    var res = await _termsConditionService.Upsert(model);
                    ////Delete Upload file for edit mode
                    var rem = await _termsConditionService.RemovePdfFile(res.Id, "");
                    for (int i = 0; i < model.FileList.Count; i++)
                    {
                        model.FileList[i].TermsConditionId = res.Id;
                        model.FileList[i].CountryId = model.CountryId;
                        model.FileList[i].Title = model.Title;

                        var resp = await _termsConditionService.TermsConditionFile_Upsert(model.FileList[i]);

                    }
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "TermsCondition Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "TermsCondition Not Saved";
                    }
                    return Json(responseViewModel);
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            var termConditions = await _termsConditionService.GetTermsConditionById(Id);
            termConditions.FileList = await _termsConditionService.GetTermsConditionFilesById(Id);
            GetCountry();
            if (termConditions != null)
            {
                return View("AddTermsCondition", termConditions);
            }
            else
            {
                return View(new TermsConditionViewModel());
            }
        }

        [HttpGet]
        public IActionResult DeleteTermsCondition(int Id)
        {
            var res = _termsConditionService.DeleteTermsCondition(Id);
            return Json(res);
        }

        public IActionResult AddTermsCondition()
        {
            GetCountry();
            var model = new TermsConditionViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPdf(List<IFormFile> FileUpload)
        {
            List<TCMultipleFilViewModel> FileList = new List<TCMultipleFilViewModel>();
            if (FileUpload != null)
            {
                var accountName = this._Configuration.GetSection("DocumentUploadCred")["BlobAccountName"];
                var accountKey = this._Configuration.GetSection("DocumentUploadCred")["BlobAccountKey"];
                var containerName = this._Configuration.GetSection("DocumentUploadCred")["Blobcontainername"];
                var fileUrl = this._Configuration.GetSection("DocumentUploadCred")["FileUrl"];
                var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);


                for (int i = 0; i < FileUpload.Count; i++)
                {
                    TCMultipleFilViewModel tCMultipleFilViewModel = new TCMultipleFilViewModel();

                    var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(FileUpload[i].FileName.ToString());

                    await using var memoryStream = new MemoryStream();
                    await FileUpload[i].CopyToAsync(memoryStream);
                    var streamBytes = memoryStream.ToArray();

                    using (Stream stream = new MemoryStream(streamBytes))
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }

                    tCMultipleFilViewModel.FileName = FileUpload[i].FileName.ToString();
                    tCMultipleFilViewModel.FileUrl = fileUrl + FileUpload[i].FileName.ToString();
                    FileList.Add(tCMultipleFilViewModel);
                }

                return PartialView("TCMultipleFile", FileList);
                //return Json(new { Data = fileName, StatusCode = 200, Message = "File Uploded successfully" });
            }
            else
            {
                return PartialView("TCMultipleFile", FileList);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string filePath, int UserId)
        {

            var user = await _applicationUserService.GetUserProfile(UserId);
            PdfReader pdfReader = new PdfReader(filePath);
            string fileName = $"termsandcondition-{UserId.ToString()}-{DateTime.UtcNow.ToUnixEpochDate().ToString()}.pdf";

            MemoryStream ms = new();
            PdfStamper pdfStamper = new PdfStamper(pdfReader, ms);
            AcroFields pdfFormFields = pdfStamper.AcroFields;
            pdfFormFields.RemoveXfa();

            var termscondition = await _termsConditionService.GetTermsCondition_ByCountryId(user.CountryId);

            if (termscondition.Exists(x => x.FileUrl.ToLower().Equals(filePath.ToLower()) && x.AutoFill))
            {
                if (pdfFormFields.GetFieldItem("topmostSubform[0].Page1[0].f_1[0]") != null)
                {
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f_1[0]", user.Name + " " + user.Surname);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_1[0]", "textcolor", BaseColor.BLACK, null);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_1[0]", "textsize", 12f, null);
                    pdfFormFields.RegenerateField("topmostSubform[0].Page1[0].f_1[0]");
                }

                if (pdfFormFields.GetFieldItem("topmostSubform[0].Page1[0].f_2[0]") != null)
                {
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f_2[0]", "India");
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_2[0]", "textcolor", BaseColor.BLACK, null);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_2[0]", "textsize", 12f, null);
                    pdfFormFields.RegenerateField("topmostSubform[0].Page1[0].f_2[0]");
                }

                if (pdfFormFields.GetFieldItem("topmostSubform[0].Page1[0].f_3[0]") != null)
                {
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f_3[0]", string.IsNullOrEmpty(user.Address) ? user.Address2 : user.Address);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_3[0]", "textcolor", BaseColor.BLACK, null);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_3[0]", "textsize", 12f, null);
                    pdfFormFields.RegenerateField("topmostSubform[0].Page1[0].f_3[0]");
                }

                if (pdfFormFields.GetFieldItem("topmostSubform[0].Page1[0].f_4[0]") != null)
                {
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f_4[0]", user.CountryCode);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_4[0]", "textcolor", BaseColor.BLACK, null);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_4[0]", "textsize", 12f, null);
                    pdfFormFields.RegenerateField("topmostSubform[0].Page1[0].f_4[0]");
                }

                if (pdfFormFields.GetFieldItem("topmostSubform[0].Page1[0].f_5[0]") != null)
                {
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f_5[0]", user.CountryCode);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_5[0]", "textcolor", BaseColor.BLACK, null);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_5[0]", "textsize", 12f, null);
                    pdfFormFields.RegenerateField("topmostSubform[0].Page1[0].f_5[0]");
                }

                if (pdfFormFields.GetFieldItem("topmostSubform[0].Page1[0].f_11[0]") != null)
                {
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f_11[0]", user.UserName);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_11[0]", "textcolor", BaseColor.BLACK, null);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_11[0]", "textsize", 12f, null);
                    pdfFormFields.RegenerateField("topmostSubform[0].Page1[0].f_11[0]");
                }

                if (pdfFormFields.GetFieldItem("topmostSubform[0].Page1[0].f_12[0]") != null)
                {
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f_12[0]", user.DateOfBirth.ToString("dd-MM-yyyy"));
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_12[0]", "textcolor", BaseColor.BLACK, null);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_12[0]", "textsize", 12f, null);
                    pdfFormFields.RegenerateField("topmostSubform[0].Page1[0].f_12[0]");
                }

                //Sign Here
                if (pdfFormFields.GetFieldItem("topmostSubform[0].Page1[0].Date[0]") != null)
                {
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].Date[0]", DateTime.Now.ToString("dd-MM-yyyy"));
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].Date[0]", "textcolor", BaseColor.BLACK, null);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].Date[0]", "textsize", 12f, null);
                    pdfFormFields.RegenerateField("topmostSubform[0].Page1[0].Date[0]");
                }

                //Signature of beneficial owner (or individual authorized to sign for beneficial owner)
                if (pdfFormFields.GetFieldItem("topmostSubform[0].Page1[0].f_21[0]") != null)
                {
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f_21[0]", user.Name + " " + user.Surname);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_21[0]", "textcolor", BaseColor.BLACK, null);
                    pdfFormFields.SetFieldProperty("topmostSubform[0].Page1[0].f_21[0]", "textsize", 12f, null);
                    pdfFormFields.RegenerateField("topmostSubform[0].Page1[0].f_21[0]");
                }
            }

            pdfStamper.FormFlattening = false;
            pdfStamper.Close();
            byte[] byteArray = ms.ToArray();
            var myfile = byteArray;
            return File(myfile, "application/pdf");
        }

        [HttpPost]
        public IActionResult RemoveFile(string FileUrl, int Id)
        {
            if (FileUrl != null)
            {
                var accountName = this._Configuration.GetSection("DocumentUploadCred")["BlobAccountName"];
                var accountKey = this._Configuration.GetSection("DocumentUploadCred")["BlobAccountKey"];
                var containerName = this._Configuration.GetSection("DocumentUploadCred")["Blobcontainername"];
                var fileUrl = this._Configuration.GetSection("DocumentUploadCred")["FileUrl"];
                var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(FileUrl);

                cloudBlockBlob.DeleteIfExistsAsync();
            }
            if (Id > 0)
            {
                var res = _termsConditionService.RemovePdfFile(Id, FileUrl);
            }
            return Json(new { Data = "", StatusCode = 200, Message = "File removed successfully." });
        }
        public void GetCountry()
        {
            var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName });

            ViewBag.CountryList = country;
        }
    }
}
