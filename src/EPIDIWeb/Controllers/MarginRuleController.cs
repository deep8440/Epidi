using Epidi.Models.Paging;
using Epidi.Service.BDM;
using Epidi.Service.InstrumentService;
using Epidi.Service.MasterLPService;
using Epidi.Service.MarginRuleService;
using Epidi.Service.RuleConditionService;
using Epidi.Service.RuleInstrumentService;
using Epidi.Service.RuleLpPriorityService;
using Microsoft.AspNetCore.Mvc;
using Epidi.Models.ViewModel.MarginRuleViewModel;
using Newtonsoft.Json;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using System;
using System.Data;
using EPIDIWeb.Extensions;
using Epidi.Models.ViewModel.Common;

namespace EPIDIWeb.Controllers
{
    public class MarginRuleController : Controller
    {
        private readonly IMarginRuleService _IMarginRuleService;
        private readonly IRuleConditionService _ruleConditionService;

        public MarginRuleController(IMarginRuleService iMarginRuleService, IRuleConditionService ruleConditionService)
        {
            _IMarginRuleService = iMarginRuleService;
            _ruleConditionService = ruleConditionService;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "MarginRule List";
            return View();
        }

        public IActionResult AddMarginRule()
        {
            return View(new MarginRuleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddMarginRule(MarginRuleViewModel marginRuleViewModel)
        {
            try
            {
               var responseData = await _IMarginRuleService.Upsert(marginRuleViewModel);
                return Json(new { StatusCode = 200, Message = "Margin Rule Added Successfully", Response = responseData });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }
        public async Task<IActionResult> EditMarginRule(int Id)
        {
            var response = await _IMarginRuleService.GetById(Id);
            //var ruleLP = await _ruleLpPriorityService.GetByRuleId(Id);
            var ruleConditions = await _ruleConditionService.GetByMarginRuleId(Id);
            response.objRuleConditionViewModel = ruleConditions;
            if (ruleConditions != null)
            {
                response.objRuleConditionViewModel.conditionRulesDtlObj = JsonConvert.SerializeObject(ruleConditions.rulesDtlViewModel);
            }
            response.mode = "Edit";
            //GetInstruments();
            //GetMarketLP();

            if (response != null)
            {
                return View(response);
            }
            else
            {
                return View(new MarginRuleViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveMarginRule(MarginRuleViewModelEdit model)
        {
            try
            {
                //model.objRuleLpPriorityViewModel = JsonConvert.DeserializeObject<List<RuleLpPriorityViewModel_Dt>>(model.objRuleLpPriorityViewModelstr);
                model.objRuleConditionViewModel = JsonConvert.DeserializeObject<List<LeverageRulesDtlViewModel>>(model.objRuleConditionViewModelstr);
                
                ListToDataTableConverter converter = new ListToDataTableConverter();
                DataTable dtRuleConditionsDtl = converter.ToDataTable(model.objRuleConditionViewModel);
                if (dtRuleConditionsDtl.Rows.Count > 0)
                {
                    RuleConditionViewModel ruleConditionViewModel = new RuleConditionViewModel();
                    ruleConditionViewModel.MarginRuleId = Convert.ToInt32(model.MarginRuleId);
                    ruleConditionViewModel.Id = Convert.ToInt32(dtRuleConditionsDtl.Rows[0]["RuleConditionsId"]);
                    ruleConditionViewModel.rulesDtlViewModel = model.objRuleConditionViewModel;

                    _ruleConditionService.Upsert(ruleConditionViewModel);
                }
                MarginRuleViewModel MarginRuleViewModel = new MarginRuleViewModel
                {
                    MarginRuleId = model.MarginRuleId,
                    RuleName = model.RuleName,
                    Priority = model.Priority,
                    StopOutPercent = model.StopOutPercent,
                    MarginCallPercent = model.MarginCallPercent,
                    LeveltotheStopOut = model.LeveltotheStopOut,
                    IsPartialCloseoutonStopOut = model.IsPartialCloseoutonStopOut
                };
                _IMarginRuleService.Upsert(MarginRuleViewModel);
                 
                return Json(new { StatusCode = 200, Message = "Margin rule updated Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
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
            var response = _IMarginRuleService.GetAll(pageParam, search);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var response = await _IMarginRuleService.GetById(Id);
            //var ruleLP = await _ruleLpPriorityService.GetByRuleId(Id);
            //var ruleConditions = await _ruleConditionService.GetByRuleId(Id);
            //response.objRuleConditionViewModel = ruleConditions;
            //if (ruleConditions != null)
            //{
            //    response.objRuleConditionViewModel.conditionRulesDtlObj = JsonConvert.SerializeObject(ruleConditions.rulesDtlViewModel);
            //}
            response.mode = "Edit";

            if (response != null)
            {
                return View("AddMarginRule", response);
            }
            else
            {
                return View(new MarginRuleViewModel());
            }
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var res = _IMarginRuleService.Delete(Id);
            return Json(res);
        }

        public async Task<IActionResult> DeleteRuleConditionById(int Id, int RuleConditionId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _ruleConditionService.DeleteMarginRuleDtlById(Id, RuleConditionId);
            if (res != null)
            {
                responseViewModel.Code = 200;
                responseViewModel.Message = "Rule Condition Deleted Successfully";
            }
            else
            {
                responseViewModel.Code = 400;
                responseViewModel.Message = "Rule Condition Not Deleted";
            }
            return Json(responseViewModel);
        }
    }
}
