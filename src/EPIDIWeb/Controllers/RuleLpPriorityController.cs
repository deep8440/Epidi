using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.RuleLpPriority;
using Epidi.Service.MasterLPService;
using Epidi.Service.QuoteMarkUpRuleService;
using Epidi.Service.RuleLpPriorityService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPIDIWeb.Controllers
{
    public class RuleLpPriorityController : Controller
    {
        private readonly IRuleLpPriorityService _ruleLpPriorityService;
        private readonly IQuoteMarkUpRuleService _IQuoteMarkUpRuleService;
        private readonly IMasterLPService _masterLPService;

        public RuleLpPriorityController(IRuleLpPriorityService ruleLpPriorityService, 
            IQuoteMarkUpRuleService iQuoteMarkUpRuleService, IMasterLPService masterLPService)
        {
            _ruleLpPriorityService = ruleLpPriorityService;
            _IQuoteMarkUpRuleService = iQuoteMarkUpRuleService;
            _masterLPService = masterLPService;
        }

        public IActionResult Index()
        {
            return View();
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
            var response = _ruleLpPriorityService.GetAll(pageParam, search);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        public IActionResult AddRuleLpPriority()
        {
            GetQuoteMarkUpRuleList();
            GetMarketLP();
            return View(new RuleLpPriorityViewModel());
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            GetQuoteMarkUpRuleList();
            GetMarketLP();
            var response = await _ruleLpPriorityService.GetById(Id);

            if (response != null)
            {
                return View("AddRuleLpPriority", response);
            }
            else
            {
                return View(new RuleLpPriorityViewModel());
            }
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var res = _ruleLpPriorityService.Delete(Id);
            return Json(res);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveRuleLpPriority(RuleLpPriorityViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {
                    var res = await _ruleLpPriorityService.Upsert(model);
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "RuleLpPriority Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "RuleLpPriority Not Saved";
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
        private void GetMarketLP()
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _masterLPService.GetAll(pageParam, "");
            var FieldsList = response.Item1.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });
            ViewBag.MarketLPDropDown = FieldsList;
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
        [HttpGet]
        public async Task<JsonResult> Get_ByRuleId(int Rule_Id)
        {
            var response = await _ruleLpPriorityService.GetByRuleId(Rule_Id);
            return Json(response);
        }
    }
}
