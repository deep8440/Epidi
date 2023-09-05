using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.QuoteTiming;
using Epidi.Service.BDM;
using Epidi.Service.CountryService;
using Epidi.Service.InstrumentService;
using Epidi.Service.QuoteTimingService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPIDIWeb.Controllers
{
    public class QuoteTimingController : Controller
    {
        private readonly IQuoteTimingService _quoteTimingService;
        private readonly IBDMService _BDMService;
        public QuoteTimingController(IQuoteTimingService quoteTimingService, IBDMService bDMService)
        {
            _quoteTimingService = quoteTimingService;
            _BDMService = bDMService;
        }
        public IActionResult Index()
        {
            GetInstruments();
            return View(new BDMViewModel());
        }
        [HttpPost]
        public JsonResult QuoteTiming_All()
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();
                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _quoteTimingService.GetQuoteTiming();

                totalRecord = response.Result.Count;
                filteredRecord = response.Result.Count;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Result });
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddQuoteTiming(QuoteTiming quoteTiming)
        {
            quoteTiming.CreatedDate = DateTime.Now;
            quoteTiming.CreatedBy = 1;
            var response=await _quoteTimingService.AddQuoteTiming(quoteTiming);
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> EditQuoteTiming(QuoteTiming quoteTiming)
        {
            var response = await _quoteTimingService.EditQuoteTiming(quoteTiming);
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteQuoteTiming(int id)
        {
            var response =await _quoteTimingService.RemoveQuoteTiming(id);
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> GetQuoteTimingById(int Id)
        {
            var response = await _quoteTimingService.GetQuoteTimingById(Id);
            return Json(response);
        }

        public void GetInstruments()
        {
            var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });

            ViewBag.InstrumentList = FieldsList;
        }
    }
}
