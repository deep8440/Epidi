using System.Data;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.SwapRule;
using Epidi.Service.CountryService;
using Epidi.Service.PromotionService;
using Epidi.Service.RuleConditionService;
using Epidi.Service.UsersService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SendGrid;

namespace EPIDIWeb.Controllers
{
    public class PromotionController : Controller
    {
        private readonly IPromotionService _promotionService;
        private readonly ICountryService _countryService;
        private readonly IUsersService _userService;
        private readonly IRuleConditionService _ruleConditionService;
        public PromotionController(IPromotionService promotionService, ICountryService countryService,IUsersService usersService, IRuleConditionService ruleConditionService)
        {
            _promotionService = promotionService;
            _countryService = countryService;
            _userService = usersService;
            _ruleConditionService = ruleConditionService;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Promotion List";
            return View();
        }
        public async Task<ActionResult> AddPromotion(int? Id)
        {
            var model = new Promotion();
            if (Id != null && Id > 0)
            {
                model = _promotionService.GetPromotion(Id ?? 0);
                var rules=await _promotionService.GetRuleConditions(Id ?? 0);
                rules.ForEach(x => x.Value = x.FieldName + '|' + x.Datatype + '|' + x.ForeignKey);
                model.ObjRuleConditions=JsonConvert.SerializeObject(rules);
            }
            else { model = new Promotion(); }
            PageParam pageParam = new PageParam() { Limit = 10000000, Offset = 0 };
            var countries = await _countryService.GetCountryData();
            var result = await _userService.GetUserFields("Users");
            
            var FieldsList = result.Select(p => new SelectListItem()
            {
                Value = p.ColumnName.ToString() + "|" + p.DataType.ToString() + "|" + p.ForeignKey.ToString(),
                Text = p.ColumnName
            }).ToList();
            
            ViewBag.Fields = FieldsList;
            ViewBag.JsonFields = JsonConvert.SerializeObject(FieldsList);
            ViewBag.Id = Id;

            ViewBag.Countries = countries;
            return View(model);
        }
        [HttpPost]
        public ActionResult GetAllPromotion()
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();
            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _promotionService.GetAllPromotion(pageParam, search);
            totalRecord = response.Item2;
            filteredRecord = response.Item1.Count;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        [HttpPost]
        public ActionResult CreditPromo_All(int Id)
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();
            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _promotionService.creditPromotions(pageParam, search, Id);
            totalRecord = response.Item2;
            filteredRecord = response.Item1.Count;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        [HttpPost]
        public ActionResult VoucherPromo_All(int Id)
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();
            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _promotionService.voucherPromotions(pageParam, search, Id);
            totalRecord = response.Item2;
            filteredRecord = response.Item1.Count;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        [HttpPost]
        public ActionResult FreeAssetPromo_All(int Id)
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();
            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _promotionService.freeAssetPromotions(pageParam, search, Id);
            totalRecord = response.Item2;
            filteredRecord = response.Item1.Count;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        [HttpPost]
        public ActionResult ReferFriendPromo_All(int Id)
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();
            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _promotionService.refPromotions(pageParam, search, Id);
            totalRecord = response.Item2;
            filteredRecord = response.Item1.Count;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        [HttpPost]
        public ActionResult RebatePromo_All(int Id)
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();
            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _promotionService.rebatePromotions(pageParam, search, Id);
            totalRecord = response.Item2;
            filteredRecord = response.Item1.Count;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        [HttpPost]
        public async Task<ActionResult> PromotionAddUpdate(Promotion promotion)
        {
            var response = await _promotionService.UpsertPromotion(promotion);
            return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
        }
        [HttpPost]
        public async Task<ActionResult> AddRuleConditions(Promotion promotion)
        {

            try
            {
                promotion.objRuleConditionViewModel = JsonConvert.DeserializeObject<List<LeverageRulesDtlViewModel>>(promotion.ObjRuleConditions);
                //promotion.objRuleInstrumentViewModel = JsonConvert.DeserializeObject<List<SwapRuleInstrumentViewModel>>(promotion.objRuleInstrumentViewModelstr);

                ListToDataTableConverter converter = new ListToDataTableConverter();

                //DataTable dtRuleInstrument = converter.ToDataTable(promotion.objRuleInstrumentViewModel);
                //DataTable dtRuleLpPriority = new DataTable();
                DataTable dtRuleConditionsDtl = converter.ToDataTable(promotion.objRuleConditionViewModel);

                if (dtRuleConditionsDtl.Rows.Count > 0)
                {
                    RuleConditionViewModel ruleConditionViewModel = new RuleConditionViewModel();
                    ruleConditionViewModel.PromotionRuleId = Convert.ToInt32(promotion.Id);
                    ruleConditionViewModel.Id = Convert.ToInt32(dtRuleConditionsDtl.Rows[0]["Id"]);
                    ruleConditionViewModel.rulesDtlViewModel = promotion.objRuleConditionViewModel;

                    _ruleConditionService.Upsert(ruleConditionViewModel);
                }
                var res1 = await _promotionService.UpsertPromotionRuleWithRuleCondition(promotion);
                promotion.RuleConditionId = res1.Id;
                //var res2=await _ruleConditionService.Upsert(ruleConditionViewModel);
                return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> VoucherAddUpdate(VoucherPromotion promotion)
        {
            var response = await _promotionService.UpsertVoucherPromotion(promotion);
            return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
        }
        [HttpPost]
        public ActionResult DeletePromo(int Id)
        {
            var response =  _promotionService.DeleteCreditPromotion(Id);
            return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
        }
        [HttpPost]
        public ActionResult DeleteReferFriendPromo(int Id)
        {
            var response = _promotionService.DeleteReferFriendPromotion(Id);
            return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
        }
        [HttpPost]
        public ActionResult DeleteFreeAssetPromo(int Id)
        {
            var response = _promotionService.DeleteFreeAssetPromotion(Id);
            return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
        }
        [HttpPost]
        public ActionResult DeleteVoucherPromo(int Id)
        {
            var response = _promotionService.DeleteVoucherPromotion(Id);
            return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
        }
        [HttpPost]
        public ActionResult DeleteRebatePromo(int Id)
        {
            var response = _promotionService.DeleteRebatePromotion(Id);
            return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
        }
        [HttpPost]
        public async Task<ActionResult> ReferFriendPromoAddUpdate(ReferPromotion referFriendPromotion)
        {
            var response = await _promotionService.UpsertReferFriendPromotion(referFriendPromotion);
            return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
        }
        [HttpPost]
        public async Task<ActionResult> FreeAssetPromoAddUpdate(FreeAssetPromotion freeAssetPromotion)
        {
            var response = await _promotionService.UpsertFreeAssetPromotion(freeAssetPromotion);
            return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
        }
        [HttpPost]
        public async Task<ActionResult> RebatePromoAddUpdate(RebatePromotion rebatePromotion)
        {
            var response = await _promotionService.UpsertRebatePromotion(rebatePromotion);
            return Json(new { StatusCode = 200, Success = true, Message = "Data Updated Successfully" });
        }
    }
}
