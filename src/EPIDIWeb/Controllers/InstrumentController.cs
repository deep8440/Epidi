using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Service.CountryService;
using Epidi.Service.InstrumentService;
using Microsoft.AspNetCore.Mvc;

namespace EPIDIWeb.Controllers
{
    public class InstrumentController : Controller
    {
        private readonly IInstrumentService _instrumentService;
        public InstrumentController(IInstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Instrument_All()
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();
                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _instrumentService.GetInstruments();

                totalRecord = response.Result.Count;
                filteredRecord = response.Result.Count;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Result });
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddMapping(InstrumentMasterMapping instrumentMasterMapping)
        {
            var response = await _instrumentService.AddMapping(instrumentMasterMapping);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteMapping(int id)
        {
            var response = await _instrumentService.RemoveMapping(id);
            return Json(response);
        }
        public async Task<JsonResult> GetGateIO()
        {
            var response = await _instrumentService.GetGateIOs();
            return Json(response);
        }
        public async Task<JsonResult> GetLMax()
        {
            var response = await _instrumentService.GetLMaxes();
            return Json(response);
        }
        
    }
}
