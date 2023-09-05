using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Service.QuoteMarkUpRuleService;
using Epidi.Service.RuleConditionService;
using Epidi.Service.UsersService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace EPIDIWeb.Controllers
{
    public class RuleConditionController : Controller
    {
        private readonly IRuleConditionService _ruleConditionService;
        private readonly IQuoteMarkUpRuleService _IQuoteMarkUpRuleService;
        private readonly IUsersService _userService;
        public RuleConditionController(IRuleConditionService ruleConditionService,
            IQuoteMarkUpRuleService iQuoteMarkUpRuleService, IUsersService userService)
        {
            _ruleConditionService = ruleConditionService;
            _IQuoteMarkUpRuleService = iQuoteMarkUpRuleService;
            _userService = userService;
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
            var response = _ruleConditionService.GetAll(pageParam, search);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        public IActionResult AddRuleCondition()
        {
            GetQuoteMarkUpRuleList();
            GetUserFields();
            GetOprationList("STRING");
            return View(new RuleConditionViewModel());
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            GetQuoteMarkUpRuleList();
            GetUserFields();
            GetOprationList("STRING");
            var response = await _ruleConditionService.GetById(Id);

            if (response != null)
            {
                return View("AddRuleCondition", response);
            }
            else
            {
                return View(new RuleConditionViewModel());
            }
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var res = _ruleConditionService.Delete(Id);
            return Json(res);
        }
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveRuleCondition(RuleConditionViewModel model)
        {
            try
            {
                model.rulesDtlViewModel = JsonConvert.DeserializeObject<List<LeverageRulesDtlViewModel>>(model.conditionRulesDtlObj);
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model.Id != 0)
                {
                    var res = await _ruleConditionService.Upsert(model);
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "RuleCondition Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "RuleCondition Not Saved";
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
        public async void GetUserFields()
        {
            var response = await _userService.GetUserFields("Users");
            var FieldsList = response.Select(p => new SelectListItem()
            {
                Value = p.ColumnName.ToString() + "|" + p.DataType.ToString() + "|" + p.ForeignKey.ToString(),
                Text = p.ColumnName
            });
            ViewBag.UserFieldsList = FieldsList;
        }
        public async void GetOprationList(string DataType)
        {
            List<SelectListItem> opration = new List<SelectListItem>();
            if (DataType.ToUpper() == "STRING")
            {
                opration.Add(new SelectListItem { Text = "Contain", Value = "Contain" });
                opration.Add(new SelectListItem { Text = "Not Contain", Value = "NotContain" });
                opration.Add(new SelectListItem { Text = "Equal", Value = "Equal" });
            }
            else if (DataType.ToUpper() == "NUMBER")
            {
                opration.Add(new SelectListItem { Text = "Equal", Value = "Equal" });
                opration.Add(new SelectListItem { Text = "Less Than (<)", Value = "LessThan" });
                opration.Add(new SelectListItem { Text = "Greater Than (>)", Value = "GreaterThan" });
            }
            ViewBag.OprationList = opration;
        }
        [HttpGet]
        public async Task<JsonResult> GetDetailsById(string Id)
        {
            var response = await _ruleConditionService.GetById(Convert.ToInt32(Id));
            return Json(response);
        }
    }
}
