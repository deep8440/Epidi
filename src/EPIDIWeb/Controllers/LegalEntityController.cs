using DataTables.AspNet.AspNetCore;
using DocumentFormat.OpenXml.Office2010.Excel;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.LegalEntity;
using Epidi.Service.CountryService;
using Epidi.Service.InstrumentService;
using Epidi.Service.LegalEntityService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace EPIDIWeb.Controllers
{
    public class LegalEntityController : Controller
    {
        private readonly ILegalEntityService _instrumentService;
        private readonly ICountryService _countryService;

        public LegalEntityController(ILegalEntityService instrumentService, ICountryService countryService)
        {
            _instrumentService = instrumentService;
            _countryService = countryService;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "LegalEntity List";

            return View();
        }
        [HttpPost]
        public JsonResult Country_All()
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 100;
            pageParam.Limit = 100;
            var response = _countryService.GetAllCountriesForAdmin(pageParam, "","","");
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> AddEntity(LegalEntityViewModel obj)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();
            var res = await _instrumentService.CheckNameExist(obj.Name);
            if(res == true) {
                responseViewModel.Code = 400;
                responseViewModel.Message = "Name already exist";
                return Json(responseViewModel);
            }
            var response = await _instrumentService.UpsertLegalEntity(obj);
            responseViewModel.Code = 200;
            responseViewModel.Message = "Data Added";
            return Json(responseViewModel);
        }
        [HttpGet]

        public IActionResult DeleteEntity(int Id)
        {
            var res = _instrumentService.Delete(Id);
            return Json(res);
        }



        //public async Task<JsonResult> DeleteEntity(int id)
        //{
        //    List<ResponseViewModel> responseViewModel = new List<ResponseViewModel>();
        //    ResponseViewModel responseViewModel1 = new ResponseViewModel();
        //    var response = await _instrumentService.Delete(id);
        //    if (response != null)
        //    {
        //        responseViewModel1.Code = 200;
        //        responseViewModel1.Message = "Country Deleted Successfully";
        //    }
        //    else
        //    {
        //        responseViewModel1.Code = 400;
        //        responseViewModel1.Message = "Country Not Deleted";
        //    }
        //    responseViewModel.Add(responseViewModel1);
        //    return Json(responseViewModel);
        //}
        [HttpPost]
        //public async Task<IActionResult> GetLegalEntities()
        //{
        //    var response = await _instrumentService.GetLegalEntity();
        //    return Json(response);
        //}

        [HttpPost]
        public async Task<JsonResult> GetLegalEntity_All()
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
                var response = await _instrumentService.GetLegalEntity_All(pageParam, search);
                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });
            }
        }



    }
}
