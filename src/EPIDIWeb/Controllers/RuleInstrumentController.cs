using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.RuleInstrument;
using Epidi.Repository.RuleInstrumentRepository;
using Epidi.Service.BDM;
using Epidi.Service.QuoteMarkUpRuleService;
using Epidi.Service.RuleInstrumentService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPIDIWeb.Controllers
{
    public class RuleInstrumentController : Controller
    {
        private readonly IRuleInstrumentService _ruleInstrumentService;
        private readonly IQuoteMarkUpRuleService _IQuoteMarkUpRuleService;
        private readonly IBDMService _BDMService;
        public RuleInstrumentController(IRuleInstrumentService ruleInstrumentService,
            IQuoteMarkUpRuleService iQuoteMarkUpRuleService,
            IBDMService bDMService
            )
        {
            _ruleInstrumentService = ruleInstrumentService;
            _IQuoteMarkUpRuleService = iQuoteMarkUpRuleService;
            _BDMService = bDMService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddRuleInstrument()
        {
            GetQuoteMarkUpRuleList();
            GetInstruments();
            return View(new RuleInstrumentViewModel());
        }
        [HttpPost]
        public JsonResult Get_All()
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();

            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _ruleInstrumentService.GetAll(pageParam, search);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveRuleInstrument(RuleInstrumentViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {
                    var res = await _ruleInstrumentService.Upsert(model);
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "RuleInstrument Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "RuleInstrument Not Saved";
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
        public async Task<IActionResult> Edit(int Id)
        {
            GetQuoteMarkUpRuleList();
            GetInstruments();
            var response = await _ruleInstrumentService.GetById(Id);

            if (response != null)
            {
                return View("AddRuleInstrument", response);
            }
            else
            {
                return View(new RuleInstrumentViewModel());
            }
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var res = _ruleInstrumentService.Delete(Id);
            return Json(res);
        }
        public void GetQuoteMarkUpRuleList()
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _IQuoteMarkUpRuleService.GetAll(pageParam, "");
            var FieldsList = response.Item1.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });
            ViewBag.MarkUpRuleDropDown = FieldsList;
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
        [HttpGet]
        public async Task<JsonResult> Get_ByRuleId(int Rule_Id)
        {
            var response = await _ruleInstrumentService.GetByRuleId(Rule_Id);
            return Json(response);
        }
    }
}
