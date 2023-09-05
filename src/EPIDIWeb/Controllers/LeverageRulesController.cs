using Epidi.Models.Paging;
using Epidi.Models.ViewModel.LeverageRule;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Service.BDM;
using Epidi.Service.CountryService;
using Epidi.Service.IBService;
using Epidi.Service.LegalEntityService;
using Epidi.Service.LeverageRulesService;
using Epidi.Service.UsersService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace EPIDIWeb.Controllers
{
    public class LeverageRulesController : Controller
    {
        private readonly IUsersService _userService;
        private readonly ILeverageRulesService _leverageRulesService;
        private readonly ICountryService _countryService;
        private readonly IIBService _iIBService;
        private readonly IBDMService _IBDMService;
        private readonly ILegalEntityService _LegalEntityService;


        public LeverageRulesController(IUsersService usersService, ILeverageRulesService leverageRulesService,ICountryService countryService, IIBService iBService, IBDMService iBDMService, ILegalEntityService legalEntityService)
        {
            _userService = usersService;
            _leverageRulesService = leverageRulesService;
            _countryService = countryService;
            _iIBService = iBService;
            _IBDMService = iBDMService;
            _LegalEntityService = legalEntityService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddRule()
        {
            GetUserFields();
            GetCountryDropDown();
            GetIBDropDown();
            return View(new LeverageRulesViewModel());
        }
        public void GetUserFields()
        {
            var FieldsList = _userService.GetUserFields("Users").Result.Select(p => new SelectListItem()
            {
                Value = p.ColumnName.ToString() + "|" + p.DataType.ToString() + "|" + p.ForeignKey.ToString(),
                Text = p.ColumnName
            });

            ViewBag.UserFieldsList = FieldsList;
        }
        public async Task<JsonResult> GetUserFieldsForDynamic(string tblName)
        {
            List<SelectListItem> FildsName = new List<SelectListItem>();
            var result = await _userService.GetUserFields(tblName);
            var FieldsList = result.Select(p => new SelectListItem()
            {
                Value = p.ColumnName.ToString() + "|" + p.DataType.ToString() + "|" + p.ForeignKey.ToString(),
                Text = p.ColumnName
            });
            foreach (var item in FieldsList)
            {
                FildsName.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return Json(FildsName);
        }
        private List<SelectListItem> GetCountryDropDown()
        {
            List<SelectListItem> countryList = new List<SelectListItem>();

            var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName });
            foreach (var item in country)
            {
                countryList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return countryList;
        }
        private List<SelectListItem> GetIBDropDown()
        {
            List<SelectListItem> ibList = new List<SelectListItem>();
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _iIBService.GetAll(pageParam,"");

            var country = response.Item1.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });
            foreach (var item in country)
            {
                ibList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return ibList;
        }
        private List<SelectListItem> GetBDMDropDown()
        {
            List<SelectListItem> BdmList = new List<SelectListItem>();
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _IBDMService.GetAll(pageParam, "");

            var country = response.Item1.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });
            foreach (var item in country)
            {
                BdmList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return BdmList;
        }
        public List<SelectListItem> GetLegal_EntityNameDropDown()
        {
            List<SelectListItem> legalEntity = new List<SelectListItem>();
            //legalEntity.Add(new SelectListItem { Text = "Johnson and Partners Trading Trust", Value = "1" });
            //legalEntity.Add(new SelectListItem { Text = "EPIDI SLL", Value = "2" });
            //legalEntity.Add(new SelectListItem { Text = "Test", Value = "3" });
            //legalEntity.Add(new SelectListItem { Text = "TATA", Value = "4" });
            //legalEntity.Add(new SelectListItem { Text = "Adani Power", Value = "5" });
            //return legalEntity;

            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _LegalEntityService.GetLegalEntity_All(pageParam, "");

            //var Entity = response.(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });
            foreach (var item in response.Result.Item1)
            {
                legalEntity.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString()});
            }
            return legalEntity;
        }
        public async Task<JsonResult> GetOprationDropDown(string FieldsName, string DataType, string FkKey)
        {
            List<SelectListItem> opration = new List<SelectListItem>();
            if (DataType.ToUpper() == "STRING")
            {
                opration.Add(new SelectListItem { Text = "Contain", Value = "Contain" });
                opration.Add(new SelectListItem { Text = "Not Contain", Value = "NotContain" });
                opration.Add(new SelectListItem { Text = "Equal", Value = "Equal" });
            }
            else if (DataType.ToUpper() == "NUMBER" && FkKey == "0")
            {
                opration.Add(new SelectListItem { Text = "Equal", Value = "Equal" });
                opration.Add(new SelectListItem { Text = "Less Than (<)", Value = "LessThan" });
                opration.Add(new SelectListItem { Text = "Greater Than (>)", Value = "GreaterThan" });
            }
            else if (DataType.ToUpper() == "NUMBER" && FkKey == "1")
            {
                opration.Add(new SelectListItem { Text = "Equal", Value = "Equal" });                
                opration.Add(new SelectListItem { Text = "Not Equal", Value = "NotEqual" });
            }
            return Json(opration);
        }
        public async Task<JsonResult> GetTempOprationDropDown(string FieldsName, string DataType, string FkKey)
        {
            List<SelectListItem> opration = new List<SelectListItem>();
            if (FieldsName.ToUpper() == "COUNTRYID")
            {
                opration = GetCountryDropDown();
            }
            else if (FieldsName.ToUpper() == "LEGALENTITYNAME")
            {
                opration = GetLegal_EntityNameDropDown();
            }
            else if (FieldsName.ToUpper() == "IBID")
            {
                opration = GetIBDropDown();
            }
            else if (FieldsName.ToUpper() == "BDMID")
            {
                opration = GetBDMDropDown();
            }
            return Json(opration);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SaveLeverageRules(LeverageRulesViewModel model)
        {
            try
            {
                model.leverageRulesDtlViewModels = JsonConvert.DeserializeObject<List<LeverageRulesDtlViewModel>>(model.leverageRulesDtlObj);
                if (model != null)
                {
                    var res = _leverageRulesService.Upsert(model);
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
        [HttpPost]
        public JsonResult LeverageRules_All()
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();

                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _leverageRulesService.GetAll(pageParam, search);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var response = await _leverageRulesService.GetById(Id);
            //response.FieldName =    
            GetUserFields();
            if (response != null)
            {
                return View("AddRule", response);
            }
            else
            {
                return View(new LeverageRulesViewModel());
            }
        }
        //public IActionResult ShowUsers()
        //{
        //    return View();
        //}
        [HttpPost]
        public JsonResult ShowUsersList(string Id)
        {
            if (Id != "")
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();

                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _userService.GetUsersVsRole_ByRulsId(pageParam, Id);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;
                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            }
            else
            {
                return Json("");
            }
        }
        [HttpPost]
        public IActionResult Delete_UserVsRules(int UserId)
        {
            if (UserId > 0)
            {
                var res = _userService.DeleteUserVsRules(UserId);
                return Json(res);
            }
            else
            {
                return Json("");
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetDetailsById(string Id)
        {
            var response = await _leverageRulesService.GetById(Convert.ToInt32(Id));
            return Json(response);
        }
    }
}
