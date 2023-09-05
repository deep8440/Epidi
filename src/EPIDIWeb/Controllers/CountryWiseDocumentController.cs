using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Margin;
using Epidi.Models.ViewModel.Question;
using Epidi.Service.CountryService;
using Epidi.Service.CountryWiseDocumentService;
using Epidi.Service.QuestionService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace EPIDIWeb.Controllers
{
    public class CountryWiseDocumentController : Controller
    {
        private readonly ICountryWiseDocumentService _countryWiseDocumentService;
        private readonly ICountryService _countryService;
        public CountryWiseDocumentController(ICountryWiseDocumentService countryWiseDocumentService, ICountryService countryService)
        {
            _countryWiseDocumentService = countryWiseDocumentService;
            _countryService = countryService;
        }
        public IActionResult Index()
        {

            ViewBag.Title = "CountryWiseDocument  List";
            ViewBag.Method = "Document";
            return View();
        }


        [HttpPost]
        public JsonResult GetCountryWiseDocument_All()
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();
                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();


                var response = _countryWiseDocumentService.GetCountryWiseDocumentList(pageParam, search, sortColumn, sortColumnDirection);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SaveCountryWiseDocument(CountryWiseDocumentViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var res = _countryWiseDocumentService.SaveCountryWiseDocument(model);
                    return Json(res);
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

        
        public async Task<JsonResult> CountryWiseDocument_Upsert(string countryWiseDocumentDataStr)
        {
            try
            {
                if (countryWiseDocumentDataStr != null)
                {
                    var CountryWiseDocumentList = JsonConvert.DeserializeObject<List<CountryWiseDocumentViewModel>>(countryWiseDocumentDataStr);
                    var res = await _countryWiseDocumentService.CountryWiseDocument_Upsert(CountryWiseDocumentList);
                    return Json(res);
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
        public IActionResult AddCountryWiseDocument()
        {
            GetCountry();
            return View(new CountryWiseDocumentViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var questions = await _countryWiseDocumentService.GetCountryWiseDocumentById(Id);
            GetCountry();
            if (questions != null)
            {
                return View("EditCountryWiseDocument", questions);
            }
            else
            {
                return View(new CountryWiseDocumentViewModel());
            }
        }

        [HttpGet]
        public IActionResult DeleteCountryWiseDocument(int Id)
        {
            var res = _countryWiseDocumentService.DeleteCountryWiseDocument(Id);
            return Json(res);
        }

        [HttpPost]
        public IActionResult CopyCountryData(int Id,int FromCountryId)
        {
            GetCountry();
            CopyCountryDataViewModel copyCountryDataViewModel = new CopyCountryDataViewModel();
            copyCountryDataViewModel.Id = Id;
            copyCountryDataViewModel.FromCountry = FromCountryId;

            return View(copyCountryDataViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SaveCopyCountryWiseDocument(CopyCountryDataViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var res = _countryWiseDocumentService.CountryWiseDocument_CopyData(model).Result;
                    return Json(res);
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


        public void GetCountry()
        {
            var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName });

            ViewBag.CountryList = country;
        }
    }
}
