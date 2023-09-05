using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.TradeTiming;
using Epidi.Service.BDM;
using Epidi.Service.CountryService;
using Epidi.Service.TradeTimingService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPIDIWeb.Controllers
{
    public class TradeTimingController : Controller
    {
        private readonly ITradeTimingService _tradeTimingService;
        private readonly IBDMService _BDMService;
        public TradeTimingController(ITradeTimingService tradeTimingService, IBDMService bDMService)
        {
            _tradeTimingService = tradeTimingService;
            _BDMService = bDMService;
        }
        public IActionResult Index()
        {
            GetInstruments();
            return View(new BDMViewModel());
        }
        [HttpPost]
        public JsonResult TradeTiming_All()
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();
                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _tradeTimingService.GetTradeTiming();

                totalRecord = response.Result.Count;
                filteredRecord = response.Result.Count;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Result });
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddTradeTiming(TradeTiming tradeTiming )
        {
            tradeTiming.CreatedDate= DateTime.Now;
            var response=await _tradeTimingService.AddTradeTiming(tradeTiming);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> EditTradeTiming(TradeTiming tradeTiming )
        {
            var response=await _tradeTimingService.EditTradeTiming(tradeTiming);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> GetTradeTimingById(int Id)
        {
            var response = await _tradeTimingService.GetTradeTimingById(Id);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteTradeTiming(int id)
        {
            var response =await _tradeTimingService.RemoveTradeTiming(id);
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
